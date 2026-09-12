namespace DandyStrategies.Tests.Mocks;

internal static class AsyncReturningStrategies
{
    public sealed record Definition(object Key) : IAsyncStrategyDefinition<object>;

    [StrategyKey("strat-a")]
    public sealed class StrategyA : IAsyncStrategy<Definition, object>
    {
        public Task<object> ExecuteAsync(Definition definition, CancellationToken cancellationToken)
        {
            return Task.FromResult(definition.Key);
        }
    }

    [StrategyKey("strat-b")]
    public sealed class StrategyB : IAsyncStrategy<Definition, object>
    {
        public Task<object> ExecuteAsync(Definition definition, CancellationToken cancellationToken)
        {
            return Task.FromResult(definition.Key);
        }
    }
}
