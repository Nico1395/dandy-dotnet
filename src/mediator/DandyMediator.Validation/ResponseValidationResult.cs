namespace DandyMediator.Validation;

/// <inheritdoc/>
public sealed class ResponseValidationResult : IResponseValidationResult
{
    /// <summary>
    /// Creates a new instance of <see cref="ResponseValidationResult"/>.
    /// </summary>
    /// <param name="title">Title of the validation result.</param>
    /// <param name="errors">Errors of the validation result.</param>
    public ResponseValidationResult(string title, Dictionary<string, List<string>> errors)
    {
        Title = title;
        Errors = errors.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.ToList() as IReadOnlyList<string>);
    }

    /// <inheritdoc/>
    public string Title { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, IReadOnlyList<string>> Errors { get; }
}
