using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.Middleware.Mocks;

internal sealed record RequestWithoutMiddleware(OrderedCallback Callback) : IRequest;

internal sealed class RequestWithoutMiddlewareHandler : IRequestHandler<RequestWithoutMiddleware>
{
    public Task HandleAsync(RequestWithoutMiddleware request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}