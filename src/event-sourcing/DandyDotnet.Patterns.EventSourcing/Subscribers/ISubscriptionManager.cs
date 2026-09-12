using DandyDotnet.Patterns.EventSourcing.Outbox;

namespace DandyDotnet.Patterns.EventSourcing.Subscribers;

internal interface ISubscriptionManager
{
    Task NotifySubscribersAsync(IEventStore? eventStore, OutboxEnvelope outboxEnvelope, SubscriberMode[] modes, CancellationToken cancellationToken);
}