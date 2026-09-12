using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Tests.Core.Fixtures;

public abstract class Fixture : IFixture
{
    protected ServiceProvider ServiceProvider { get; }

    protected Fixture()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public object? GetService(Type serviceType)
    {
        return ServiceProvider.GetService(serviceType);
    }

    protected abstract void ConfigureServices(IServiceCollection services);
}