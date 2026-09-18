namespace DandyDotnet.Patterns.EventSourcing.Outbox;

public enum OutboxEventConsumerType
{
    Subscriber = 0,
    Projection = 1,
}