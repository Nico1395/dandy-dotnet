using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests.Factories;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests.Mapping;
using DandyDotnet.Patterns.Mediator.Configuration;

namespace DandyDotnet.Patterns.Mediator.Validation;

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
            [MediatorConstants.Plugins.Validation.RequestMetadataKey] = validationResult,
        };

        return requestResponseFactory.CreateAndCast<TResponse>(
            requestResponseMapper.GetImplementationTypeFor(typeof(TResponse)),
            args: [RequestResponseStatus.UnprocessableEntity_422, metadata]);
    }
}
