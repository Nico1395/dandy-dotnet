using Microsoft.Extensions.DependencyInjection;

namespace DandyStrategies.Tests;

public class NonGenericDependencyInjectionTests
{
    private const string _stratAKey = "strat-a";
    private const string _stratBKey = "strat-b";

    private sealed record NonGenericStrategyDefinition(object Key) : IStrategyDefinition;

    private sealed class NonGenericStrategyA : IStrategy<NonGenericStrategyDefinition>
    {
        public void Execute(NonGenericStrategyDefinition definition) => throw new NotImplementedException();
    }

    private sealed class NonGenericStrategyB : IStrategy<NonGenericStrategyDefinition>
    {
        public void Execute(NonGenericStrategyDefinition definition) => throw new NotImplementedException();
    }

    [Fact]
    public void ServiceCollectionExtensions_AddStrategyDefinition_NonGeneric_AddsStrategies()
    {
        var serviceProvider = new ServiceCollection().AddStrategyDefinition<NonGenericStrategyDefinition>(def =>
        {
            def.AddStrategy<NonGenericStrategyA>(_stratAKey);
            def.AddStrategy<NonGenericStrategyB>(_stratBKey);
        }).BuildServiceProvider();

        // Assert for a
        var a = serviceProvider.GetKeyedService<IStrategy<NonGenericStrategyDefinition>>(_stratAKey);
        Assert.NotNull(a);
        Assert.Equal(typeof(NonGenericStrategyA), a.GetType());

        // Assert for b
        var b = serviceProvider.GetKeyedService<IStrategy<NonGenericStrategyDefinition>>(_stratBKey);
        Assert.NotNull(b);
        Assert.Equal(typeof(NonGenericStrategyB), b.GetType());
    }
}
