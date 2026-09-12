using Microsoft.Extensions.DependencyInjection;

namespace DandyStrategies.Tests;

public class GenericDependencyInjectionTests
{
    private const string _stratAKey = "strat-a";
    private const string _stratBKey = "strat-b";

    private sealed record GenericStrategyDefinition(object Key) : IStrategyDefinition<bool>;

    private sealed class GenericStrategyA : IStrategy<GenericStrategyDefinition, bool>
    {
        public bool Execute(GenericStrategyDefinition definition) => throw new NotImplementedException();
    }

    private sealed class GenericStrategyB : IStrategy<GenericStrategyDefinition, bool>
    {
        public bool Execute(GenericStrategyDefinition definition) => throw new NotImplementedException();
    }

    [Fact]
    public void ServiceCollectionExtensions_AddStrategyDefinition_Generic_AddsStrategies()
    {
        var serviceProvider = new ServiceCollection().AddStrategyDefinition<GenericStrategyDefinition, bool>(def =>
        {
            def.AddStrategy<GenericStrategyA>(_stratAKey);
            def.AddStrategy<GenericStrategyB>(_stratBKey);
        }).BuildServiceProvider();

        // Assert for a
        var a = serviceProvider.GetKeyedService<IStrategy<GenericStrategyDefinition, bool>>(_stratAKey);
        Assert.NotNull(a);
        Assert.Equal(typeof(GenericStrategyA), a.GetType());

        // Assert for b
        var b = serviceProvider.GetKeyedService<IStrategy<GenericStrategyDefinition, bool>>(_stratBKey);
        Assert.NotNull(b);
        Assert.Equal(typeof(GenericStrategyB), b.GetType());
    }
}
