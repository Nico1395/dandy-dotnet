namespace DandyStrategies.Tests.Mocks;

internal static class SyncStrategies
{
    public sealed record Definition(object Key, StrategyAssertHelper Helper) : IStrategyDefinition;

    [StrategyKey("strat-a")]
    public sealed class StrategyA : IStrategy<Definition>
    {
        public void Execute(Definition definition)
        {
            definition.Helper.ExecutedStrategy = definition.Key;
        }
    }
    
    [StrategyKey("strat-b")]
    public sealed class StrategyB : IStrategy<Definition>
    {
        public void Execute(Definition definition)
        {
            definition.Helper.ExecutedStrategy = definition.Key;
        }
    }
}
