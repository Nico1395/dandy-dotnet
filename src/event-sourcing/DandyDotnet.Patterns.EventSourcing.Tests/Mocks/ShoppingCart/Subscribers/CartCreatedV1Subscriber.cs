using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Tests.Mocks.ShoppingCart.Events;

namespace DandyDotnet.Patterns.EventSourcing.Tests.Mocks.ShoppingCart.Subscribers;

[Subscriber(Mode = SubscriberMode.Inline)]
internal sealed class CartCreatedV1Subscriber : ISubscriber<CartCreatedV1>
{
    public Task HandleAsync(CartCreatedV1 subscribed, SubscriberContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}