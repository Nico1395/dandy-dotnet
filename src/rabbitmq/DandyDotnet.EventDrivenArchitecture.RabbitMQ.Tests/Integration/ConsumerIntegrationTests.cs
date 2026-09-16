using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Integration;

[Collection(RabbitMqIntegrationCollection.Name)]
[Trait("Category", "Integration")]
public sealed class ConsumerIntegrationTests(IntegrationFixture fixture)
{
    [Fact]
    public async Task ConsumerWorker_WithPublishedMessage_ReceivesDeserializedMessageAndDeliveryContext()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        var message = new IntegrationMessage { Content = "hello" };

        await producer.ProduceAsync(message, CancellationToken.None);
        var received = await fixture.Consumer.WaitForMessageAsync(message.Id);

        Assert.Equal(message.Id, received.Message.Id);
        Assert.Equal(message.Timestamp, received.Message.Timestamp);
        Assert.Equal(message.Content, received.Message.Content);
        Assert.Equal(fixture.ExchangeName, received.Context.DeliverArgs.Exchange);
        Assert.Equal(fixture.RoutingKey, received.Context.DeliverArgs.RoutingKey);
        Assert.False(received.Context.DeliverArgs.Redelivered);
    }

    [Fact]
    public async Task ConsumerWorker_WhenConsumerRequeues_DeliversSameMessageAgainWithRedeliveredFlag()
    {
        using var scope = fixture.CreateProducerScope();
        var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
        var message = new IntegrationMessage { Content = "retry" };

        await producer.ProduceAsync(message, CancellationToken.None);
        var first = await fixture.Consumer.WaitForMessageAsync(message.Id);
        var second = await fixture.Consumer.WaitForMessageAsync(message.Id);

        Assert.False(first.Context.DeliverArgs.Redelivered);
        Assert.True(second.Context.DeliverArgs.Redelivered);
        Assert.Equal(first.Message.Id, second.Message.Id);
        Assert.Equal(message.Content, second.Message.Content);
    }
}