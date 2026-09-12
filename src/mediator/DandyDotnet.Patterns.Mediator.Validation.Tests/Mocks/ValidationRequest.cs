using System.ComponentModel.DataAnnotations;
using DandyDotnet.Patterns.Mediator.Responses;

namespace DandyDotnet.Patterns.Mediator.Validation.Tests.Mocks;

internal sealed record ValidationRequest([StringLength(10)] string String) : IResponseRequest<IRequestResponse>;

internal sealed class ValidationRequestHandler : IRequestHandler<ValidationRequest, IRequestResponse>
{
    public async Task<IRequestResponse> HandleAsync(ValidationRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new RequestResponse(RequestResponseStatus.Accepted_202);
    }
}
