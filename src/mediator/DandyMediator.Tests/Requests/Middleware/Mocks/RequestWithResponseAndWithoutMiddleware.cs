using DandyMediator.Tests.Mocks;

namespace DandyMediator.Tests.Requests.Middleware.Mocks;

internal sealed record RequestWithResponseAndWithoutMiddleware(OrderedCallback Callback) : IRequest<bool>;

internal sealed class RequestWithResponseAndWithoutMiddlewareHandler : IRequestHandler<RequestWithResponseAndWithoutMiddleware, bool>
{
    public Task<bool> HandleAsync(RequestWithResponseAndWithoutMiddleware request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}