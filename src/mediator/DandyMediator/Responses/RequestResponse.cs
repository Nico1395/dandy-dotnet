namespace DandyMediator.Responses;

/// <summary>
/// Concrete response without data.
/// </summary>
public class RequestResponse : IRequestResponse
{
    /// <summary>
    /// Creates a response.
    /// </summary>
    /// <param name="status">Response status.</param>
    public RequestResponse(RequestResponseStatus status)
    {
        Status = status;
    }

    /// <summary>
    /// Creates a response.
    /// </summary>
    /// <param name="status">Response status.</param>
    /// <param name="metadata">Response metadata.</param>
    public RequestResponse(RequestResponseStatus status, IReadOnlyDictionary<string, object>? metadata)
    {
        Status = status;
        Metadata = metadata ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Response status.
    /// </summary>
    public RequestResponseStatus Status { get; }
    /// <summary>
    /// Response metadata.
    /// </summary>
    public IReadOnlyDictionary<string, object> Metadata { get; init; } = new Dictionary<string, object>();
}

/// <summary>
/// Concrete response containing data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of response data.</typeparam>
public class RequestResponse<TData> : RequestResponse, IRequestResponse<TData>
{
    /// <summary>
    /// Creates a response.
    /// </summary>
    /// <param name="status">Response status.</param>
    public RequestResponse(RequestResponseStatus status)
        : base(status)
    {
    }

    /// <summary>
    /// Creates a response.
    /// </summary>
    /// <param name="status">Response status.</param>
    /// <param name="metadata">Response metadata.</param>
    public RequestResponse(RequestResponseStatus status, IReadOnlyDictionary<string, object>? metadata)
        : base(status, metadata)
    {
    }

    /// <summary>
    /// Creates a response.
    /// </summary>
    /// <param name="status">Response status.</param>
    /// <param name="metadata">Response metadata.</param>
    /// <param name="data">Response data.</param>
    public RequestResponse(RequestResponseStatus status, IReadOnlyDictionary<string, object>? metadata, TData? data)
        : base(status, metadata)
    {
        Data = data;
    }

    /// <summary>
    /// Response data.
    /// </summary>
    public TData? Data { get; init; }
}
