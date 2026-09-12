using DandyMediator.Responses;

namespace DandyMediator.Validation;

/// <summary>
/// Validation result of an <see cref="IRequestResponse"/>.
/// </summary>
public interface IResponseValidationResult
{
    /// <summary>
    /// Title of the validation result.
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Errors of the validation result.
    /// </summary>
    IReadOnlyDictionary<string, IReadOnlyList<string>> Errors { get; }
}
