using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.DependencyInjection;

public sealed class ConsumerServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRabbitMQConsumer_RegistersConsumerServices()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(_ => { });
        using var provider = services.BuildServiceProvider();

        Assert.IsType<ConsumerPipeline>(provider.GetRequiredService<IConsumerPipeline>());
        Assert.IsType<Receiver>(provider.GetRequiredService<IReceiver>());
        var configuration = provider.GetRequiredService<ConsumerConfiguration>();
        Assert.Same(configuration, provider.GetRequiredService<ConsumerConfiguration>());
    }

    [Fact]
    public void AddRabbitMQConsumer_WhenCalledTwice_AddsTwoPipelineRegistrations()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(_ => { });
        services.AddRabbitMQConsumer(_ => { });
        Assert.Equal(2, services.Count(descriptor => descriptor.ServiceType == typeof(IConsumerPipeline)));
    }
}