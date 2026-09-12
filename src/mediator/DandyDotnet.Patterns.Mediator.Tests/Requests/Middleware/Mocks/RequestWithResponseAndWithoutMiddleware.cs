using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.Middleware.Mocks;

internal sealed record RequestWithResponseAndWithoutMiddleware(OrderedCallback Callback) : IRequest<bool>;

internal sealed class RequestWithResponseAndWithoutMiddlewareHandler : IRequestHandler<RequestWithResponseAndWithoutMiddleware, bool>
{
    public Task<bool> HandleAsync(RequestWithResponseAndWithoutMiddleware request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}