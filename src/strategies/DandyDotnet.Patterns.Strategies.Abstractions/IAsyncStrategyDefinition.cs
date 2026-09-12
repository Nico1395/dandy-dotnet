namespace DandyDotnet.Patterns.Strategies.Abstractions;

public interface IAsyncStrategyDefinition
{
    object Key { get; }
}

public interface IAsyncStrategyDefinition<TReturn>
{
    object Key { get; }
}
