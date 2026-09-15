using System.Reflection;
using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.Patterns.Strategies.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies;

public static class ServiceCollectionExtensions
{
    private static readonly IReadOnlyList<Type> _serviceTypes =
    [
        typeof(IStrategy<>),
        typeof(IStrategy<,>),
        typeof(IAsyncStrategy<>),
        typeof(IAsyncStrategy<,>),
    ];

    public static IServiceCollection AddStrategies(this IServiceCollection services, Action<StrategiesConfigurationBuilder>? configuration = null)
    {
        var builder = new StrategiesConfigurationBuilder(services);
        configuration?.Invoke(builder);
        var cfg = builder.Build();

        services.AddTransient<IStrategyExecutor, StrategyExecutor>();
        services.ScanAndAdd(scanner =>
        {
            scanner.ScanIn(cfg.Assemblies);

            foreach (var serviceType in _serviceTypes)
            {
                scanner.ScanFor(serviceType, type =>
                {
                    type.When(t => t.GetCustomAttribute<StrategyKeyAttribute>() != null);
                    type.WithKey(t => t.GetCustomAttribute<StrategyKeyAttribute>()?.Key);
                    type.AsTransient();
                });
            }
        });

        return services;
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
