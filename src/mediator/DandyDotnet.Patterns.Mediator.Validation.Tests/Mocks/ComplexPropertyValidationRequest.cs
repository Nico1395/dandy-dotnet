using System.ComponentModel.DataAnnotations;
using DandyDotnet.Patterns.Mediator.Responses;

namespace DandyDotnet.Patterns.Mediator.Validation.Tests.Mocks;

internal sealed record ComplexProperty([StringLength(10)] string String);

internal sealed record ComplexPropertyValidationRequest([Validate] ComplexProperty ComplexProperty) : IResponseRequest<IRequestResponse>;

internal sealed class ComplexPropertyValidationRequestHandler : IRequestHandler<ComplexPropertyValidationRequest, IRequestResponse>
{
    public async Task<IRequestResponse> HandleAsync(ComplexPropertyValidationRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new RequestResponse(RequestResponseStatus.Accepted_202);
    }
}
