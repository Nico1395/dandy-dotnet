using DandyMediator.Tests.Fixtures;
using DandyMediator.Tests.Mocks;
using DandyMediator.Tests.Requests.RequestSender.Mocks;

namespace DandyMediator.Tests.Requests.RequestSender;

public class RequestSenderTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public async Task RequestWithoutResponse_Handled()
    {
        var callback = new CounterCallback();
        var request = new RequestWithoutResponse(callback);

        await fixture.GetMediator().SendAsync(request);

        Assert.Equal(1, callback.Successes);
    }

    [Fact]
    public async Task RequestWithoutResponseButNoHandler_Throws()
    {
        var request = new RequestWithoutResponseButNoHandler();

        await Assert.ThrowsAnyAsync<Exception>(() => fixture.GetMediator().SendAsync(request));
    }

    [Fact]
    public async Task RequestWithResponseButNoHandler_Handled()
    {
        var request = new RequestWithResponse();
        var result = await fixture.GetMediator().SendAsync(request);

        Assert.True(result);
    }

    [Fact]
    public async Task RequestWithResponseButNoHandler_Throws()
    {
        var request = new RequestWithResponseButNoHandler();

        await Assert.ThrowsAnyAsync<Exception>(() => fixture.GetMediator().SendAsync(request));
    }
}