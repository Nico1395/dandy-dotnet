using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Tests.Core.Fixtures;

public abstract class Fixture : IFixture
{
    protected ServiceProvider ServiceProvider { get; }

    protected Fixture()
    {
        try
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public Task InitializeAsync()
    {
        try
        {
            return OnInitializeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public virtual async Task DisposeAsync()
    {
        await ServiceProvider.DisposeAsync();
    }

    public object? GetService(Type serviceType)
    {
        return ServiceProvider.GetService(serviceType);
    }

    protected virtual Task OnInitializeAsync()
    {
        return Task.CompletedTask;
    }

    protected abstract void ConfigureServices(IServiceCollection services);
}