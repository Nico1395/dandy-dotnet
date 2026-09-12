using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;

namespace DandyDotnet.Patterns.Mediator.Tests.Notifications.Mocks;

internal sealed record NotificationWithoutHandler(CounterCallback Callback) : INotification;
