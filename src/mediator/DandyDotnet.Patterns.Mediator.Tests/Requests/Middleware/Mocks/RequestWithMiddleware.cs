using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.Middleware.Mocks;

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