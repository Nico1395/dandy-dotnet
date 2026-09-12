namespace DandyMediator.Commands;

/// <summary>
/// Builds command responses without data.
/// </summary>
public interface ICommandResponseBuilder
{
    /// <summary>
    /// Adds metadata to the response.
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    /// <returns>This builder.</returns>
    ICommandResponseBuilder WithMetadata(string key, object value);
    /// <summary>
    /// Builds the response.
    /// </summary>
    /// <returns>The command response.</returns>
    ICommandResponse Build();
}

/// <summary>
/// Builds command responses containing data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of response data.</typeparam>
public interface ICommandResponseBuilder<TData>
{
    /// <summary>
    /// Adds metadata to the response.
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    /// <returns>This builder.</returns>
    ICommandResponseBuilder<TData> WithMetadata(string key, object value);
    /// <summary>
    /// Builds the response.
    /// </summary>
    /// <returns>The command response.</returns>
    ICommandResponse<TData> Build();
}
