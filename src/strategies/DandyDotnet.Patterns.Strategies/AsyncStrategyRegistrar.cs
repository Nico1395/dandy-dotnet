using DandyDotnet.Patterns.Strategies.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies;

/// <summary>
///     Provides the standard implementation of <see cref="IAsyncStrategyRegistrar{TDefinition}" /> for registering asynchronous strategies.
/// </summary>
/// <typeparam name="TDefinition">The type of the asynchronous strategy definition.</typeparam>
/// <param name="services">The service collection to add registrations to.</param>
public sealed class AsyncStrategyRegistrar<TDefinition>(IServiceCollection services) : IAsyncStrategyRegistrar<TDefinition>
    where TDefinition : IAsyncStrategyDefinition
{
    /// <inheritdoc />
    public IAsyncStrategyRegistrar<TDefinition> AddStrategy(object key, Type strategyType)
    {
        var interfaceType = typeof(IAsyncStrategy<TDefinition>);
        if (!strategyType.IsAssignableTo(interfaceType))
            throw new InvalidOperationException($"Type '{strategyType}' does not implement '{interfaceType}'.");

        services.AddKeyedTransient(interfaceType, key, strategyType);
        return this;
    }

    /// <inheritdoc />
    public IAsyncStrategyRegistrar<TDefinition> AddStrategy<TStrategy>(object key)
        where TStrategy : class, IAsyncStrategy<TDefinition>
    {
        services.AddKeyedTransient<IAsyncStrategy<TDefinition>, TStrategy>(key);
        return this;
    }
}

/// <summary>
///     Provides the standard implementation of <see cref="IAsyncStrategyRegistrar{TDefinition, TReturn}" /> for registering asynchronous strategies that return a result.
/// </summary>
/// <typeparam name="TDefinition">The type of the asynchronous strategy definition.</typeparam>
/// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
/// <param name="services">The service collection to add registrations to.</param>
public sealed class AsyncStrategyRegistrar<TDefinition, TReturn>(IServiceCollection services) : IAsyncStrategyRegistrar<TDefinition, TReturn>
    where TDefinition : IAsyncStrategyDefinition<TReturn>
{
    /// <inheritdoc />
    public IAsyncStrategyRegistrar<TDefinition, TReturn> AddStrategy(object key, Type strategyType)
    {
        var interfaceType = typeof(IAsyncStrategy<TDefinition, TReturn>);
        if (!strategyType.IsAssignableTo(interfaceType))
            throw new InvalidOperationException($"Type '{strategyType}' does not implement '{interfaceType}'.");

        services.AddKeyedTransient(interfaceType, key, strategyType);
        return this;
    }

    /// <inheritdoc />
    public IAsyncStrategyRegistrar<TDefinition, TReturn> AddStrategy<TStrategy>(object key)
        where TStrategy : class, IAsyncStrategy<TDefinition, TReturn>
    {
        services.AddKeyedTransient<IAsyncStrategy<TDefinition, TReturn>, TStrategy>(key);
        return this;
    }
}
