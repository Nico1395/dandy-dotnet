using DandyMediator.Tests.Mocks;

namespace DandyMediator.Tests.Requests.RequestSender.Mocks;

internal sealed record RequestWithoutResponse(CounterCallback Callback) : IRequest;

internal sealed class RequestWithoutResponseHandler : IRequestHandler<RequestWithoutResponse>
{
    public Task HandleAsync(RequestWithoutResponse request, CancellationToken cancellationToken)
    {
        request.Callback.Success();
        return Task.CompletedTask;
    }
}