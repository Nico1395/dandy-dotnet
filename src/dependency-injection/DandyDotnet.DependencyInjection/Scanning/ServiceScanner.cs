using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

public sealed class ServiceScanner(IReadOnlyDictionary<Type, ScanDescriptor> descriptors, Assembly[] assemblies)
{
    public IReadOnlyDictionary<Type, ScanDescriptor> Descriptors { get; } = descriptors;
    public Assembly[] Assemblies { get; } = assemblies;

    public IEnumerable<ServiceDescriptor> GetServiceDescriptors()
    {
        // Ruling out value types and abstract types
        var implementationTypes = Assemblies
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(type => type is { IsClass: true, IsAbstract: false })
            .ToArray();

        foreach (var descriptor in Descriptors.Values)
        {
            var filteredTypes = implementationTypes
                .Where(type => descriptor.AllowOpenGeneric == type.IsGenericTypeDefinition) // Match open generic setting
                .Where(type => descriptor.Predicate?.Invoke(type) ?? true); // Apply predicate

            foreach (var filteredType in filteredTypes)
            {
                var serviceTypes = GetServiceTypes(descriptor.AbstractType, filteredType, descriptor.AllowOpenGeneric);
                foreach (var serviceType in serviceTypes)
                {
                    var serviceKey = descriptor.ServiceKeyFactory is null
                        ? descriptor.ServiceKey
                        : descriptor.ServiceKeyFactory(filteredType);

                    yield return CreateServiceDescriptor(
                        descriptor,
                        serviceType,
                        filteredType,
                        serviceKey);
                }
            }
        }
    }

    private static IEnumerable<Type> GetServiceTypes(Type abstractType, Type implementationType, bool isOpenGeneric)
    {
        if (isOpenGeneric)
        {
            if (ImplementsGenericType(implementationType, abstractType))
                yield return abstractType;

            yield break;
        }

        if (!abstractType.IsGenericTypeDefinition && abstractType.IsAssignableFrom(implementationType))
        {
            yield return abstractType;
            yield break;
        }

        foreach (var implementedType in GetImplementedTypes(implementationType))
        {
            if (implementedType.IsGenericType && abstractType.IsGenericTypeDefinition && implementedType.GetGenericTypeDefinition() == abstractType)
                yield return implementedType;
        }
    }

    private static bool ImplementsGenericType(Type implementationType, Type abstractType)
    {
        if (!abstractType.IsGenericTypeDefinition)
            return abstractType.IsAssignableFrom(implementationType);

        return GetImplementedTypes(implementationType).Any(type =>
            type.IsGenericType &&
            type.GetGenericTypeDefinition() == abstractType);
    }

    private static IEnumerable<Type> GetImplementedTypes(Type implementationType)
    {
        foreach (var implementedType in implementationType.GetInterfaces())
            yield return implementedType;

        for (var baseType = implementationType.BaseType; baseType is not null; baseType = baseType.BaseType)
            yield return baseType;
    }

    private static ServiceDescriptor CreateServiceDescriptor(ScanDescriptor descriptor, Type serviceType, Type implementationType, object? serviceKey)
    {
        // Non-keyed descriptor
        if (!descriptor.IsKeyed())
        {
            // Factory
            if (descriptor.Factory != null)
            {
                return ServiceDescriptor.Describe(
                    serviceType,
                    descriptor.Factory,
                    descriptor.Lifetime);
            }

            // Implementation type
            return ServiceDescriptor.Describe(
                serviceType,
                implementationType,
                descriptor.Lifetime);
        }

        // Factory with key
        if (descriptor.KeyedFactory != null)
        {
            return ServiceDescriptor.DescribeKeyed(
                serviceType,
                serviceKey,
                descriptor.KeyedFactory,
                descriptor.Lifetime);
        }

        // Factory without key
        if (descriptor.Factory != null)
        {
            return ServiceDescriptor.DescribeKeyed(
                serviceType,
                serviceKey,
                (serviceProvider, _) => descriptor.Factory(serviceProvider),
                descriptor.Lifetime);
        }

        // Implementation type rather than a factory
        return ServiceDescriptor.DescribeKeyed(
            serviceType,
            serviceKey,
            implementationType,
            descriptor.Lifetime);
    }
}