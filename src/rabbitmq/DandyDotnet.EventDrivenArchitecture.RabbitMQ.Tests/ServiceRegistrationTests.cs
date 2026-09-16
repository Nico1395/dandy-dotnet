using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class ServiceRegistrationTests
{
    [Fact]
    public void AddRabbitMQConsumer_RegistersConsumerServices()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(_ => { });
        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetRequiredService<IConsumerPipeline>());
        Assert.NotNull(provider.GetRequiredService<IReceiver>());
        Assert.NotNull(provider.GetRequiredService<ConsumerConfiguration>());
    }

    [Fact]
    public void AddRabbitMQProducer_RegistersProducerConfiguration()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQProducer(_ => { });
        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetRequiredService<ProducerConfiguration>());
        Assert.NotNull(provider.GetRequiredService<IProducer>());
    }

    [Fact]
    public void AddRabbitMQProducer_RegistersProducerAsScoped()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQProducer(_ => { });
        using var provider = services.BuildServiceProvider();
        using var first = provider.CreateScope();
        using var second = provider.CreateScope();

        Assert.NotSame(first.ServiceProvider.GetRequiredService<IProducer>(), second.ServiceProvider.GetRequiredService<IProducer>());
    }
}
