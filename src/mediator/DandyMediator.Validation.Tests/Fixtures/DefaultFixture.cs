using Microsoft.Extensions.DependencyInjection;

namespace DandyMediator.Validation.Tests.Fixtures;

public sealed class DefaultFixture : IServiceProvider
{
    private readonly IServiceProvider _serviceProvider;
    
    public DefaultFixture()
    {
        var services = new ServiceCollection();

        services.AddDandyMediator(config => config
            .ScanInAssemblies(typeof(DefaultFixture).Assembly)
            .UseValidation());

        _serviceProvider = services.BuildServiceProvider();
    }

    public object? GetService(Type serviceType)
    {
        return _serviceProvider.GetService(serviceType);
    }
    
    public IMediator GetMediator()
    {
        return _serviceProvider.GetRequiredService<IMediator>();
    }
}