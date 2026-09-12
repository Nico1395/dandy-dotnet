using DandyMediator.Tests.Mocks;

namespace DandyMediator.Tests.Notifications.Mocks;

internal sealed record NotificationWithMultipleHandlers(CounterCallback Callback) : INotification;

internal sealed class NotificationWithMultipleHandlersHandler1 : INotificationHandler<NotificationWithMultipleHandlers>
{
    public Task HandleAsync(NotificationWithMultipleHandlers notification, CancellationToken cancellationToken)
    {
        notification.Callback.Success();
        return Task.CompletedTask;
    }
}

internal sealed class NotificationWithMultipleHandlersHandler2 : INotificationHandler<NotificationWithMultipleHandlers>
{
    public Task HandleAsync(NotificationWithMultipleHandlers notification, CancellationToken cancellationToken)
    {
        notification.Callback.Success();
        return Task.CompletedTask;
    }
}