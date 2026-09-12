using DandyMediator.Responses;

namespace DandyMediator.Queries;

/// <summary>
/// Response returned by a query containing data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of response data.</typeparam>
public interface IQueryResponse<TData> : IRequestResponse<TData>
{
}
