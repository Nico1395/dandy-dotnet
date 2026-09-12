namespace DandyMediator;

/// <summary>
/// Service publishes <see cref="INotification"/>.
/// </summary>
public interface INotificationPublisher
{
    /// <summary>
    /// Publishes the given <paramref name="notification"/> of type <typeparamref name="TNotification"/>.
    /// </summary>
    /// <typeparam name="TNotification">Type of notification being published.</typeparam>
    /// <param name="notification">Notification being published.</param>
    /// <param name="cancellationToken">Cancellation token for aborting asynchronous operations.</param>
    Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
}
