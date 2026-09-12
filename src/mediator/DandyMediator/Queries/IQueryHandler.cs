namespace DandyMediator.Queries;

/// <summary>
/// Handles queries of type <typeparamref name="TQuery"/>.
/// </summary>
/// <typeparam name="TQuery">Type of query being handled.</typeparam>
/// <typeparam name="TData">Type of query result data.</typeparam>
public interface IQueryHandler<TQuery, TData> : IRequestHandler<TQuery, IQueryResponse<TData>>
    where TQuery : IQuery<TData>
{
}
