namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public interface ISubscriber<in TEvent>
    where TEvent : class
{
    Task HandleAsync(TEvent @event, SubscriberContext context, CancellationToken cancellationToken);
}