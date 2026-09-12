namespace DandyMediator;

/// <summary>
/// Service for handling exceptions thrown while handling a notification of type <typeparamref name="TNotification"/>.
/// </summary>
/// <typeparam name="TNotification">Type of notification the exception has been thrown for.</typeparam>
public interface INotificationExceptionHandler<in TNotification>
    where TNotification : INotification
{
    /// <summary>
    /// Handles the given <paramref name="exception"/> thrown in a handler for the <paramref name="notification"/>.
    /// </summary>
    /// <param name="notification">Notification the exception has been thrown for.</param>
    /// <param name="exception">Thrown exception.</param>
    /// <param name="cancellationToken">Cancellation token for aborting asynchronous operations.</param>
    Task HandleAsync(TNotification notification, Exception exception, CancellationToken cancellationToken);
}
