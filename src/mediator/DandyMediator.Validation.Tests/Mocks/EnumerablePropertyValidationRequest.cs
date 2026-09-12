using System.ComponentModel.DataAnnotations;
using DandyMediator.Responses;

namespace DandyMediator.Validation.Tests.Mocks;

internal sealed record EnumerableItem([StringLength(10)] string String);

internal sealed record EnumerablePropertyValidationRequest([Validate] List<EnumerableItem> Items) : IResponseRequest<IRequestResponse>;

internal sealed class EnumerablePropertyValidationRequestHandler : IRequestHandler<EnumerablePropertyValidationRequest, IRequestResponse>
{
    public async Task<IRequestResponse> HandleAsync(EnumerablePropertyValidationRequest request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return new RequestResponse(RequestResponseStatus.Accepted_202);
    }
}
