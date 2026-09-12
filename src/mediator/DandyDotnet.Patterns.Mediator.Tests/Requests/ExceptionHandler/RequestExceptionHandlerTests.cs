using DandyDotnet.Patterns.Mediator.Tests.Fixtures;
using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator.Tests.Requests.ExceptionHandler.Mocks;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.ExceptionHandler;

public class RequestExceptionHandlerTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public async Task RequestWithoutResponse_ExceptionCaught()
    {
        var callback = new CounterCallback();
        var request = new ExceptionRequestWithoutResponse(callback);

        await Assert.ThrowsAnyAsync<Exception>(() => fixture.GetMediator().SendAsync(request));
        Assert.Equal(1, callback.Successes);
    }
    
    [Fact]
    public async Task RequestWithoutResponseAndMiddleware_ExceptionCaught()
    {
        var callback = new CounterCallback();
        var request = new ExceptionRequestWithoutResponseAndMiddleware(callback);

        await Assert.ThrowsAnyAsync<Exception>(() => fixture.GetMediator().SendAsync(request));
        Assert.Equal(1, callback.Successes);
    }
    
    [Fact]
    public async Task RequestWithResponse_ExceptionCaught()
    {
        var callback = new CounterCallback();
        var request = new ExceptionRequestWithResponse(callback);

        await Assert.ThrowsAnyAsync<Exception>(() => fixture.GetMediator().SendAsync(request));
        Assert.Equal(1, callback.Successes);
    }
    
    [Fact]
    public async Task RequestWithResponseAndMiddleware_ExceptionCaught()
    {
        var callback = new CounterCallback();
        var request = new ExceptionRequestWithResponseAndMiddleware(callback);

        await Assert.ThrowsAnyAsync<Exception>(() => fixture.GetMediator().SendAsync(request));
        Assert.Equal(1, callback.Successes);
    }
}