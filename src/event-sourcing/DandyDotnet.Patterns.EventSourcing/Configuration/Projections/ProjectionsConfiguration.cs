using System.Collections.Concurrent;
using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Projections;

public sealed class ProjectionsConfiguration
{
    internal ConcurrentDictionary<Type, ProjectionConfiguration> ProjectionConfigsByType { get; } = new();
    internal ConcurrentDictionary<string, ProjectionConfiguration> ProjectionConfigsByKey { get; } = new();
    internal ConcurrentDictionary<string, ProjectionConfiguration> ProjectionConfigsByEventKey { get; } = new();

    public IReadOnlyDictionary<Type, ProjectionConfiguration> ProjectionByType => ProjectionConfigsByType;
    public IReadOnlyDictionary<string, ProjectionConfiguration> ProjectionByKey => ProjectionConfigsByKey;
    public IReadOnlyDictionary<string, ProjectionConfiguration> ProjectionByEventKey => ProjectionConfigsByEventKey;

    internal ProjectionConfiguration GetOrAddProjectionConfiguration(Type projectionType)
    {
        return ProjectionConfigsByType.GetOrAdd(projectionType, type =>
        {
            var configuration = CreateProjectionConfiguration(type, type.GetCustomAttribute<ProjectionAttribute>());
            return ProjectionConfigsByKey[configuration.Key] = configuration;
        });
    }

    internal ProjectionConfiguration CreateProjectionConfiguration(Type projectionType, ProjectionAttribute? attribute)
    {
        var configuration = new ProjectionConfiguration
        {
            Key = attribute?.Key ?? projectionType.Name,
            RuntimeType = projectionType,
            Mode = attribute?.Mode ?? ProjectionMode.Async,
        };

        SetFactoryFromMethod(configuration);
        SetFactoryFromConstructor(configuration);
        SetKeyFactory(configuration);

        return configuration;
    }

    private static void SetFactoryFromMethod(ProjectionConfiguration configuration)
    {
        var factoryMethod = configuration.RuntimeType
            .GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
            .Where(method => method.IsDefined(typeof(ProjectionFactoryAttribute), inherit: false))
            .Where(method => method.ReturnType == configuration.RuntimeType)
            .SingleOrDefault(method => HasFactoryParameters(method.GetParameters(), configuration.RuntimeType));

        if (factoryMethod != null)
            configuration.FactoryFunc = (snapshot, envelopes) => factoryMethod.Invoke(null, [snapshot, envelopes])!;
    }

    private static void SetFactoryFromConstructor(ProjectionConfiguration configuration)
    {
        // Prioritize factory methods so don't overwrite it
        if (configuration.FactoryFunc != null)
            return;

        var factoryConstructor = configuration.RuntimeType
            .GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(constructor => constructor.IsDefined(typeof(ProjectionFactoryAttribute), inherit: false))
            .SingleOrDefault(constructor => HasFactoryParameters(constructor.GetParameters(), configuration.RuntimeType));

        if (factoryConstructor != null)
            configuration.FactoryFunc = (snapshot, envelopes) => factoryConstructor.Invoke([snapshot, envelopes])!;
    }

    private static void SetKeyFactory(ProjectionConfiguration configuration)
    {
        if (configuration.KeyFactoryFunc != null)
            return;

        var keyProperties = configuration.RuntimeType
            .GetProperties()
            .Where(p => p.GetCustomAttribute<ProjectionKeyAttribute>() != null)
            .ToArray();

        if (keyProperties.Length == 0)
            throw new InvalidOperationException($"No key properties were defined for projection '{configuration.Key}'.");

        configuration.KeyFactoryFunc = projection =>
        {
            var keyValues = new List<(string Name, object Value)>();
            foreach (var keyProperty in keyProperties)
            {
                var keyValue = keyProperty.GetValue(projection);
                if (keyValue == null)
                    throw new InvalidOperationException($"Projection '{configuration.Key}' had a key with a value of 'null' for key property '{keyProperty.Name}'.");

                keyValues.Add((keyProperty.Name, keyValue));
            }

            return keyValues;
        };
    }

    private static bool HasFactoryParameters(ParameterInfo[] parameters, Type projectionType)
    {
        return parameters.Length == 2 &&
               parameters[0].ParameterType == projectionType &&
               parameters[1].ParameterType == typeof(IReadOnlyEnvelope[]);
    }
}