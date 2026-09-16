using System.Reflection;
using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.Patterns.Strategies.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies;

/// <summary>
///     Extension methods for <see cref="IServiceCollection" /> to register strategy pattern services and implementations.
/// </summary>
public static class ServiceCollectionExtensions
{
    private static readonly IReadOnlyList<Type> _serviceTypes =
    [
        typeof(IStrategy<>),
        typeof(IStrategy<,>),
        typeof(IAsyncStrategy<>),
        typeof(IAsyncStrategy<,>),
    ];

    /// <summary>
    ///     Adds strategy pattern services and configures strategy discovery and registration.
    /// </summary>
    /// <param name="services">The service collection to add strategy services to.</param>
    /// <param name="configuration">An optional action to configure strategies using <see cref="StrategiesConfigurationBuilder" />.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
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

    /// <summary>
    ///     Registers synchronous strategies for the specified strategy definition type.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the strategy definition.</typeparam>
    /// <param name="services">The service collection to add strategy registrations to.</param>
    /// <param name="definition">An action to configure and register strategies using <see cref="IStrategyRegistrar{TDefinition}" />.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddStrategyDefinition<TDefinition>(this IServiceCollection services, Action<IStrategyRegistrar<TDefinition>> definition)
        where TDefinition : IStrategyDefinition
    {
        definition(new StrategyRegistrar<TDefinition>(services));
        return services;
    }

    /// <summary>
    ///     Registers synchronous strategies that return a result for the specified strategy definition type.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the strategy definition.</typeparam>
    /// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
    /// <param name="services">The service collection to add strategy registrations to.</param>
    /// <param name="definition">An action to configure and register strategies using <see cref="IStrategyRegistrar{TDefinition, TReturn}" />.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddStrategyDefinition<TDefinition, TReturn>(this IServiceCollection services, Action<IStrategyRegistrar<TDefinition, TReturn>> definition)
        where TDefinition : IStrategyDefinition<TReturn>
    {
        definition(new StrategyRegistrar<TDefinition, TReturn>(services));
        return services;
    }

    /// <summary>
    ///     Registers asynchronous strategies for the specified asynchronous strategy definition type.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the asynchronous strategy definition.</typeparam>
    /// <param name="services">The service collection to add strategy registrations to.</param>
    /// <param name="definition">An action to configure and register strategies using <see cref="IAsyncStrategyRegistrar{TDefinition}" />.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddStrategyDefinition<TDefinition>(this IServiceCollection services, Action<IAsyncStrategyRegistrar<TDefinition>> definition)
        where TDefinition : IAsyncStrategyDefinition
    {
        definition(new AsyncStrategyRegistrar<TDefinition>(services));
        return services;
    }

    /// <summary>
    ///     Registers asynchronous strategies that return a result for the specified asynchronous strategy definition type.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the asynchronous strategy definition.</typeparam>
    /// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
    /// <param name="services">The service collection to add strategy registrations to.</param>
    /// <param name="definition">An action to configure and register strategies using <see cref="IAsyncStrategyRegistrar{TDefinition, TReturn}" />.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddStrategyDefinition<TDefinition, TReturn>(this IServiceCollection services, Action<IAsyncStrategyRegistrar<TDefinition, TReturn>> definition)
        where TDefinition : IAsyncStrategyDefinition<TReturn>
    {
        definition(new AsyncStrategyRegistrar<TDefinition, TReturn>(services));
        return services;
    }
}
