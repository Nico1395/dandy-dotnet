using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class DeclarationsConfigurationBuilderTests
{
    [Fact]
    public void SubscribeChannel_StoresChannelByQueueName()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel => channel.Queue.RoutingKeys = ["key"])
            .Build();

        var channel = configuration.ChannelsByQueueName["queue"];
        Assert.Equal("exchange", channel.Exchange.Name);
        Assert.Equal("queue", channel.Queue.Name);
        Assert.Equal(["key"], channel.Queue.RoutingKeys);
    }

    [Fact]
    public void SubscribeChannel_WithSameQueue_ReplacesPreviousChannel()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("first", "queue", _ => { })
            .SubscribeChannel("second", "queue", _ => { })
            .Build();

        Assert.Equal("second", configuration.ChannelsByQueueName["queue"].Exchange.Name);
    }

    [Fact]
    public void ChannelConfiguration_HasExpectedDefaults()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", _ => { })
            .Build();

        var channel = configuration.ChannelsByQueueName["queue"];
        Assert.Equal(32, channel.PrefetchCount);
        Assert.Equal("topic", channel.Exchange.ExchangeType);
        Assert.Equal("quorum", channel.Queue.Arguments["x-queue-type"]);
        Assert.False(channel.AutoAck);
    }
}
