using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.DependencyInjection;

public sealed class ConnectivityServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRabbitMQConnectivity_WhenCalledTwice_RegistersOneConnectionProvider()
    {
        var services = new ServiceCollection();
        var config = new ConnectivityConfigurationBuilder().Build();
        services.AddRabbitMQConnectivity(config).AddRabbitMQConnectivity(config);
        Assert.Single(services, descriptor => descriptor.ServiceType == typeof(IConnectionProvider));
    }
}