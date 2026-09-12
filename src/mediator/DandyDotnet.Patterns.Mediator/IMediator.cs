namespace DandyDotnet.Patterns.Mediator;

/// <summary>
/// Mediator service combining a request sender and a notification publisher.
/// </summary>
public interface IMediator : IRequestSender, INotificationPublisher
{
}
