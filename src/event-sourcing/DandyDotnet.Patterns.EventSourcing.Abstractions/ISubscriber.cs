namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

public interface ISubscriber<in TEvent>
    where TEvent : class
{
    Task HandleAsync(TEvent subscribed, SubscriberContext context, CancellationToken cancellationToken);
}