using DandyDotnet.Patterns.Strategies.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies;

public sealed class AsyncStrategyRegistrar<TDefinition>(IServiceCollection services) : IAsyncStrategyRegistrar<TDefinition>
    where TDefinition : IAsyncStrategyDefinition
{
    public IAsyncStrategyRegistrar<TDefinition> AddStrategy(object key, Type strategyType)
    {
        var interfaceType = typeof(IAsyncStrategy<TDefinition>);
        if (!strategyType.IsAssignableTo(interfaceType))
            throw new InvalidOperationException($"Type '{strategyType}' does not implement '{interfaceType}'.");

        services.AddKeyedTransient(interfaceType, key, strategyType);
        return this;
    }

    public IAsyncStrategyRegistrar<TDefinition> AddStrategy<TStrategy>(object key)
        where TStrategy : class, IAsyncStrategy<TDefinition>
    {
        services.AddKeyedTransient<IAsyncStrategy<TDefinition>, TStrategy>(key);
        return this;
    }
}

public sealed class AsyncStrategyRegistrar<TDefinition, TReturn>(IServiceCollection services) : IAsyncStrategyRegistrar<TDefinition, TReturn>
    where TDefinition : IAsyncStrategyDefinition<TReturn>
{
    public IAsyncStrategyRegistrar<TDefinition, TReturn> AddStrategy(object key, Type strategyType)
    {
        var interfaceType = typeof(IAsyncStrategy<TDefinition, TReturn>);
        if (!strategyType.IsAssignableTo(interfaceType))
            throw new InvalidOperationException($"Type '{strategyType}' does not implement '{interfaceType}'.");

        services.AddKeyedTransient(interfaceType, key, strategyType);
        return this;
    }

    public IAsyncStrategyRegistrar<TDefinition, TReturn> AddStrategy<TStrategy>(object key)
        where TStrategy : class, IAsyncStrategy<TDefinition, TReturn>
    {
        services.AddKeyedTransient<IAsyncStrategy<TDefinition, TReturn>, TStrategy>(key);
        return this;
    }
}
