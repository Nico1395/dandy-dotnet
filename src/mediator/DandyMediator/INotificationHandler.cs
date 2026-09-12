namespace DandyMediator;

/// <summary>
/// Service handles notifications of type <typeparamref name="TNotification"/>.
/// </summary>
/// <typeparam name="TNotification">Type of notification being handled.</typeparam>
public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    /// <summary>
    /// Handles the given <paramref name="notification"/>.
    /// </summary>
    /// <param name="notification">Notification being handled.</param>
    /// <param name="cancellationToken">Cancellation token for aborting asynchronous operations.</param>
    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}
