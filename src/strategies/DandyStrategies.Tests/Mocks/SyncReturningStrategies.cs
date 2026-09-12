namespace DandyStrategies.Tests.Mocks;

internal static class SyncReturningStrategies
{
    public sealed record Definition(object Key) : IStrategyDefinition<object>;

    [StrategyKey("strat-a")]
    public sealed class StrategyA : IStrategy<Definition, object>
    {
        public object Execute(Definition definition)
        {
            return definition.Key;
        }
    }

    [StrategyKey("strat-b")]
    public sealed class StrategyB : IStrategy<Definition, object>
    {
        public object Execute(Definition definition)
        {
            return definition.Key;
        }
    }
}
