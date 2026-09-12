namespace DandyDotnet.Patterns.Strategies;

public interface IAsyncStrategyDefinition
{
    object Key { get; }
}

public interface IAsyncStrategyDefinition<TReturn>
{
    object Key { get; }
}
