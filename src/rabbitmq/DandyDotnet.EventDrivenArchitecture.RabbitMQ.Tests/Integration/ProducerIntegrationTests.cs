using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Integration;

[Collection(RabbitMqIntegrationCollection.Name)]
[Trait("Category", "Integration")]
public sealed class ProducerIntegrationTests(IntegrationFixture fixture)
{
    [Fact]
    public async Task ProduceAsync_UsesExplicitExchangeAndRoutingKeys()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("explicit");
        await producer.ProduceAsync(fixture.ExchangeName, "explicit", new IntegrationMessage { Content = "explicit" }, CancellationToken.None);
        Assert.Equal("explicit", (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).RoutingKey);
    }

    [Fact]
    public async Task ProduceAsync_UsesConfiguredExchangeAndRoutingKeys_WhenArgumentsAreNull()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync(fixture.RoutingKey);
        await producer.ProduceAsync(null, null, new IntegrationMessage { Content = "configured" }, null, CancellationToken.None);
        var delivery = await WaitForDeliveryAsync(probe.Channel, probe.QueueName);
        Assert.Equal(fixture.ExchangeName, delivery.Exchange);
        Assert.Equal(fixture.RoutingKey, delivery.RoutingKey);
    }

    [Fact]
    public async Task ProduceAsync_PublishesToMultipleRoutingKeys()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("one", "two");
        await producer.ProduceAsync(fixture.ExchangeName, ["one", "two"], new IntegrationMessage { Content = "multiple" }, null, CancellationToken.None);
        var deliveries = await WaitForDeliveriesAsync(probe.Channel, probe.QueueName, 2);
        Assert.Equal(["one", "two"], deliveries.Select(x => x.RoutingKey).OrderBy(x => x).ToArray());
    }

    [Fact]
    public async Task ProduceAsync_RemovesDuplicateAndWhitespaceRoutingKeys()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("one", "two");
        await producer.ProduceAsync(fixture.ExchangeName, [" one ", "one", " ", "two", "two"], new IntegrationMessage { Content = "distinct" }, null, CancellationToken.None);
        var deliveries = await WaitForDeliveriesAsync(probe.Channel, probe.QueueName, 2);
        await producer.ProduceAsync(fixture.ExchangeName, "one", new IntegrationMessage { Content = "sentinel" }, null, CancellationToken.None);
        var next = await WaitForDeliveryAsync(probe.Channel, probe.QueueName);
        Assert.Equal("sentinel", System.Text.Json.JsonSerializer.Deserialize<IntegrationMessage>(next.Body.Span)!.Content);
        Assert.Equal(["one", "two"], deliveries.Select(x => x.RoutingKey).OrderBy(x => x).ToArray());
    }

    [Fact]
    public async Task ProduceAsync_SetsMessageTypeProperty()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("type");
        await producer.ProduceAsync(fixture.ExchangeName, "type", new IntegrationMessage { Content = "type" }, null, CancellationToken.None);
        Assert.Equal(nameof(IntegrationMessage), (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties.Type);
    }

    [Fact]
    public async Task ProduceAsync_PreservesExplicitMessageTypeProperty()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("type-explicit");
        await producer.ProduceAsync(fixture.ExchangeName, "type-explicit", new IntegrationMessage { Content = "type" }, new BasicProperties { Type = "custom-type" }, CancellationToken.None);
        Assert.Equal("custom-type", (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties.Type);
    }

    [Fact]
    public async Task ProduceAsync_SerializesAndEncodesPayload()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("payload");
        await producer.ProduceAsync(fixture.ExchangeName, "payload", new IntegrationMessage { Content = "encoded payload" }, null, CancellationToken.None);
        var delivery = await WaitForDeliveryAsync(probe.Channel, probe.QueueName);
        var payload = System.Text.Json.JsonSerializer.Deserialize<IntegrationMessage>(delivery.Body.Span);
        Assert.Equal("encoded payload", Assert.IsType<IntegrationMessage>(payload).Content);
    }

    [Fact]
    public async Task ProduceAsync_WithSameProducer_PublishesBothMessages()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("reuse");

        await producer.ProduceAsync(fixture.ExchangeName, "reuse", new IntegrationMessage { Content = "one" }, null, CancellationToken.None);
        await producer.ProduceAsync(fixture.ExchangeName, "reuse", new IntegrationMessage { Content = "two" }, null, CancellationToken.None);
        Assert.Equal(2, (await WaitForDeliveriesAsync(probe.Channel, probe.QueueName, 2)).Count);
    }

    [Fact]
    public async Task ProduceAsync_WithIMessageExtension_UsesMessageIdAndTimestamp()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("identity");
        var message = new IntegrationMessage { Content = "identity" };
        await producer.ProduceAsync(fixture.ExchangeName, "identity", message, CancellationToken.None);
        var properties = (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties;
        Assert.Equal(message.Id.ToString(), properties.MessageId);
        Assert.Equal(message.Timestamp.Ticks, properties.Timestamp!.UnixTime);
    }

    [Fact]
    public async Task ProduceAsync_WithStringIdExtension_SetsMessageProperties()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("string-id");
        var timestamp = DateTime.UtcNow;
        await producer.ProduceAsync(fixture.ExchangeName, "string-id", "message-id", timestamp, new IntegrationMessage { Content = "id" }, CancellationToken.None);
        var properties = (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties;
        Assert.Equal("message-id", properties.MessageId);
        Assert.Equal(timestamp.Ticks, properties.Timestamp!.UnixTime);
    }

    [Fact]
    public async Task ProduceAsync_WithGuidIdExtension_SetsStringMessageId()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("guid-id");
        var id = Guid.NewGuid();
        await producer.ProduceAsync(fixture.ExchangeName, "guid-id", id, DateTime.UtcNow, new IntegrationMessage { Content = "id" }, CancellationToken.None);
        Assert.Equal(id.ToString(), (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties.MessageId);
    }

    [Fact]
    public async Task ProduceAsync_WithCustomProperties_MergesExtensionPropertiesCorrectly()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("properties");
        var properties = new BasicProperties { CorrelationId = "correlation", Headers = new Dictionary<string, object?> { ["header"] = "value" } };
        await producer.ProduceAsync(fixture.ExchangeName, "properties", "id", DateTime.UtcNow, new IntegrationMessage { Content = "properties" }, properties, CancellationToken.None);
        var received = (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties;
        Assert.Equal("id", received.MessageId);
        Assert.Equal("correlation", received.CorrelationId);
        Assert.Equal("value", global::System.Text.Encoding.UTF8.GetString(Assert.IsType<byte[]>(received.Headers!["header"])));
    }

    [Fact]
    public async Task ProduceAsync_WithUnconfiguredMessageAndExplicitRouting_PublishesRuntimeTypeName()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await using var probe = await fixture.CreateProbeQueueAsync("unknown");
        await producer.ProduceAsync(fixture.ExchangeName, ["unknown"], new UnconfiguredMessage("unknown"), null, CancellationToken.None);
        Assert.Equal(nameof(UnconfiguredMessage), (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties.Type);
    }

    [Fact]
    public async Task ProduceAsync_WithUnknownMessageTypeAndMissingMetadata_Throws()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        await Assert.ThrowsAsync<InvalidOperationException>(() => producer.ProduceAsync(null, null, new UnconfiguredMessage("missing"), null, CancellationToken.None));
    }

    private static async Task<BasicGetResult> WaitForDeliveryAsync(IChannel channel, string queue) =>
        Assert.Single(await WaitForDeliveriesAsync(channel, queue, 1));

    private static async Task<List<BasicGetResult>> WaitForDeliveriesAsync(IChannel channel, string queue, int count)
    {
        var deliveries = new List<BasicGetResult>();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        try
        {
            while (deliveries.Count < count)
            {
                var result = await channel.BasicGetAsync(queue, autoAck: true, cancellationToken: timeout.Token);
                if (result is not null)
                    deliveries.Add(result);
                else
                    await Task.Delay(25, timeout.Token);
            }
        }
        catch (OperationCanceledException exception) when (timeout.IsCancellationRequested)
        {
            throw new TimeoutException($"Expected {count} deliveries in '{queue}' but received {deliveries.Count}.", exception);
        }

        return deliveries;
    }

    private sealed record UnconfiguredMessage(string Content);
}