using DandyMediator.Tests.Fixtures;
using DandyMediator.Tests.Mocks;
using DandyMediator.Tests.Requests.Middleware.Mocks;

namespace DandyMediator.Tests.Requests.Middleware;

public class RequestMiddlewareTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public async Task RequestWithMiddleware_InterceptedBefore()
    {
        var callback = new OrderedCallback();
        var request = new RequestWithMiddleware(callback);

        await fixture.GetMediator().SendAsync(request);

        Assert.True(callback.SucceededInExpectedOrder(nameof(RequestWithMiddlewareMiddleware), nameof(RequestWithMiddlewareHandler)));
    }

    [Fact]
    public async Task RequestWithoutMiddleware_NeverIntercepted()
    {
        var callback = new OrderedCallback();
        var request = new RequestWithoutMiddleware(callback);

        await fixture.GetMediator().SendAsync(request);

        Assert.Empty(callback.Successes);
    }

    [Fact]
    public async Task RequestWithMultipleMiddlewares_InterceptedInOrder()
    {
        var callback = new OrderedCallback();
        var request = new RequestWithMultipleMiddlewares(callback);
        
        await fixture.GetMediator().SendAsync(request);
        
        Assert.True(callback.SucceededInExpectedOrder(
            nameof(RequestWithMultipleMiddlewaresMiddleware1),
            nameof(RequestWithMultipleMiddlewaresMiddleware2),
            nameof(RequestWithMultipleMiddlewaresHandler)));
    }

    [Fact]
    public async Task RequestWithResponseAndMiddleware_InterceptedBefore()
    {
        var callback = new OrderedCallback();
        var request = new RequestWithResponseAndMiddleware(callback);

        var handled = await fixture.GetMediator().SendAsync(request);

        Assert.True(handled);
        Assert.True(callback.SucceededInExpectedOrder(nameof(RequestWithResponseAndMiddlewareMiddleware), nameof(RequestWithResponseAndMiddlewareHandler)));
    }

    [Fact]
    public async Task RequestWithResponseAndWithoutMiddleware_NeverIntercepted()
    {
        var callback = new OrderedCallback();
        var request = new RequestWithResponseAndWithoutMiddleware(callback);

        await fixture.GetMediator().SendAsync(request);

        Assert.Empty(callback.Successes);
    }

    [Fact]
    public async Task RequestWithResponseAndWithMultipleMiddlewares_InterceptedInOrder()
    {
        var callback = new OrderedCallback();
        var request = new RequestWithResponseAndWithMultipleMiddlewares(callback);
        
        var handled = await fixture.GetMediator().SendAsync(request);
        
        Assert.True(handled);
        Assert.True(callback.SucceededInExpectedOrder(
            nameof(RequestWithResponseAndWithMultipleMiddlewaresMiddleware1),
            nameof(RequestWithResponseAndWithMultipleMiddlewaresMiddleware2),
            nameof(RequestWithResponseAndWithMultipleMiddlewaresHandler)));
    }
}