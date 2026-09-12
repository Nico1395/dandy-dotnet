using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.Middleware.Mocks;

internal sealed record RequestWithResponseAndMiddleware(OrderedCallback Callback) : IRequest<bool>;

internal sealed class RequestWithResponseAndMiddlewareMiddleware : IRequestMiddleware<RequestWithResponseAndMiddleware, bool>
{
    public Task<bool> InterceptAsync(RequestWithResponseAndMiddleware request, RequestHandlerDelegate<bool> nextStep, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return nextStep.Invoke();
    }
}

internal sealed class RequestWithResponseAndMiddlewareHandler : IRequestHandler<RequestWithResponseAndMiddleware, bool>
{
    public Task<bool> HandleAsync(RequestWithResponseAndMiddleware request, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return Task.FromResult(true);
    }
}