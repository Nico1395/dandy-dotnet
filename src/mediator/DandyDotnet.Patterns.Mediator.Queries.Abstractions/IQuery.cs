using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Queries.Abstractions;

/// <summary>
/// A query that returns data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of query result data.</typeparam>
public interface IQuery<TData> : IResponseRequest<IQueryResponse<TData>>
{
}
