using DandyMediator.Tests.Fixtures;
using DandyMediator.Tests.Mocks;
using DandyMediator.Tests.Notifications.Mocks;

namespace DandyMediator.Tests.Notifications;

public class NotificationPublisherTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public async Task NotificationWithOneHandler_HandledOnce()
    {
        var callback = new CounterCallback();
        var notification = new NotificationWithOneHandler(callback);

        await fixture.GetMediator().PublishAsync(notification);

        Assert.Equal(1, callback.Successes);
    }

    [Fact]
    public async Task NotificationWithMultipleHandler_HandledTwice()
    {
        var callback = new CounterCallback();
        var notification = new NotificationWithMultipleHandlers(callback);

        await fixture.GetMediator().PublishAsync(notification);

        Assert.Equal(2, callback.Successes);
    }

    [Fact]
    public async Task NotificationWithoutHandler_NotHandled()
    {
        var callback = new CounterCallback();
        var notification = new NotificationWithoutHandler(callback);

        await fixture.GetMediator().PublishAsync(notification);

        Assert.Equal(0, callback.Successes);
    }

    [Fact]
    public async Task NotificationWithExceptionHandler_Caught()
    {
        var callback = new CounterCallback();
        var notification = new NotificationWithExceptionHandler(callback);

        await Assert.ThrowsAnyAsync<Exception>(() => fixture.GetMediator().PublishAsync(notification));
        Assert.Equal(1, callback.Successes);
    }
}