using DandyDotnet.Patterns.Strategies.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies;

public sealed class StrategyRegistrar<TDefinition>(IServiceCollection services) : IStrategyRegistrar<TDefinition>
    where TDefinition : IStrategyDefinition
{
    public IStrategyRegistrar<TDefinition> AddStrategy(object key, Type strategyType)
    {
        var interfaceType = typeof(IStrategy<TDefinition>);
        if (!strategyType.IsAssignableTo(interfaceType))
            throw new InvalidOperationException($"Type '{strategyType}' does not implement '{interfaceType}'.");

        services.AddKeyedTransient(interfaceType, key, strategyType);
        return this;
    }

    public IStrategyRegistrar<TDefinition> AddStrategy<TStrategy>(object key)
        where TStrategy : class, IStrategy<TDefinition>
    {
        services.AddKeyedTransient<IStrategy<TDefinition>, TStrategy>(key);
        return this;
    }
}

public sealed class StrategyRegistrar<TDefinition, TReturn>(IServiceCollection services) : IStrategyRegistrar<TDefinition, TReturn>
    where TDefinition : IStrategyDefinition<TReturn>
{
    public IStrategyRegistrar<TDefinition, TReturn> AddStrategy(object key, Type strategyType)
    {
        var interfaceType = typeof(IStrategy<TDefinition, TReturn>);
        if (!strategyType.IsAssignableTo(interfaceType))
            throw new InvalidOperationException($"Type '{strategyType}' does not implement '{interfaceType}'.");

        services.AddKeyedTransient(interfaceType, key, strategyType);
        return this;
    }

    public IStrategyRegistrar<TDefinition, TReturn> AddStrategy<TStrategy>(object key)
        where TStrategy : class, IStrategy<TDefinition, TReturn>
    {
        services.AddKeyedTransient<IStrategy<TDefinition, TReturn>, TStrategy>(key);
        return this;
    }
}