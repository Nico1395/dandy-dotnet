using Microsoft.Extensions.DependencyInjection;

namespace DandyStrategies.Tests;

public class StrategyKeyAttributeDependencyInjectionTests
{
    private const string _stratAKey = "strat-a";
    private const string _stratBKey = "strat-b";

    private sealed record AssemblyScanningStrategyDefinition(object Key) : IStrategyDefinition;

    [StrategyKey(_stratAKey)]
    private sealed class AssemblyScanningStrategyA : IStrategy<AssemblyScanningStrategyDefinition>
    {
        public void Execute(AssemblyScanningStrategyDefinition definition) => throw new NotImplementedException();
    }

    [StrategyKey(_stratBKey)]
    private sealed class AssemblyScanningStrategyB : IStrategy<AssemblyScanningStrategyDefinition>
    {
        public void Execute(AssemblyScanningStrategyDefinition definition) => throw new NotImplementedException();
    }

    [Fact]
    public void ServiceCollectionExtensions_AddStrategiesByKeyAttributeFromAssemblies_CorrectlyAddsStrategies()
    {
        var assembly = GetType().Assembly;
        var serviceProvider = new ServiceCollection().AddDandyStrategies(cfg => cfg.ScanInAssemblies(assembly)).BuildServiceProvider();

        // Assert for a
        var a = serviceProvider.GetKeyedService<IStrategy<AssemblyScanningStrategyDefinition>>(_stratAKey);
        Assert.NotNull(a);
        Assert.Equal(typeof(AssemblyScanningStrategyA), a.GetType());

        // Assert for b
        var b = serviceProvider.GetKeyedService<IStrategy<AssemblyScanningStrategyDefinition>>(_stratBKey);
        Assert.NotNull(b);
        Assert.Equal(typeof(AssemblyScanningStrategyB), b.GetType());
    }
}
