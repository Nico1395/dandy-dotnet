using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Outbox;

/// <summary>
/// Types of consumers.
/// </summary>
public enum OutboxEventConsumerType
{
    /// <summary>
    /// Consumer is an event subscriber implementing <see cref="ISubscriber{TEvent}"/>.
    /// </summary>
    Subscriber = 0,
}