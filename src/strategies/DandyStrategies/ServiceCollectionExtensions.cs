using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DandyStrategies;

public static class ServiceCollectionExtensions
{
    private static readonly IReadOnlyList<Type> _serviceTypes =
    [
        typeof(IStrategy<>),
        typeof(IStrategy<,>),
        typeof(IAsyncStrategy<>),
        typeof(IAsyncStrategy<,>),
    ];

    public static IServiceCollection AddDandyStrategies(this IServiceCollection services, Action<StrategiesConfigurationBuilder>? configuration = null)
    {
        var builder = new StrategiesConfigurationBuilder(services);
        configuration?.Invoke(builder);
        var cfg = builder.Build();

        services.AddTransient<IStrategyExecutor, StrategyExecutor>();
        AddStrategiesByKeyAttributeFromAssemblies(services, cfg.Assemblies);

        return services;
    }

    private static void AddStrategiesByKeyAttributeFromAssemblies(IServiceCollection services, IReadOnlyList<Assembly> assemblies)
    {
        var handlerTypes = assemblies.SelectMany(a => a.DefinedTypes).Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition && t.GetCustomAttribute<StrategyKeyAttribute>() != null);
        foreach (var implementationType in handlerTypes)
        {
            var interfaces = implementationType.ImplementedInterfaces;
            var keyAttribute = implementationType.GetCustomAttribute<StrategyKeyAttribute>();
            if (keyAttribute == null)
                continue;
            
            foreach (var @interface in interfaces)
            {
                if (!@interface.IsGenericType)
                    continue;

                var genericDefinition = @interface.GetGenericTypeDefinition();
                if (_serviceTypes.Contains(genericDefinition))
                    services.AddKeyedTransient(@interface, keyAttribute.Key, implementationType);
            }
        }
    }

    public static IServiceCollection AddStrategyDefinition<TDefinition>(this IServiceCollection services, Action<IStrategyRegistrar<TDefinition>> definition)
        where TDefinition : IStrategyDefinition
    {
        definition(new StrategyRegistrar<TDefinition>(services));
        return services;
    }

    public static IServiceCollection AddStrategyDefinition<TDefinition, TReturn>(this IServiceCollection services, Action<IStrategyRegistrar<TDefinition, TReturn>> definition)
        where TDefinition : IStrategyDefinition<TReturn>
    {
        definition(new StrategyRegistrar<TDefinition, TReturn>(services));
        return services;
    }

    public static IServiceCollection AddStrategyDefinition<TDefinition>(this IServiceCollection services, Action<IAsyncStrategyRegistrar<TDefinition>> definition)
        where TDefinition : IAsyncStrategyDefinition
    {
        definition(new AsyncStrategyRegistrar<TDefinition>(services));
        return services;
    }

    public static IServiceCollection AddStrategyDefinition<TDefinition, TReturn>(this IServiceCollection services, Action<IAsyncStrategyRegistrar<TDefinition, TReturn>> definition)
        where TDefinition : IAsyncStrategyDefinition<TReturn>
    {
        definition(new AsyncStrategyRegistrar<TDefinition, TReturn>(services));
        return services;
    }
}
