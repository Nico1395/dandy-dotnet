using DandyDotnet.Patterns.Strategies.Tests.Fixtures;
using DandyDotnet.Patterns.Strategies.Tests.Mocks;

namespace DandyDotnet.Patterns.Strategies.Tests;

public class StrategyExecutorTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void Execute_Passes()
    {
        var helper = new StrategyAssertHelper();
        var executor = fixture.GetStrategyExecutor();
        var def = new SyncStrategies.Definition("strat-b", helper);

        executor.Execute(def);

        Assert.NotNull(helper.ExecutedStrategy);
        Assert.Equal("strat-b", helper.ExecutedStrategy);
    }

    [Fact]
    public void ExecuteWithReturning_Passes()
    {
        var executor = fixture.GetStrategyExecutor();
        var def = new SyncReturningStrategies.Definition("strat-b");

        Assert.Equal("strat-b", executor.Execute(def));
    }

    [Fact]
    public async Task ExecuteAsync_Passes()
    {
        var helper = new StrategyAssertHelper();
        var executor = fixture.GetStrategyExecutor();
        var def = new AsyncStrategies.Definition("strat-a", helper);

        await executor.ExecuteAsync(def, cancellationToken: CancellationToken.None);

        Assert.NotNull(helper.ExecutedStrategy);
        Assert.Equal("strat-a", helper.ExecutedStrategy);
    }

    [Fact]
    public async Task ExecuteAsyncWithReturning_Passes()
    {
        var executor = fixture.GetStrategyExecutor();
        var def = new AsyncReturningStrategies.Definition("strat-a");

        var resultingKey = await executor.ExecuteAsync(def, cancellationToken: CancellationToken.None);

        Assert.Equal("strat-a", resultingKey);
    }
}
