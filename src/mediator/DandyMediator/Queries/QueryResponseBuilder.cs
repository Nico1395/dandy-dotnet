using DandyMediator.Responses;

namespace DandyMediator.Queries;

internal sealed class QueryResponseBuilder<TData>(RequestResponseStatus status, TData? data = default) : IQueryResponseBuilder<TData>
{
    private readonly Dictionary<string, object> _metadata = [];

    public IQueryResponseBuilder<TData> WithMetadata(string key, object value)
    {
        _metadata[key] = value;
        return this;
    }
    
    public IQueryResponse<TData> Build()
    {
        return new QueryResponse<TData>(status)
        {
            Data = data,
            Metadata = _metadata,
        };
    }
}
