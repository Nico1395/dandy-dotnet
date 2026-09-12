namespace DandyMediator.Tests.Requests.RequestSender.Mocks;

internal sealed record RequestWithResponse : IRequest<bool>;

internal sealed class RequestWithResponseHandler : IRequestHandler<RequestWithResponse, bool>
{
    public Task<bool> HandleAsync(RequestWithResponse request, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }
}
