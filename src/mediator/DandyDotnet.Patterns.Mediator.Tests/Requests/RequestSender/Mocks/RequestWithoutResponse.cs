using DandyDotnet.Patterns.Mediator.Tests.Mocks;
using DandyDotnet.Patterns.Mediator;

namespace DandyDotnet.Patterns.Mediator.Tests.Requests.RequestSender.Mocks;

internal sealed record RequestWithoutResponse(CounterCallback Callback) : IRequest;

internal sealed class RequestWithoutResponseHandler : IRequestHandler<RequestWithoutResponse>
{
    public Task HandleAsync(RequestWithoutResponse request, CancellationToken cancellationToken)
    {
        request.Callback.Success();
        return Task.CompletedTask;
    }
}