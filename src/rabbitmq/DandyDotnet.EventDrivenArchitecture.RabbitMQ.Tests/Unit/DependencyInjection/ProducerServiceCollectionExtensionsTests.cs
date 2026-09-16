using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.DependencyInjection;

public sealed class ProducerServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRabbitMQProducer_RegistersSingletonConfigurationAndResolvableProducer()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQProducer(_ => { });
        using var provider = services.BuildServiceProvider();

        var configuration = provider.GetRequiredService<ProducerConfiguration>();
        Assert.Same(configuration, provider.GetRequiredService<ProducerConfiguration>());
        using var scope = provider.CreateScope();
        Assert.IsAssignableFrom<IProducer>(scope.ServiceProvider.GetRequiredService<IProducer>());
    }

    [Fact]
    public void AddRabbitMQProducer_RegistersProducerAsScoped()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQProducer(_ => { });
        using var provider = services.BuildServiceProvider();
        using var first = provider.CreateScope();
        using var second = provider.CreateScope();

        var producer = first.ServiceProvider.GetRequiredService<IProducer>();
        Assert.Same(producer, first.ServiceProvider.GetRequiredService<IProducer>());
        Assert.NotSame(producer, second.ServiceProvider.GetRequiredService<IProducer>());
    }

    [Fact]
    public void AddRabbitMQProducer_WhenCalledTwice_AddsTwoConfigurationRegistrations()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQProducer(_ => { });
        services.AddRabbitMQProducer(_ => { });
        Assert.Equal(2, services.Count(x => x.ServiceType == typeof(ProducerConfiguration)));
    }
}