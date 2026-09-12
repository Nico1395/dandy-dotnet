using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Requests;

namespace DandyDotnet.Patterns.Mediator.Validation.Tests.Mocks;

internal sealed record RequestWithoutValidation(string String) : IResponseRequest<IRequestResponse>;

internal sealed record RequestWithoutValidationHandler : IRequestHandler<RequestWithoutValidation, IRequestResponse>
{
    public async Task<IRequestResponse> HandleAsync(RequestWithoutValidation request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new RequestResponse(RequestResponseStatus.Accepted_202);
    }
}
