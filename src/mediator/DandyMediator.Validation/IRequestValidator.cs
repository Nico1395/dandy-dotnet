using DandyMediator.Responses;

namespace DandyMediator.Validation;

/// <summary>
/// Service validates requests implementing <see cref="IResponseRequest{TResponse}"/>.
/// </summary>
public interface IRequestValidator
{
    /// <summary>
    /// Validates the given <paramref name="request"/>.
    /// </summary>
    /// <param name="request">Request to be validated.</param>
    /// <returns>The validation result or <see langword="null"/> when no validation could be determined.</returns>
    IResponseValidationResult? Validate(object request);
}
