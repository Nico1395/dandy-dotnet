namespace DandyMediator.Queries;

/// <summary>
/// Builds query responses containing data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of response data.</typeparam>
public interface IQueryResponseBuilder<TData>
{
    /// <summary>
    /// Adds metadata to the response.
    /// </summary>
    /// <param name="key">Metadata key.</param>
    /// <param name="value">Metadata value.</param>
    /// <returns>This builder.</returns>
    IQueryResponseBuilder<TData> WithMetadata(string key, object value);
    /// <summary>
    /// Builds the response.
    /// </summary>
    /// <returns>The query response.</returns>
    IQueryResponse<TData> Build();
}
