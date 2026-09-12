using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;

namespace DandyDotnet.Patterns.Mediator.Tests.Notifications.Mocks;

internal sealed record NotificationWithExceptionHandler(CounterCallback Callback) : INotification;

internal sealed class NotificationWithExceptionHandlerExceptionHandler : INotificationExceptionHandler<NotificationWithExceptionHandler>
{
    public Task HandleAsync(NotificationWithExceptionHandler notificationWithExceptionHandler, Exception exception, CancellationToken cancellationToken)
    {
        notificationWithExceptionHandler.Callback.Success();
        return Task.CompletedTask;
    }
}

internal sealed class NotificationWithExceptionHandlerHandler : INotificationHandler<NotificationWithExceptionHandler>
{
    public Task HandleAsync(NotificationWithExceptionHandler notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
