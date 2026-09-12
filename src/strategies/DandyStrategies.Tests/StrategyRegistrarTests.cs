using Microsoft.Extensions.DependencyInjection;

namespace DandyStrategies.Tests;

public class StrategyRegistrarTests
{
    private sealed record TestDefinition(object Key) : IStrategyDefinition;
    private sealed class TestDefinitionStrategy : IStrategy<TestDefinition>
    {
        public void Execute(TestDefinition definition) => throw new NotImplementedException();
    }

    private sealed record RetTestDefinition(object Key) : IStrategyDefinition<int>;
    private sealed class RetTestDefinitionStrategy : IStrategy<RetTestDefinition, int>
    {
        public int Execute(RetTestDefinition definition) => throw new NotImplementedException();
    }

    [Fact]
    public void AddStrategy_Sync_ValidType_Passes()
    {
        var registrar = new StrategyRegistrar<TestDefinition>(new ServiceCollection());
        registrar.AddStrategy("valid-type", typeof(TestDefinitionStrategy));
    }

    [Fact]
    public void AddStrategy_Sync_InvalidType_Throws()
    {
        var registrar = new StrategyRegistrar<TestDefinition>(new ServiceCollection());
        Assert.Throws<InvalidOperationException>(() => registrar.AddStrategy("invalid-type", typeof(string)));
    }

    [Fact]
    public void AddStrategyRet_Sync_ValidType_Passes()
    {
        var registrar = new StrategyRegistrar<RetTestDefinition, int>(new ServiceCollection());
        registrar.AddStrategy("valid-type", typeof(RetTestDefinitionStrategy));
    }

    [Fact]
    public void AddStrategyRet_Sync_InvalidType_Throws()
    {
        var registrar = new StrategyRegistrar<RetTestDefinition, int>(new ServiceCollection());
        Assert.Throws<InvalidOperationException>(() => registrar.AddStrategy("invalid-type", typeof(string)));
    }
}
