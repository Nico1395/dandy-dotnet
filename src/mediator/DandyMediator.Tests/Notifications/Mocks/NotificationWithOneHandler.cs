using DandyMediator.Tests.Mocks;

namespace DandyMediator.Tests.Notifications.Mocks;

internal sealed record NotificationWithOneHandler(CounterCallback Callback) : INotification;

internal sealed class NotificationWithOneHandlerHandler : INotificationHandler<NotificationWithOneHandler>
{
    public Task HandleAsync(NotificationWithOneHandler notificationWithOneHandler, CancellationToken cancellationToken)
    {
        notificationWithOneHandler.Callback.Success();
        return Task.CompletedTask;
    }
}