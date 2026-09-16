using System.Reflection;
using DandyDotnet.Patterns.Strategies.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies;

/// <summary>
///     Provides a builder for configuring strategies and registering strategy implementations.
/// </summary>
/// <param name="services">The service collection to add strategy registrations to.</param>
public sealed class StrategiesConfigurationBuilder(IServiceCollection services)
{
    private readonly StrategiesConfiguration _configuration = new();

    /// <summary>
    ///     Specifies the assemblies to scan for strategy definitions and implementations.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public StrategiesConfigurationBuilder ScanInAssemblies(params IEnumerable<Assembly> assemblies)
    {
        _configuration.Assemblies = [.. assemblies];
        return this;
    }

    /// <summary>
    ///     Registers synchronous strategies for the specified strategy definition type.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the strategy definition.</typeparam>
    /// <param name="definition">An action to configure and register strategies using <see cref="IStrategyRegistrar{TDefinition}" />.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public StrategiesConfigurationBuilder AddStrategyDefinition<TDefinition>(Action<IStrategyRegistrar<TDefinition>> definition)
        where TDefinition : IStrategyDefinition
    {
        definition(new StrategyRegistrar<TDefinition>(services));
        return this;
    }

    /// <summary>
    ///     Registers synchronous strategies that return a result for the specified strategy definition type.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the strategy definition.</typeparam>
    /// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
    /// <param name="definition">An action to configure and register strategies using <see cref="IStrategyRegistrar{TDefinition, TReturn}" />.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public StrategiesConfigurationBuilder AddStrategyDefinition<TDefinition, TReturn>(Action<IStrategyRegistrar<TDefinition, TReturn>> definition)
        where TDefinition : IStrategyDefinition<TReturn>
    {
        definition(new StrategyRegistrar<TDefinition, TReturn>(services));
        return this;
    }

    /// <summary>
    ///     Registers asynchronous strategies for the specified asynchronous strategy definition type.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the asynchronous strategy definition.</typeparam>
    /// <param name="definition">An action to configure and register strategies using <see cref="IAsyncStrategyRegistrar{TDefinition}" />.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public StrategiesConfigurationBuilder AddStrategyDefinition<TDefinition>(Action<IAsyncStrategyRegistrar<TDefinition>> definition)
        where TDefinition : IAsyncStrategyDefinition
    {
        definition(new AsyncStrategyRegistrar<TDefinition>(services));
        return this;
    }

    /// <summary>
    ///     Registers asynchronous strategies that return a result for the specified asynchronous strategy definition type.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the asynchronous strategy definition.</typeparam>
    /// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
    /// <param name="definition">An action to configure and register strategies using <see cref="IAsyncStrategyRegistrar{TDefinition, TReturn}" />.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public StrategiesConfigurationBuilder AddStrategyDefinition<TDefinition, TReturn>(Action<IAsyncStrategyRegistrar<TDefinition, TReturn>> definition)
        where TDefinition : IAsyncStrategyDefinition<TReturn>
    {
        definition(new AsyncStrategyRegistrar<TDefinition, TReturn>(services));
        return this;
    }

    internal StrategiesConfiguration Build() => _configuration;
}
