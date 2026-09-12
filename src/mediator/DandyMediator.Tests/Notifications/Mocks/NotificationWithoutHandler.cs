using DandyMediator.Tests.Mocks;

namespace DandyMediator.Tests.Notifications.Mocks;

internal sealed record NotificationWithoutHandler(CounterCallback Callback) : INotification;
