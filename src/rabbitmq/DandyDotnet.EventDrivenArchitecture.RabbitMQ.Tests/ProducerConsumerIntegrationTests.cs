using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Mocks;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

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
    public async Task ProduceAsync_WithIMessageExtension_PreservesMessageIdentity()
    {
        IntegrationMessageConsumer.Reset();
        var message = new IntegrationMessage { Content = "identity" };

        await fixture.CreateProducer().ProduceAsync(message, CancellationToken.None);

        var received = await WaitForMessageAsync();
        Assert.Equal(message.Id, received.Message.Id);
        Assert.Equal(message.Timestamp, received.Message.Timestamp);
    }

    [Fact]
    public async Task ConsumerReturningNackRequeue_MessageIsReceivedAgain()
    {
        IntegrationMessageConsumer.Reset();
        IntegrationMessageConsumer.Result = ConsumerResult.NackRequeue();
        var message = new IntegrationMessage { Content = "retry" };

        await fixture.CreateProducer().ProduceAsync(message, CancellationToken.None);

        var first = await WaitForMessageAsync();
        IntegrationMessageConsumer.Result = ConsumerResult.Ack();
        var second = await WaitForMessageAsync(minimumCount: 2);

        Assert.Equal(first.Message.Id, second.Message.Id);
        Assert.True(second.Context.DeliverArgs.Redelivered);
    }

    [Fact]
    public async Task ProduceAsync_WithExplicitExchangeAndRoutingKey_IsConsumed()
    {
        IntegrationMessageConsumer.Reset();
        var message = new IntegrationMessage { Content = "explicit" };

        await fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, fixture.RoutingKey, message, CancellationToken.None);

        Assert.Equal(message.Content, (await WaitForMessageAsync()).Message.Content);
    }

    [Fact]
    public async Task ProduceAsync_WithEmptyRoutingKeys_Throws()
    {
        var message = new IntegrationMessage { Content = "invalid" };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            fixture.CreateProducer().ProduceAsync(fixture.ExchangeName, [], message, null, CancellationToken.None));
    }

    [Fact]
    public async Task ProduceAsync_WithWhitespaceExchange_Throws()
    {
        var message = new IntegrationMessage { Content = "invalid" };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            fixture.CreateProducer().ProduceAsync(" ", fixture.RoutingKey, message, null, CancellationToken.None));
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
}
