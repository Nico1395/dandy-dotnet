using DandyMediator.Responses;

namespace DandyMediator.Queries;

/// <summary>
/// Factory methods for query responses.
/// </summary>
public static class QueryResponse
{
    /// <summary>
    /// Creates an OK response containing data.
    /// </summary>
    /// <typeparam name="TData">Type of response data.</typeparam>
    /// <param name="data">Response data.</param>
    /// <returns>A response builder.</returns>
    public static IQueryResponseBuilder<TData> OK_200<TData>(TData data) => new QueryResponseBuilder<TData>(RequestResponseStatus.OK_200, data);
    
    /// <summary>
    /// Creates a Created response.
    /// </summary>
    /// <typeparam name="TData">Type of response data.</typeparam>
    /// <returns>A response builder.</returns>
    public static IQueryResponseBuilder<TData> Created_201<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.Created_201);
    
    /// <summary>
    /// Creates an Accepted response.
    /// </summary>
    /// <typeparam name="TData">Type of response data.</typeparam>
    /// <returns>A response builder.</returns>
    public static IQueryResponseBuilder<TData> Accepted_202<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.Accepted_202);
    
    /// <summary>
    /// Creates a No Content response.
    /// </summary>
    /// <typeparam name="TData">Type of response data.</typeparam>
    /// <returns>A response builder.</returns>
    public static IQueryResponseBuilder<TData> NoContent_204<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.NoContent_204);
    
    /// <summary>
    /// Creates a Bad Request response.
    /// </summary>
    public static IQueryResponseBuilder<TData> BadRequest_400<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.BadRequest_400);
    
    /// <summary>
    /// Creates an Unauthorized response.
    /// </summary>
    public static IQueryResponseBuilder<TData> Unauthorized_401<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.Unauthorized_401);
    
    /// <summary>
    /// Creates a Forbidden response.
    /// </summary>
    public static IQueryResponseBuilder<TData> Forbidden_403<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.Forbidden_403);
    
    /// <summary>
    /// Creates a Not Found response.
    /// </summary>
    public static IQueryResponseBuilder<TData> NotFound_404<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.NotFound_404);
    
    /// <summary>
    /// Creates a Not Acceptable response.
    /// </summary>
    public static IQueryResponseBuilder<TData> NotAcceptable_406<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.NotAcceptable_406);
    
    /// <summary>
    /// Creates a Conflict response.
    /// </summary>
    public static IQueryResponseBuilder<TData> Conflict_409<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.Conflict_409);
    
    /// <summary>
    /// Creates an Unprocessable Entity response.
    /// </summary>
    public static IQueryResponseBuilder<TData> UnprocessableEntity_422<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.UnprocessableEntity_422);
    
    /// <summary>
    /// Creates an Internal Server Error response.
    /// </summary>
    public static IQueryResponseBuilder<TData> InternalServerError_500<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.InternalServerError_500);
    
    /// <summary>
    /// Creates a Not Implemented response.
    /// </summary>
    public static IQueryResponseBuilder<TData> NotImplemented_501<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.NotImplemented_501);
    
    /// <summary>
    /// Creates a Service Unavailable response.
    /// </summary>
    public static IQueryResponseBuilder<TData> ServiceUnavailable_503<TData>() => new QueryResponseBuilder<TData>(RequestResponseStatus.ServiceUnavailable_503);

    /// <summary>
    /// Creates a <see cref="RequestResponseStatus.OK_200"/> response when data exists, otherwise <see cref="RequestResponseStatus.NotFound_404"/>.
    /// </summary>
    /// <typeparam name="TData">Type of response data.</typeparam>
    /// <param name="data">Response data.</param>
    /// <returns>A response builder.</returns>
    public static IQueryResponseBuilder<TData> OkOrNotFound<TData>(TData? data)
    {
        if (data == null)
            return NotFound_404<TData>();

        return OK_200(data);
    }

    /// <summary>
    /// Creates a <see cref="RequestResponseStatus.OK_200"/> response when data exists, otherwise <see cref="RequestResponseStatus.NotFound_404"/>.
    /// </summary>
    /// <typeparam name="TData">Type of response data.</typeparam>
    /// <param name="data">Response data.</param>
    /// <returns>A query response.</returns>
    public static IQueryResponse<TData> FromData<TData>(TData? data)
    {
        return (data == null
            ? NotFound_404<TData>()
            : OK_200(data)).Build();
    }

    /// <summary>
    /// Converts data to a <see cref="RequestResponseStatus.OK_200"/> response when data exists, otherwise <see cref="RequestResponseStatus.NotFound_404"/>.
    /// </summary>
    /// <typeparam name="TData">Type of response data.</typeparam>
    /// <param name="data">Response data.</param>
    /// <returns>A query response.</returns>
    public static IQueryResponse<TData> ToResponse<TData>(this TData? data)
    {
        return (data == null
            ? NotFound_404<TData>()
            : OK_200(data)).Build();
    }
}

/// <summary>
/// Concrete query response containing data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of response data.</typeparam>
public sealed class QueryResponse<TData> : RequestResponse<TData>, IQueryResponse<TData>
{
    /// <summary>
    /// Creates a query response.
    /// </summary>
    /// <param name="status">Response status.</param>
    public QueryResponse(RequestResponseStatus status)
        : base(status)
    {
    }

    /// <summary>
    /// Creates a query response.
    /// </summary>
    /// <param name="status">Response status.</param>
    /// <param name="metadata">Response metadata.</param>
    /// <param name="data">Response data.</param>
    public QueryResponse(RequestResponseStatus status, IReadOnlyDictionary<string, object>? metadata, TData data)
        : base(status, metadata, data)
    {
    }
}
