namespace DandyStrategies;

public interface IStrategyDefinition
{
    object Key { get; }
}

public interface IStrategyDefinition<TReturn>
{
    object Key { get; }
}