using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyMediator;

namespace DandyDotnet.Patterns.Mediator.Tests.Notifications.Mocks;

internal sealed record NotificationWithOneHandler(CounterCallback Callback) : INotification;

internal sealed class NotificationWithOneHandlerHandler : INotificationHandler<NotificationWithOneHandler>
{
    public Task HandleAsync(NotificationWithOneHandler notificationWithOneHandler, CancellationToken cancellationToken)
    {
        notificationWithOneHandler.Callback.Success();
        return Task.CompletedTask;
    }
}