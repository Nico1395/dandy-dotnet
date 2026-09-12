using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.Middleware.Mocks;

internal sealed record RequestWithResponseAndWithMultipleMiddlewares(OrderedCallback Callback) : IRequest<bool>;

internal sealed class RequestWithResponseAndWithMultipleMiddlewaresMiddleware1 : IRequestMiddleware<RequestWithResponseAndWithMultipleMiddlewares, bool>
{
    public Task<bool> InterceptAsync(RequestWithResponseAndWithMultipleMiddlewares request, RequestHandlerDelegate<bool> nextStep, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return nextStep.Invoke();
    }
}

internal sealed class RequestWithResponseAndWithMultipleMiddlewaresMiddleware2 : IRequestMiddleware<RequestWithResponseAndWithMultipleMiddlewares, bool>
{
    public Task<bool> InterceptAsync(RequestWithResponseAndWithMultipleMiddlewares request, RequestHandlerDelegate<bool> nextStep, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return nextStep.Invoke();
    }
}

internal sealed class RequestWithResponseAndWithMultipleMiddlewaresHandler : IRequestHandler<RequestWithResponseAndWithMultipleMiddlewares, bool>
{
    public Task<bool> HandleAsync(RequestWithResponseAndWithMultipleMiddlewares request, CancellationToken cancellationToken)
    {
        request.Callback.Success(this);
        return Task.FromResult(true);
    }
}