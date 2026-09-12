using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Notifications;

namespace DandyDotnet.Patterns.Mediator.Tests.Notifications.Mocks;

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