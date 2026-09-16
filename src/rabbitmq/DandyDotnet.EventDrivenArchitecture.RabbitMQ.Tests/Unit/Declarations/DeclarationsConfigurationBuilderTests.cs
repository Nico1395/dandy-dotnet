using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Declarations;

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
        Assert.Equal(["key"], Assert.IsType<string[]>(channel.Queue.RoutingKeys));
    }

    [Fact]
    public void SubscribeChannel_WithSameQueue_ReplacesPreviousChannel()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("first", "queue", _ => { })
            .SubscribeChannel("second", "queue", _ => { })
            .Build();

        Assert.Single(configuration.ChannelsByQueueName);
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

    [Fact]
    public void SubscribeChannel_StoresExchangeDurabilityAndAutoDelete()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel =>
            {
                channel.Exchange.Durable = false;
                channel.Exchange.AutoDelete = true;
            }).Build();
        var exchange = configuration.ChannelsByQueueName["queue"].Exchange;
        Assert.False(exchange.Durable);
        Assert.True(exchange.AutoDelete);
    }

    [Fact]
    public void SubscribeChannel_StoresQueueDurabilityExclusivityAndAutoDelete()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel =>
            {
                channel.Queue.Durable = false;
                channel.Queue.Exclusive = true;
                channel.Queue.AutoDelete = true;
            }).Build();
        var queue = configuration.ChannelsByQueueName["queue"].Queue;
        Assert.False(queue.Durable);
        Assert.True(queue.Exclusive);
        Assert.True(queue.AutoDelete);
    }

    [Fact]
    public void SubscribeChannel_StoresQueueArguments()
    {
        var arguments = new Dictionary<string, object?> { ["x-message-ttl"] = 1000 };
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel => channel.Queue.Arguments = arguments).Build();
        Assert.Same(arguments, configuration.ChannelsByQueueName["queue"].Queue.Arguments);
    }

    [Fact]
    public void SubscribeChannel_StoresPrefetchSettings()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel =>
            {
                channel.PrefetchSize = 12;
                channel.PrefetchCount = 7;
                channel.Global = true;
            }).Build();
        var channel = configuration.ChannelsByQueueName["queue"];
        Assert.Equal((uint)12, channel.PrefetchSize);
        Assert.Equal((ushort)7, channel.PrefetchCount);
        Assert.True(channel.Global);
    }

    [Fact]
    public void SubscribeChannel_StoresConsumerTagAndAutoAck()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel =>
            {
                channel.ConsumerTag = "tag";
                channel.AutoAck = true;
            }).Build();
        var channel = configuration.ChannelsByQueueName["queue"];
        Assert.Equal("tag", channel.ConsumerTag);
        Assert.True(channel.AutoAck);
    }

    [Fact]
    public void SubscribeChannel_WithDifferentQueueNamesCreatesIndependentConfigurations()
    {
        var result = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "one", _ => { })
            .SubscribeChannel("exchange", "two", _ => { }).Build();
        Assert.Equal(2, result.ChannelsByQueueName.Count);
        Assert.NotSame(result.ChannelsByQueueName["one"], result.ChannelsByQueueName["two"]);
        Assert.NotSame(result.ChannelsByQueueName["one"].Exchange, result.ChannelsByQueueName["two"].Exchange);
        Assert.NotSame(result.ChannelsByQueueName["one"].Queue, result.ChannelsByQueueName["two"].Queue);
    }

    [Fact]
    public void SubscribeChannel_WithMultipleRoutingKeys_StoresAllRoutingKeys()
    {
        var result = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel => channel.Queue.RoutingKeys = ["one", "two"])
            .Build();
        Assert.Equal(["one", "two"], Assert.IsType<string[]>(result.ChannelsByQueueName["queue"].Queue.RoutingKeys));
    }
}