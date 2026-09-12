namespace DandyStrategies;

public interface IAsyncStrategyDefinition
{
    object Key { get; }
}

public interface IAsyncStrategyDefinition<TReturn>
{
    object Key { get; }
}
