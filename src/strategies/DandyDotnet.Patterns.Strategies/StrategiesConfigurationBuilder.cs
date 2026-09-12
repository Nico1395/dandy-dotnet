using System.Reflection;
using DandyDotnet.Patterns.Strategies.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies;

public sealed class StrategiesConfigurationBuilder(IServiceCollection services)
{
    private readonly StrategiesConfiguration _configuration = new();

    public StrategiesConfigurationBuilder ScanInAssemblies(params IEnumerable<Assembly> assemblies)
    {
        _configuration.Assemblies = [.. assemblies];
        return this;
    }

    public StrategiesConfigurationBuilder AddStrategyDefinition<TDefinition>(Action<IStrategyRegistrar<TDefinition>> definition)
        where TDefinition : IStrategyDefinition
    {
        definition(new StrategyRegistrar<TDefinition>(services));
        return this;
    }

    public StrategiesConfigurationBuilder AddStrategyDefinition<TDefinition, TReturn>(Action<IStrategyRegistrar<TDefinition, TReturn>> definition)
        where TDefinition : IStrategyDefinition<TReturn>
    {
        definition(new StrategyRegistrar<TDefinition, TReturn>(services));
        return this;
    }

    public StrategiesConfigurationBuilder AddStrategyDefinition<TDefinition>(Action<IAsyncStrategyRegistrar<TDefinition>> definition)
        where TDefinition : IAsyncStrategyDefinition
    {
        definition(new AsyncStrategyRegistrar<TDefinition>(services));
        return this;
    }

    public StrategiesConfigurationBuilder AddStrategyDefinition<TDefinition, TReturn>(Action<IAsyncStrategyRegistrar<TDefinition, TReturn>> definition)
        where TDefinition : IAsyncStrategyDefinition<TReturn>
    {
        definition(new AsyncStrategyRegistrar<TDefinition, TReturn>(services));
        return this;
    }

    internal StrategiesConfiguration Build() => _configuration;
}
