using DandyMediator.Configuration;
using DandyMediator.Responses;

namespace DandyMediator.Validation;

internal sealed class ResponseRequestValidationMiddleware<TRequest, TResponse>(
    IRequestResponseMapper requestResponseMapper,
    IRequestResponseFactory requestResponseFactory,
    IRequestValidator requestValidator) : IRequestMiddleware<TRequest, TResponse>
    where TRequest : IResponseRequest<TResponse>
    where TResponse : IRequestResponse
{
    public async Task<TResponse> InterceptAsync(TRequest request, RequestHandlerDelegate<TResponse> nextStep, CancellationToken cancellationToken)
    {
        var validationResult = requestValidator.Validate(request);
        if (validationResult == null)
            return await nextStep.Invoke();

        var metadata = new Dictionary<string, object>
        {
            [DandyMediatorConstants.Plugins.Validation.RequestMetadataKey] = validationResult,
        };

        return requestResponseFactory.CreateAndCast<TResponse>(
            requestResponseMapper.GetImplementationTypeFor(typeof(TResponse)),
            args: [RequestResponseStatus.UnprocessableEntity_422, metadata]);
    }
}
