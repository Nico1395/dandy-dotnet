using System.Diagnostics.CodeAnalysis;
using DandyMediator.Configuration;
using DandyMediator.Responses;

namespace DandyMediator.Validation;

/// <summary>
/// Contains extension methods related to <see cref="IResponseValidationResult"/>.
/// </summary>
public static class ResponseValidationResultExtensions
{
    /// <summary>
    /// Gets the validation result from the response metadata.
    /// </summary>
    /// <param name="response">The response, the validation result is fetched from.</param>
    /// <returns>The fetched validation result if found.</returns>
    public static IResponseValidationResult? GetValidationResult(this IRequestResponse response)
    {
        return response.GetMetadataValueOrDefault(DandyMediatorConstants.Plugins.Validation.RequestMetadataKey) as IResponseValidationResult;
    }

    /// <summary>
    /// Tries to get the validation result from the response metadata.
    /// </summary>
    /// <param name="response">The response, the validation result is fetched from.</param>
    /// <param name="result">The fetched validation result if found.</param>
    /// <returns><see langword="true"/> if a validation result was fetched, <see langword="false"/> if not.</returns>
    public static bool TryGetValidationResult(this IRequestResponse response, [MaybeNullWhen(false)] out IResponseValidationResult? result)
    {
        return (result = response.GetValidationResult()) != null;
    }
}
