using DandyMediator.Responses;

namespace DandyMediator.Validation.Tests.Mocks;

internal sealed record RequestWithoutValidation(string String) : IResponseRequest<IRequestResponse>;

internal sealed record RequestWithoutValidationHandler : IRequestHandler<RequestWithoutValidation, IRequestResponse>
{
    public async Task<IRequestResponse> HandleAsync(RequestWithoutValidation request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new RequestResponse(RequestResponseStatus.Accepted_202);
    }
}
