using System.Text;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

[Collection("RabbitMQ integration")]
public sealed class ProducerConsumerIntegrationTests(IntegrationFixture fixture) : IClassFixture<IntegrationFixture>
{
    [Fact]
    public async Task ProduceAsync_PublishesConfiguredMessage_ConsumerReceivesDeserializedMessage()
    {
        IntegrationMessageConsumer.Reset();
        var message = new IntegrationMessage { Content = "hello" };
        await fixture.CreateProducer().ProduceAsync(message, CancellationToken.None);
        var received = await WaitForMessageAsync();
        Assert.Equal(message.Content, received.Message.Content);
        Assert.Equal(fixture.ExchangeName, received.Context.DeliverArgs.Exchange);
        Assert.Equal(fixture.RoutingKey, received.Context.DeliverArgs.RoutingKey);
    }

    [Fact]
    public async Task ProduceAsync_UsesExplicitExchangeAndRoutingKeys()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("explicit");
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, "explicit", new IntegrationMessage { Content = "explicit" }, CancellationToken.None);
        Assert.Equal("explicit", (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).RoutingKey);
    }

    [Fact]
    public async Task ProduceAsync_UsesConfiguredExchangeAndRoutingKeys_WhenArgumentsAreNull()
    {
        await using var probe = await fixture.CreateProbeQueueAsync(fixture.RoutingKey);
        await fixture.CreateProducer().ProduceAsync(null, null, new IntegrationMessage { Content = "configured" }, null, CancellationToken.None);
        var delivery = await WaitForDeliveryAsync(probe.Channel, probe.QueueName);
        Assert.Equal(fixture.ExchangeName, delivery.Exchange);
        Assert.Equal(fixture.RoutingKey, delivery.RoutingKey);
    }

    [Fact]
    public async Task ProduceAsync_PublishesToMultipleRoutingKeys()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("one", "two");
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, ["one", "two"], new IntegrationMessage { Content = "multiple" }, null, CancellationToken.None);
        var deliveries = await WaitForDeliveriesAsync(probe.Channel, probe.QueueName, 2);
        Assert.Equal(["one", "two"], deliveries.Select(x => x.RoutingKey).OrderBy(x => x).ToArray());
    }

    [Fact]
    public async Task ProduceAsync_RemovesDuplicateAndWhitespaceRoutingKeys()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("one", "two");
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, [" one ", "one", " ", "two", "two"], new IntegrationMessage { Content = "distinct" }, null, CancellationToken.None);
        var deliveries = await WaitForDeliveriesAsync(probe.Channel, probe.QueueName, 2);
        Assert.Equal(2, deliveries.Count);
        Assert.Equal(["one", "two"], deliveries.Select(x => x.RoutingKey).OrderBy(x => x).ToArray());
    }

    [Fact]
    public async Task ProduceAsync_SetsMessageTypeProperty()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("type");
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, "type", new IntegrationMessage { Content = "type" }, null, CancellationToken.None);
        Assert.Equal(nameof(IntegrationMessage), (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties.Type);
    }

    [Fact]
    public async Task ProduceAsync_PreservesExplicitMessageTypeProperty()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("type-explicit");
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, "type-explicit", new IntegrationMessage { Content = "type" }, new BasicProperties { Type = "custom-type" }, CancellationToken.None);
        Assert.Equal("custom-type", (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties.Type);
    }

    [Fact]
    public async Task ProduceAsync_SerializesAndEncodesPayload()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("payload");
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, "payload", new IntegrationMessage { Content = "encoded payload" }, null, CancellationToken.None);
        Assert.Contains("encoded payload", global::System.Text.Encoding.UTF8.GetString((await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).Body.ToArray()));
    }

    [Fact]
    public async Task ProduceAsync_ReusesProducerChannel()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("reuse");
        var producer = fixture.CreateProducer();
        await producer.ProduceAsync(fixture.ExchangeName, "reuse", new IntegrationMessage { Content = "one" }, null, CancellationToken.None);
        await producer.ProduceAsync(fixture.ExchangeName, "reuse", new IntegrationMessage { Content = "two" }, null, CancellationToken.None);
        Assert.Equal(2, (await WaitForDeliveriesAsync(probe.Channel, probe.QueueName, 2)).Count);
    }

    [Fact]
    public async Task ProduceAsync_WithIMessageExtension_UsesMessageIdAndTimestamp()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("identity");
        var message = new IntegrationMessage { Content = "identity" };
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, "identity", message, CancellationToken.None);
        var properties = (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties;
        Assert.Equal(message.Id.ToString(), properties.MessageId);
        Assert.Equal(message.Timestamp.Ticks, properties.Timestamp!.UnixTime);
    }

    [Fact]
    public async Task ProduceAsync_WithStringIdExtension_SetsMessageProperties()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("string-id");
        var timestamp = DateTime.UtcNow;
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, "string-id", "message-id", timestamp, new IntegrationMessage { Content = "id" }, CancellationToken.None);
        var properties = (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties;
        Assert.Equal("message-id", properties.MessageId);
        Assert.Equal(timestamp.Ticks, properties.Timestamp!.UnixTime);
    }

    [Fact]
    public async Task ProduceAsync_WithGuidIdExtension_SetsStringMessageId()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("guid-id");
        var id = Guid.NewGuid();
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, "guid-id", id, DateTime.UtcNow, new IntegrationMessage { Content = "id" }, CancellationToken.None);
        Assert.Equal(id.ToString(), (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties.MessageId);
    }

    [Fact]
    public async Task ProduceAsync_WithCustomProperties_MergesExtensionPropertiesCorrectly()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("properties");
        var properties = new BasicProperties { CorrelationId = "correlation", Headers = new Dictionary<string, object?> { ["header"] = "value" } };
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, "properties", "id", DateTime.UtcNow, new IntegrationMessage { Content = "properties" }, properties, CancellationToken.None);
        var received = (await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties;
        Assert.Equal("id", received.MessageId);
        Assert.Equal("correlation", received.CorrelationId);
        Assert.Equal("value", global::System.Text.Encoding.UTF8.GetString((byte[])received.Headers!["header"]));
    }

    [Fact]
    public async Task ProduceAsync_WithUnknownMessageTypeAndExplicitRoutingData()
    {
        await using var probe = await fixture.CreateProbeQueueAsync("unknown");
        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, ["unknown"], new { Content = "unknown" }, null, CancellationToken.None);
        Assert.NotNull((await WaitForDeliveryAsync(probe.Channel, probe.QueueName)).BasicProperties.Type);
    }

    [Fact]
    public async Task ProduceAsync_WithUnknownMessageTypeAndMissingMetadata_Throws()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() => fixture.CreateProducer().ProduceAsync(new NonConfiguredMessage { Content = "missing" }, CancellationToken.None));
    }

    [Fact]
    public async Task ProduceAsync_ToMissingExchange_Fails()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        var failed = false;
        try
        {
            await producer.ProduceAsync("missing-exchange", ["missing"], new IntegrationMessage { Content = "missing" }, null, CancellationToken.None);
        }
        catch (Exception)
        {
            failed = true;
        }

        if (!failed)
            await Assert.ThrowsAnyAsync<Exception>(() => producer.ProduceAsync("missing-exchange", ["missing"], new IntegrationMessage { Content = "missing" }, null, CancellationToken.None));
    }

    [Fact]
    public async Task ConsumerReturningNackRequeue_MessageIsDeliveredAgain()
    {
        IntegrationMessageConsumer.Reset();
        IntegrationMessageConsumer.Result = ConsumerResult.NackRequeue();
        await fixture.CreateProducer().ProduceAsync(new IntegrationMessage { Content = "retry" }, CancellationToken.None);
        var first = await WaitForMessageAsync();
        IntegrationMessageConsumer.Result = ConsumerResult.Ack();
        var second = await WaitForMessageAsync(2);
        Assert.Equal(first.Message.Id, second.Message.Id);
        Assert.True(second.Context.DeliverArgs.Redelivered);
    }

    [Fact]
    public async Task ConsumerReturningAck_RemovesMessageFromQueue()
    {
        IntegrationMessageConsumer.Reset();
        await fixture.CreateProducer().ProduceAsync(new IntegrationMessage { Content = "ack" }, CancellationToken.None);
        await WaitForMessageAsync();
        await WaitForQueueCountAsync(0);
    }

    [Fact]
    public async Task ConsumerReturningNack_RemovesMessageWithoutRequeue()
    {
        IntegrationMessageConsumer.Reset();
        IntegrationMessageConsumer.Result = ConsumerResult.Nack();
        await fixture.CreateProducer().ProduceAsync(new IntegrationMessage { Content = "nack" }, CancellationToken.None);
        await WaitForMessageAsync();
        await WaitForQueueCountAsync(0);
    }

    [Fact]
    public async Task ConsumerReturningAckMultiple_AcknowledgesMultipleDeliveries()
    {
        IntegrationMessageConsumer.Reset();
        IntegrationMessageConsumer.Result = ConsumerResult.AckMultiple();
        var producer = fixture.CreateProducer();
        await producer.ProduceAsync(new IntegrationMessage { Content = "one" }, CancellationToken.None);
        await producer.ProduceAsync(new IntegrationMessage { Content = "two" }, CancellationToken.None);
        await WaitForMessageAsync(2);
        await WaitForQueueCountAsync(0);
    }

    [Fact]
    public async Task ConsumerReturningNackRequeueMultiple_RequeuesMultipleDeliveries()
    {
        IntegrationMessageConsumer.Reset();
        IntegrationMessageConsumer.Result = ConsumerResult.NackRequeueMultiple();
        var producer = fixture.CreateProducer();
        await producer.ProduceAsync(new IntegrationMessage { Content = "one" }, CancellationToken.None);
        await producer.ProduceAsync(new IntegrationMessage { Content = "two" }, CancellationToken.None);
        await WaitForMessageAsync(2);
        IntegrationMessageConsumer.Result = ConsumerResult.Ack();
        await WaitForMessageAsync(4);
        Assert.Contains(IntegrationMessageConsumer.Received, x => x.Context.DeliverArgs.Redelivered);
    }

    [Fact]
    public async Task ConsumerException_DefaultsToNackWithoutRequeue()
    {
        IntegrationMessageConsumer.Reset();
        IntegrationMessageConsumer.Throw = true;
        await fixture.CreateProducer().ProduceAsync(new IntegrationMessage { Content = "exception" }, CancellationToken.None);
        await WaitForMessageAsync();
        await WaitForQueueCountAsync(0);
    }

    [Fact]
    public async Task MessageWithUnknownTypeKey_IsNacked()
    {
        IntegrationMessageConsumer.Reset();
        await fixture.PublishRawAsync("unknown-type", global::System.Text.Encoding.UTF8.GetBytes("{}"));
        await WaitForQueueCountAsync(0);
        Assert.Empty(IntegrationMessageConsumer.Received);
    }

    [Fact]
    public async Task MessageWithoutTypeProperty_IsNacked()
    {
        IntegrationMessageConsumer.Reset();
        var connection = await fixture.GetConnectionProvider().GetAsync(CancellationToken.None);
        await using var channel = await connection.CreateChannelAsync();
        await channel.BasicPublishAsync(fixture.ExchangeName, fixture.RoutingKey, true, new BasicProperties(), global::System.Text.Encoding.UTF8.GetBytes("{}"));
        await WaitForQueueCountAsync(0);
        Assert.Empty(IntegrationMessageConsumer.Received);
    }

    [Fact]
    public async Task MessageWithEmptyEncodedBody_IsNacked()
    {
        IntegrationMessageConsumer.Reset();
        await fixture.PublishRawAsync(nameof(IntegrationMessage), ReadOnlyMemory<byte>.Empty);
        await WaitForQueueCountAsync(0);
        Assert.Empty(IntegrationMessageConsumer.Received);
    }

    [Fact]
    public async Task MessageWithInvalidSerializedBody_IsNacked()
    {
        IntegrationMessageConsumer.Reset();
        await fixture.PublishRawAsync(nameof(IntegrationMessage), global::System.Text.Encoding.UTF8.GetBytes("not-json"));
        await WaitForQueueCountAsync(0);
        Assert.Empty(IntegrationMessageConsumer.Received);
    }

    [Fact]
    public async Task DeserializationFailure_DoesNotInvokeConsumerOrInterceptor()
    {
        IntegrationMessageConsumer.Reset();
        await fixture.PublishRawAsync(nameof(IntegrationMessage), global::System.Text.Encoding.UTF8.GetBytes("{ invalid"));
        await WaitForQueueCountAsync(0);
        Assert.Empty(IntegrationMessageConsumer.Received);
    }

    private async Task WaitForQueueCountAsync(uint expected)
    {
        var timeout = DateTime.UtcNow.AddSeconds(10);
        while (DateTime.UtcNow < timeout)
        {
            if (await fixture.GetQueueMessageCountAsync() == expected)
                return;
            await Task.Delay(25);
        }
        Assert.Equal(expected, await fixture.GetQueueMessageCountAsync());
    }

    private static async Task<(IntegrationMessage Message, ConsumerContext Context)> WaitForMessageAsync(int minimumCount = 1)
    {
        var timeout = DateTime.UtcNow.AddSeconds(10);
        while (DateTime.UtcNow < timeout)
        {
            if (IntegrationMessageConsumer.Received.Count >= minimumCount)
                return IntegrationMessageConsumer.Received.Last();
            await Task.Delay(25);
        }
        throw new TimeoutException("The expected message was not consumed.");
    }

    private static async Task<BasicGetResult> WaitForDeliveryAsync(IChannel channel, string queue)
    {
        var timeout = DateTime.UtcNow.AddSeconds(10);
        while (DateTime.UtcNow < timeout)
        {
            var result = await channel.BasicGetAsync(queue, autoAck: true);
            if (result != null)
                return result;
            await Task.Delay(25);
        }
        throw new TimeoutException($"The expected delivery was not available in queue '{queue}'.");
    }

    private static async Task<List<BasicGetResult>> WaitForDeliveriesAsync(IChannel channel, string queue, int count)
    {
        var deliveries = new List<BasicGetResult>();
        var timeout = DateTime.UtcNow.AddSeconds(10);
        while (DateTime.UtcNow < timeout && deliveries.Count < count)
        {
            var result = await channel.BasicGetAsync(queue, autoAck: true);
            if (result != null)
                deliveries.Add(result);
            else
                await Task.Delay(25);
        }
        if (deliveries.Count != count)
            throw new TimeoutException($"Expected {count} deliveries but received {deliveries.Count}.");
        return deliveries;
    }
}
