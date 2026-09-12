using Microsoft.Extensions.DependencyInjection;

namespace DandyStrategies.Tests.Setups;

public class StrategyMediatorSetup
{
    public StrategyMediatorSetup()
    {
        var services = new ServiceCollection();

        services.AddDandyStrategies(cfg => cfg.ScanInAssemblies(GetType().Assembly));

        ServiceProvider = services.BuildServiceProvider();
    }

    public IServiceProvider ServiceProvider { get; }

    public TService GetRequiredService<TService>()
        where TService : class
    {
        return ServiceProvider.GetRequiredService<TService>();
    }

    public IStrategyExecutor GetStrategyExecutor()
    {
        return GetRequiredService<IStrategyExecutor>();
    }
}
