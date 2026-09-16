using DandyDotnet.Patterns.Strategies.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies;

/// <summary>
///     Provides the standard implementation of <see cref="IStrategyRegistrar{TDefinition}" /> for registering synchronous strategies.
/// </summary>
/// <typeparam name="TDefinition">The type of the strategy definition.</typeparam>
/// <param name="services">The service collection to add registrations to.</param>
public sealed class StrategyRegistrar<TDefinition>(IServiceCollection services) : IStrategyRegistrar<TDefinition>
    where TDefinition : IStrategyDefinition
{
    /// <inheritdoc />
    public IStrategyRegistrar<TDefinition> AddStrategy(object key, Type strategyType)
    {
        var interfaceType = typeof(IStrategy<TDefinition>);
        if (!strategyType.IsAssignableTo(interfaceType))
            throw new InvalidOperationException($"Type '{strategyType}' does not implement '{interfaceType}'.");

        services.AddKeyedTransient(interfaceType, key, strategyType);
        return this;
    }

    /// <inheritdoc />
    public IStrategyRegistrar<TDefinition> AddStrategy<TStrategy>(object key)
        where TStrategy : class, IStrategy<TDefinition>
    {
        services.AddKeyedTransient<IStrategy<TDefinition>, TStrategy>(key);
        return this;
    }
}

/// <summary>
///     Provides the standard implementation of <see cref="IStrategyRegistrar{TDefinition, TReturn}" /> for registering synchronous strategies that return a result.
/// </summary>
/// <typeparam name="TDefinition">The type of the strategy definition.</typeparam>
/// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
/// <param name="services">The service collection to add registrations to.</param>
public sealed class StrategyRegistrar<TDefinition, TReturn>(IServiceCollection services) : IStrategyRegistrar<TDefinition, TReturn>
    where TDefinition : IStrategyDefinition<TReturn>
{
    /// <inheritdoc />
    public IStrategyRegistrar<TDefinition, TReturn> AddStrategy(object key, Type strategyType)
    {
        var interfaceType = typeof(IStrategy<TDefinition, TReturn>);
        if (!strategyType.IsAssignableTo(interfaceType))
            throw new InvalidOperationException($"Type '{strategyType}' does not implement '{interfaceType}'.");

        services.AddKeyedTransient(interfaceType, key, strategyType);
        return this;
    }

    /// <inheritdoc />
    public IStrategyRegistrar<TDefinition, TReturn> AddStrategy<TStrategy>(object key)
        where TStrategy : class, IStrategy<TDefinition, TReturn>
    {
        services.AddKeyedTransient<IStrategy<TDefinition, TReturn>, TStrategy>(key);
        return this;
    }
}