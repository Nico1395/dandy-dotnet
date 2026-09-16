using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Declarations;

public sealed class DeclarerTests
{
    [Fact]
    public async Task DeclareQueueAsync_WithUnknownQueue_ThrowsArgumentExceptionBeforeUsingChannel()
    {
        var (channel, recording) = RecordingChannel.Create();
        var declarer = new Declarer(new UnexpectedConnectionProvider(), new DeclarationsConfigurationBuilder().Build());

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            declarer.DeclareQueueAsync("unknown", channel, CancellationToken.None));

        Assert.Equal("queueName", exception.ParamName);
        Assert.Empty(recording.Calls);
    }

    [Fact]
    public async Task DeclareQueueAsync_WithNullRoutingKeys_ThrowsWithoutBindingQueue()
    {
        var (channel, recording) = RecordingChannel.Create();
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", value => value.Queue.RoutingKeys = null).Build();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new Declarer(new UnexpectedConnectionProvider(), configuration)
                .DeclareQueueAsync("queue", channel, CancellationToken.None));

        Assert.Equal("Routing keys are not set.", exception.Message);
        Assert.DoesNotContain(recording.Calls, call => call.Method == nameof(IChannel.QueueBindAsync));
    }

    [Fact]
    public async Task DeclareQueueAsync_WithSuppliedChannel_ForwardsTopologyQosBindingsAndCancellation()
    {
        var (channel, recording) = RecordingChannel.Create();
        using var cancellation = new CancellationTokenSource();
        var arguments = new Dictionary<string, object?> { ["x-message-ttl"] = 1000 };
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", value =>
            {
                value.Exchange.ExchangeType = ExchangeType.Direct;
                value.Exchange.Durable = false;
                value.Exchange.AutoDelete = true;
                value.Queue.Durable = false;
                value.Queue.Exclusive = true;
                value.Queue.AutoDelete = true;
                value.Queue.NoWait = true;
                value.Queue.Arguments = arguments;
                value.Queue.RoutingKeys = ["one", "two"];
                value.PrefetchCount = 7;
                value.PrefetchSize = 12;
                value.Global = true;
            }).Build();

        await new Declarer(new UnexpectedConnectionProvider(), configuration)
            .DeclareQueueAsync("queue", channel, cancellation.Token);

        Assert.Equal([
                nameof(IChannel.ExchangeDeclareAsync), nameof(IChannel.QueueDeclareAsync),
                nameof(IChannel.BasicQosAsync), nameof(IChannel.QueueBindAsync), nameof(IChannel.QueueBindAsync)
            ],
            recording.Calls.Select(call => call.Method));
        var exchange = recording.Calls[0].Arguments;
        Assert.Equal("exchange", exchange["exchange"]);
        Assert.Equal(ExchangeType.Direct, exchange["type"]);
        Assert.Equal(false, exchange["durable"]);
        Assert.Equal(true, exchange["autoDelete"]);
        var queue = recording.Calls[1].Arguments;
        Assert.Equal("queue", queue["queue"]);
        Assert.Equal(false, queue["durable"]);
        Assert.Equal(true, queue["exclusive"]);
        Assert.Equal(true, queue["autoDelete"]);
        Assert.Equal(true, queue["noWait"]);
        Assert.Equal(arguments, Assert.IsAssignableFrom<IDictionary<string, object?>>(queue["arguments"]));
        var qos = recording.Calls[2].Arguments;
        Assert.Equal((uint)12, qos["prefetchSize"]);
        Assert.Equal((ushort)7, qos["prefetchCount"]);
        Assert.Equal(true, qos["global"]);
        Assert.Equal(["one", "two"], recording.Calls.Skip(3).Select(call => call.Arguments["routingKey"]));
        Assert.All(recording.Calls.Skip(3), call =>
        {
            Assert.Equal("queue", call.Arguments["queue"]);
            Assert.Equal("exchange", call.Arguments["exchange"]);
            Assert.Equal(true, call.Arguments["noWait"]);
            Assert.Equal(arguments, Assert.IsAssignableFrom<IDictionary<string, object?>>(call.Arguments["arguments"]));
        });
        Assert.All(recording.Calls, call => Assert.Equal(cancellation.Token, call.Arguments["cancellationToken"]));
    }

    private sealed class UnexpectedConnectionProvider : IConnectionProvider
    {
        public void Dispose()
        {
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        public Task<IConnection> GetAsync(CancellationToken cancellationToken) =>
            throw new InvalidOperationException("A supplied channel must not acquire a connection.");
    }
}