using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Consumer;

public sealed class ConsumerContextTests
{
    [Fact]
    public void Constructor_ExposesDeliveryArgumentsAndChannelConfiguration()
    {
        var args = new BasicDeliverEventArgs("consumer", 7, false, "exchange", "routing", new BasicProperties(), ReadOnlyMemory<byte>.Empty);
        var configuration = new ChannelConfiguration("exchange", "queue");

        var context = new ConsumerContext(args, configuration);

        Assert.Same(args, context.DeliverArgs);
        Assert.Same(configuration, context.ChannelConfiguration);
    }
}