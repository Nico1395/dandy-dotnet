namespace DandyMediator.Queries;

/// <summary>
/// Contains extension methods for sending queries.
/// </summary>
public static class QueryRequestSenderExtensions
{
    /// <summary>
    /// Sends a query.
    /// </summary>
    /// <typeparam name="TData">Type of query result data.</typeparam>
    /// <param name="sender">Request sender.</param>
    /// <param name="query">Query being sent.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The query response.</returns>
    public static Task<IQueryResponse<TData>> SendAsync<TData>(this IRequestSender sender, IQuery<TData> query, CancellationToken cancellationToken = default)
    {
        return sender.SendAsync(query, cancellationToken);
    }
}
