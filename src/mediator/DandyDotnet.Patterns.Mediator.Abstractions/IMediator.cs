using DandyDotnet.Patterns.Mediator.Abstractions.Notifications;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Abstractions;

/// <summary>
/// Mediator service combining a request sender and a notification publisher.
/// </summary>
public interface IMediator : IRequestSender, INotificationPublisher
{
}
