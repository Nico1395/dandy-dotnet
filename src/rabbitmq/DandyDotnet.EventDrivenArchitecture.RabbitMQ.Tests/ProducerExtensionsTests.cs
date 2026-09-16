using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class ProducerExtensionsTests
{
    [Fact]
    public async Task StringIdOverload_ForwardsMessageIdAndTimestamp()
    {
        var producer = new RecordingProducer();
        var timestamp = DateTime.UtcNow;

        await producer.ProduceAsync("exchange", "routing", "id", timestamp, new object(), CancellationToken.None);

        Assert.Equal("exchange", producer.Exchange);
        Assert.Equal(["routing"], producer.RoutingKeys);
        Assert.Equal("id", producer.Properties!.MessageId);
        Assert.Equal(timestamp.Ticks, producer.Properties.Timestamp!.UnixTime);
    }

    [Fact]
    public async Task GuidOverload_ConvertsGuidToString()
    {
        var producer = new RecordingProducer();
        var id = Guid.NewGuid();

        await producer.ProduceAsync(id, DateTime.UtcNow, new object(), CancellationToken.None);

        Assert.Equal(id.ToString(), producer.Properties!.MessageId);
    }

    [Fact]
    public async Task IMessageOverload_UsesMessageMetadata()
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();

        await producer.ProduceAsync(message, CancellationToken.None);

        Assert.Equal(message.Id.ToString(), producer.Properties!.MessageId);
        Assert.Equal(message.Timestamp.Ticks, producer.Properties.Timestamp!.UnixTime);
    }

    [Fact]
    public async Task CustomPropertiesOverload_PreservesProperties()
    {
        var producer = new RecordingProducer();
        var properties = new BasicProperties { CorrelationId = "correlation" };

        await producer.ProduceAsync("exchange", ["routing"], "id", DateTime.UtcNow, new object(), properties, CancellationToken.None);

        Assert.Same(properties, producer.Properties);
        Assert.Equal("correlation", producer.Properties.CorrelationId);
    }

    [Fact]
    public async Task SingleRoutingKeyOverload_ForwardsOneKey()
    {
        var producer = new RecordingProducer();

        await producer.ProduceAsync("exchange", "routing", new TestMessage(), CancellationToken.None);

        Assert.Equal(["routing"], producer.RoutingKeys);
    }

    private sealed class RecordingProducer : IProducer
    {
        public string? Exchange { get; private set; }
        public string[]? RoutingKeys { get; private set; }
        public object? Message { get; private set; }
        public BasicProperties? Properties { get; private set; }

        public Task ProduceAsync(string? exchange, IEnumerable<string>? routingKeys, object message, BasicProperties? properties, CancellationToken cancellationToken)
        {
            Exchange = exchange;
            RoutingKeys = routingKeys?.ToArray();
            Message = message;
            Properties = properties;
            return Task.CompletedTask;
        }
    }

    private sealed class TestMessage : IMessage
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
