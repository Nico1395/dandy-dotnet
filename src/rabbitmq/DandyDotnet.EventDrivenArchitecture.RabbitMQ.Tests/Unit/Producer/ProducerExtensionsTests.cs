using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Producer;

public sealed class ProducerExtensionsTests
{
    // Static invocation selects the extension even where IProducer has an applicable instance overload.
    [Fact]
    public void ProduceAsync_WithStringIdAndSingleKey_ForwardsAllArgumentsAndTask()
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", "one", "string-id", message.Timestamp, message, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one"], "string-id");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ProduceAsync_WithStringIdAndSingleKeyAndProperties_ForwardsAllArgumentsAndTask(bool supplyProperties)
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();
        var properties = supplyProperties
            ? new BasicProperties
            {
                MessageId = "old-id",
                Timestamp = new AmqpTimestamp(1),
                CorrelationId = "correlation",
                Headers = new Dictionary<string, object?> { ["source"] = "test" }
            }
            : null;

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", "one", "string-id", message.Timestamp, message, properties, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one"], "string-id");
        if (supplyProperties)
        {
            Assert.Same(properties, producer.Properties);
            Assert.Equal("correlation", producer.Properties!.CorrelationId);
            Assert.Equal("test", producer.Properties.Headers!["source"]);
        }
    }

    [Fact]
    public void ProduceAsync_WithStringIdAndMultipleKeys_ForwardsAllArgumentsAndTask()
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", ["one", "two"], "string-id", message.Timestamp, message, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one", "two"], "string-id");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ProduceAsync_WithStringIdAndMultipleKeysAndProperties_ForwardsAllArgumentsAndTask(bool supplyProperties)
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();
        var properties = supplyProperties
            ? new BasicProperties
            {
                MessageId = "old-id",
                Timestamp = new AmqpTimestamp(1),
                CorrelationId = "correlation",
                Headers = new Dictionary<string, object?> { ["source"] = "test" }
            }
            : null;

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", ["one", "two"], "string-id", message.Timestamp, message, properties, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one", "two"], "string-id");
        if (supplyProperties)
        {
            Assert.Same(properties, producer.Properties);
            Assert.Equal("correlation", producer.Properties!.CorrelationId);
            Assert.Equal("test", producer.Properties.Headers!["source"]);
        }
    }

    [Fact]
    public void ProduceAsync_WithGuidIdAndDefaultRouting_ForwardsAllArgumentsAndTask()
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();

        var task = ProducerExtensions.ProduceAsync(producer, message.Id, message.Timestamp, message, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, null, null, message.Id.ToString());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ProduceAsync_WithGuidIdAndDefaultRoutingAndProperties_ForwardsAllArgumentsAndTask(bool supplyProperties)
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();
        var properties = supplyProperties
            ? new BasicProperties
            {
                MessageId = "old-id",
                Timestamp = new AmqpTimestamp(1),
                CorrelationId = "correlation",
                Headers = new Dictionary<string, object?> { ["source"] = "test" }
            }
            : null;

        var task = ProducerExtensions.ProduceAsync(producer, message.Id, message.Timestamp, message, properties, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, null, null, message.Id.ToString());
        if (supplyProperties)
        {
            Assert.Same(properties, producer.Properties);
            Assert.Equal("correlation", producer.Properties!.CorrelationId);
            Assert.Equal("test", producer.Properties.Headers!["source"]);
        }
    }

    [Fact]
    public void ProduceAsync_WithGuidIdAndSingleKey_ForwardsAllArgumentsAndTask()
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", "one", message.Id, message.Timestamp, message, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one"], message.Id.ToString());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ProduceAsync_WithGuidIdAndSingleKeyAndProperties_ForwardsAllArgumentsAndTask(bool supplyProperties)
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();
        var properties = supplyProperties
            ? new BasicProperties
            {
                MessageId = "old-id",
                Timestamp = new AmqpTimestamp(1),
                CorrelationId = "correlation",
                Headers = new Dictionary<string, object?> { ["source"] = "test" }
            }
            : null;

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", "one", message.Id, message.Timestamp, message, properties, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one"], message.Id.ToString());
        if (supplyProperties)
        {
            Assert.Same(properties, producer.Properties);
            Assert.Equal("correlation", producer.Properties!.CorrelationId);
            Assert.Equal("test", producer.Properties.Headers!["source"]);
        }
    }

    [Fact]
    public void ProduceAsync_WithGuidIdAndMultipleKeys_ForwardsAllArgumentsAndTask()
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", ["one", "two"], message.Id, message.Timestamp, message, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one", "two"], message.Id.ToString());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ProduceAsync_WithGuidIdAndMultipleKeysAndProperties_ForwardsAllArgumentsAndTask(bool supplyProperties)
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();
        var properties = supplyProperties
            ? new BasicProperties
            {
                MessageId = "old-id",
                Timestamp = new AmqpTimestamp(1),
                CorrelationId = "correlation",
                Headers = new Dictionary<string, object?> { ["source"] = "test" }
            }
            : null;

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", ["one", "two"], message.Id, message.Timestamp, message, properties, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one", "two"], message.Id.ToString());
        if (supplyProperties)
        {
            Assert.Same(properties, producer.Properties);
            Assert.Equal("correlation", producer.Properties!.CorrelationId);
            Assert.Equal("test", producer.Properties.Headers!["source"]);
        }
    }

    [Fact]
    public void ProduceAsync_WithMessageAndDefaultRouting_ForwardsAllArgumentsAndTask()
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();

        var task = ProducerExtensions.ProduceAsync(producer, message, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, null, null, message.Id.ToString());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ProduceAsync_WithMessageAndDefaultRoutingAndProperties_ForwardsAllArgumentsAndTask(bool supplyProperties)
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();
        var properties = supplyProperties
            ? new BasicProperties
            {
                MessageId = "old-id",
                Timestamp = new AmqpTimestamp(1),
                CorrelationId = "correlation",
                Headers = new Dictionary<string, object?> { ["source"] = "test" }
            }
            : null;

        var task = ProducerExtensions.ProduceAsync(producer, message, properties, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, null, null, message.Id.ToString());
        if (supplyProperties)
        {
            Assert.Same(properties, producer.Properties);
            Assert.Equal("correlation", producer.Properties!.CorrelationId);
            Assert.Equal("test", producer.Properties.Headers!["source"]);
        }
    }

    [Fact]
    public void ProduceAsync_WithMessageAndSingleKey_ForwardsAllArgumentsAndTask()
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", "one", message, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one"], message.Id.ToString());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ProduceAsync_WithMessageAndSingleKeyAndProperties_ForwardsAllArgumentsAndTask(bool supplyProperties)
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();
        var properties = supplyProperties
            ? new BasicProperties
            {
                MessageId = "old-id",
                Timestamp = new AmqpTimestamp(1),
                CorrelationId = "correlation",
                Headers = new Dictionary<string, object?> { ["source"] = "test" }
            }
            : null;

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", "one", message, properties, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one"], message.Id.ToString());
        if (supplyProperties)
        {
            Assert.Same(properties, producer.Properties);
            Assert.Equal("correlation", producer.Properties!.CorrelationId);
            Assert.Equal("test", producer.Properties.Headers!["source"]);
        }
    }

    [Fact]
    public void ProduceAsync_WithMessageAndMultipleKeys_ForwardsAllArgumentsAndTask()
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", ["one", "two"], message, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one", "two"], message.Id.ToString());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ProduceAsync_WithMessageAndMultipleKeysAndProperties_ForwardsAllArgumentsAndTask(bool supplyProperties)
    {
        var producer = new RecordingProducer();
        var message = new TestMessage();
        using var cancellation = new CancellationTokenSource();
        var properties = supplyProperties
            ? new BasicProperties
            {
                MessageId = "old-id",
                Timestamp = new AmqpTimestamp(1),
                CorrelationId = "correlation",
                Headers = new Dictionary<string, object?> { ["source"] = "test" }
            }
            : null;

        var task = ProducerExtensions.ProduceAsync(producer, "exchange", ["one", "two"], message, properties, cancellation.Token);

        AssertForwarded(producer, task, message, cancellation.Token, "exchange", ["one", "two"], message.Id.ToString());
        if (supplyProperties)
        {
            Assert.Same(properties, producer.Properties);
            Assert.Equal("correlation", producer.Properties!.CorrelationId);
            Assert.Equal("test", producer.Properties.Headers!["source"]);
        }
    }

    private static void AssertForwarded(RecordingProducer producer, Task task, TestMessage message,
        CancellationToken token, string? exchange, string[]? routingKeys, string id)
    {
        Assert.Equal(1, producer.Calls);
        Assert.Same(producer.Completion.Task, task);
        Assert.Same(message, producer.Message);
        Assert.Equal(token, producer.Token);
        Assert.Equal(exchange, producer.Exchange);
        Assert.Equal(routingKeys, producer.RoutingKeys);
        var properties = Assert.IsType<BasicProperties>(producer.Properties);
        Assert.Equal(id, properties.MessageId);
        // The current API writes DateTime ticks, not Unix seconds; preserve that behavior in this test refactor.
        Assert.Equal(message.Timestamp.Ticks, properties.Timestamp.UnixTime);
    }

    private sealed class RecordingProducer : IProducer
    {
        public TaskCompletionSource Completion { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public int Calls { get; private set; }
        public string? Exchange { get; private set; }
        public string[]? RoutingKeys { get; private set; }
        public object? Message { get; private set; }
        public BasicProperties? Properties { get; private set; }
        public CancellationToken Token { get; private set; }

        public Task ProduceAsync(string? exchange, IEnumerable<string>? routingKeys, object message, BasicProperties? properties, CancellationToken cancellationToken)
        {
            Calls++;
            Exchange = exchange;
            RoutingKeys = routingKeys?.ToArray();
            Message = message;
            Properties = properties;
            Token = cancellationToken;
            return Completion.Task;
        }
    }

    private sealed class TestMessage : IMessage
    {
        public Guid Id { get; } = Guid.Parse("b27ea8ae-6d24-4ac4-a9c3-686916e72671");
        public DateTime Timestamp { get; } = new(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc);
    }
}