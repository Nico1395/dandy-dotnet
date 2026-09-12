namespace DandyStrategies;

public interface IStrategy<in TDefinition>
    where TDefinition : IStrategyDefinition
{
    void Execute(TDefinition definition);
}

public interface IStrategy<in TDefinition, out TReturn>
    where TDefinition : IStrategyDefinition<TReturn>
{
    TReturn Execute(TDefinition definition);
}