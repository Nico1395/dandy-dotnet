using DandyMediator.Responses;

namespace DandyMediator.Queries;

/// <summary>
/// Contains extension methods for query responses.
/// </summary>
public static class QueryResponseExtensions
{
    /// <summary>
    /// Maps query response data to another type.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Only maps if the response status is <see cref="RequestResponseStatus.OK_200"/> and the data is not <see langword="null"/>.
    ///     </para>
    ///     <para>
    ///         If the response status is <see cref="RequestResponseStatus.OK_200"/> and the data is <see langword="null"/>, the status
    ///         is automatically set to <see cref="RequestResponseStatus.NoContent_204"/>.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TSource">Source data type.</typeparam>
    /// <typeparam name="TDestination">Destination data type.</typeparam>
    /// <param name="response">Response to map.</param>
    /// <param name="map">Mapping function.</param>
    /// <returns>A response containing mapped data.</returns>
    public static IQueryResponse<TDestination> Map<TSource, TDestination>(this IQueryResponse<TSource> response, Func<TSource, TDestination> map)
    {
        var status = response.Status;
        TDestination? data = default;

        // Only map the data if it's code 200 and data is actually present
        if (response.IsOK_200() && response.Data != null)
            data = map(response.Data);
        else if (response.IsOK_200() && response.Data == null)
            status = RequestResponseStatus.NoContent_204;

        return new QueryResponse<TDestination>(status)
        {
            Data = data,
            Metadata = response.Metadata,
        };
    }
}
