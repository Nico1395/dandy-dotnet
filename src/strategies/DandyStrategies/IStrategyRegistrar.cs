namespace DandyStrategies;

public interface IStrategyRegistrar<out TDefinition>
    where TDefinition : IStrategyDefinition
{
    IStrategyRegistrar<TDefinition> AddStrategy(object key, Type strategyType);
    IStrategyRegistrar<TDefinition> AddStrategy<TStrategy>(object key) where TStrategy : class, IStrategy<TDefinition>;
}

public interface IStrategyRegistrar<out TDefinition, in TReturn>
    where TDefinition : IStrategyDefinition<TReturn>
{
    IStrategyRegistrar<TDefinition, TReturn> AddStrategy(object key, Type strategyType);
    IStrategyRegistrar<TDefinition, TReturn> AddStrategy<TStrategy>(object key) where TStrategy : class, IStrategy<TDefinition, TReturn>;
}