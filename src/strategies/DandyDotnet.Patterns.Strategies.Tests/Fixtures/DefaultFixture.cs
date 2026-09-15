using DandyDotnet.Patterns.Strategies.Abstractions;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddStrategies(cfg => cfg.ScanInAssemblies(GetType().Assembly));
    }

    public IStrategyExecutor GetStrategyExecutor()
    {
        return ServiceProvider.GetRequiredService<IStrategyExecutor>();
    }
}