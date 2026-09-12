using DandyMediator.Tests.Mocks;

namespace DandyMediator.Tests.Requests.Middleware.Mocks;

internal sealed record RequestWithMiddleware(OrderedCallback Callback) : IRequest;

internal sealed class RequestWithMiddlewareMiddleware : IRequestMiddleware<RequestWithMiddleware>
{
    public Task InterceptAsync(RequestWithMiddleware request, RequestHandlerDelegate nextStep, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return nextStep.Invoke();
    }
}

internal sealed class RequestWithMiddlewareHandler : IRequestHandler<RequestWithMiddleware>
{
    public Task HandleAsync(RequestWithMiddleware request, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return Task.CompletedTask;
    }
}