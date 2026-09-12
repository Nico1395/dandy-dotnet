using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyMediator;

namespace DandyDotnet.Patterns.Mediator.Tests.Notifications.Mocks;

internal sealed record NotificationWithoutHandler(CounterCallback Callback) : INotification;
