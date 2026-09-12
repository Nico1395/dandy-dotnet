using DandyMediator.Tests.Mocks;

namespace DandyMediator.Tests.Requests.Middleware.Mocks;

internal sealed record RequestWithMultipleMiddlewares(OrderedCallback Callback) : IRequest;

internal sealed class RequestWithMultipleMiddlewaresMiddleware1 : IRequestMiddleware<RequestWithMultipleMiddlewares>
{
    public Task InterceptAsync(RequestWithMultipleMiddlewares request, RequestHandlerDelegate nextStep, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return nextStep.Invoke();
    }
}

internal sealed class RequestWithMultipleMiddlewaresMiddleware2 : IRequestMiddleware<RequestWithMultipleMiddlewares>
{
    public Task InterceptAsync(RequestWithMultipleMiddlewares request, RequestHandlerDelegate nextStep, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return nextStep.Invoke();
    }
}

internal sealed class RequestWithMultipleMiddlewaresHandler : IRequestHandler<RequestWithMultipleMiddlewares>
{
    public Task HandleAsync(RequestWithMultipleMiddlewares request, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return Task.CompletedTask;
    }
}