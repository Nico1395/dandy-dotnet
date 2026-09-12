using DandyMediator.Responses;

namespace DandyMediator.Commands;

/// <summary>
/// Concrete command response without data.
/// </summary>
public sealed class CommandResponse : RequestResponse, ICommandResponse
{
    /// <summary>
    /// Creates a command response.
    /// </summary>
    /// <param name="status">Response status.</param>
    public CommandResponse(RequestResponseStatus status)
        : base(status)
    {
    }

    /// <summary>
    /// Creates a command response.
    /// </summary>
    /// <param name="status">Response status.</param>
    /// <param name="metadata">Response metadata.</param>
    public CommandResponse(RequestResponseStatus status, IReadOnlyDictionary<string, object>? metadata)
        : base(status, metadata)
    {
    }
    
    /// <summary>
    /// Creates an OK response builder.
    /// </summary>
    public static ICommandResponseBuilder OK_200() => new CommandResponseBuilder(RequestResponseStatus.OK_200);
    
    /// <summary>
    /// Creates a Created response builder.
    /// </summary>
    public static ICommandResponseBuilder Created_201() => new CommandResponseBuilder(RequestResponseStatus.Created_201);
    
    /// <summary>
    /// Creates an Accepted response builder.
    /// </summary>
    public static ICommandResponseBuilder Accepted_202() => new CommandResponseBuilder(RequestResponseStatus.Accepted_202);
    
    /// <summary>
    /// Creates a No Content response builder.
    /// </summary>
    public static ICommandResponseBuilder NoContent_204() => new CommandResponseBuilder(RequestResponseStatus.NoContent_204);
    
    /// <summary>
    /// Creates a Bad Request response builder.
    /// </summary>
    public static ICommandResponseBuilder BadRequest_400() => new CommandResponseBuilder(RequestResponseStatus.BadRequest_400);
    
    /// <summary>
    /// Creates an Unauthorized response builder.
    /// </summary>
    public static ICommandResponseBuilder Unauthorized_401() => new CommandResponseBuilder(RequestResponseStatus.Unauthorized_401);
    
    /// <summary>
    /// Creates a Forbidden response builder.
    /// </summary>
    public static ICommandResponseBuilder Forbidden_403() => new CommandResponseBuilder(RequestResponseStatus.Forbidden_403);
    
    /// <summary>
    /// Creates a Not Found response builder.
    /// </summary>
    public static ICommandResponseBuilder NotFound_404() => new CommandResponseBuilder(RequestResponseStatus.NotFound_404);
    
    /// <summary>
    /// Creates a Not Acceptable response builder.
    /// </summary>
    public static ICommandResponseBuilder NotAcceptable_406() => new CommandResponseBuilder(RequestResponseStatus.NotAcceptable_406);
    
    /// <summary>
    /// Creates a Conflict response builder.
    /// </summary>
    public static ICommandResponseBuilder Conflict_409() => new CommandResponseBuilder(RequestResponseStatus.Conflict_409);
    
    /// <summary>
    /// Creates an Unprocessable Entity response builder.
    /// </summary>
    public static ICommandResponseBuilder UnprocessableEntity_422() => new CommandResponseBuilder(RequestResponseStatus.UnprocessableEntity_422);
    
    /// <summary>
    /// Creates an Internal Server Error response builder.
    /// </summary>
    public static ICommandResponseBuilder InternalServerError_500() => new CommandResponseBuilder(RequestResponseStatus.InternalServerError_500);
    
    /// <summary>
    /// Creates a Not Implemented response builder.
    /// </summary>
    public static ICommandResponseBuilder NotImplemented_501() => new CommandResponseBuilder(RequestResponseStatus.NotImplemented_501);
    
    /// <summary>
    /// Creates a Service Unavailable response builder.
    /// </summary>
    public static ICommandResponseBuilder ServiceUnavailable_503() => new CommandResponseBuilder(RequestResponseStatus.ServiceUnavailable_503);
    

    /// <summary>
    /// Creates an OK response builder with data.
    /// </summary>
    public static ICommandResponseBuilder<TData> OK_200<TData>(TData? data) => new CommandResponseBuilder<TData>(RequestResponseStatus.OK_200, data);
    
    /// <summary>
    /// Creates an OK response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> OK_200<TData>() => OK_200<TData>(data: default);
    
    /// <summary>
    /// Creates a Created response builder with data.
    /// </summary>
    public static ICommandResponseBuilder<TData> Created_201<TData>(TData? data) => new CommandResponseBuilder<TData>(RequestResponseStatus.Created_201, data);
    
    /// <summary>
    /// Creates a Created response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> Created_201<TData>() => Created_201<TData>(data: default);
    
    /// <summary>
    /// Creates an Accepted response builder with data.
    /// </summary>
    public static ICommandResponseBuilder<TData> Accepted_202<TData>(TData? data) => new CommandResponseBuilder<TData>(RequestResponseStatus.Accepted_202, data);
    
    /// <summary>
    /// Creates an Accepted response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> Accepted_202<TData>() => Accepted_202<TData>(data: default);
    
    /// <summary>
    /// Creates a No Content response builder with data.
    /// </summary>
    public static ICommandResponseBuilder<TData> NoContent_204<TData>(TData? data) => new CommandResponseBuilder<TData>(RequestResponseStatus.NoContent_204, data);
    
    /// <summary>
    /// Creates a No Content response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> NoContent_204<TData>() => NoContent_204<TData>(data: default);
    
    /// <summary>
    /// Creates a Bad Request response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> BadRequest_400<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.BadRequest_400, data: default);
    
    /// <summary>
    /// Creates an Unauthorized response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> Unauthorized_401<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.Unauthorized_401, data: default);
    
    /// <summary>
    /// Creates a Forbidden response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> Forbidden_403<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.Forbidden_403, data: default);
    
    /// <summary>
    /// Creates a Not Found response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> NotFound_404<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.NotFound_404, data: default);
    
    /// <summary>
    /// Creates a Not Acceptable response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> NotAcceptable_406<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.NotAcceptable_406, data: default);
    
    /// <summary>
    /// Creates a Conflict response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> Conflict_409<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.Conflict_409, data: default);
    
    /// <summary>
    /// Creates an Unprocessable Entity response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> UnprocessableEntity_422<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.UnprocessableEntity_422, data: default);
    
    /// <summary>
    /// Creates an Internal Server Error response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> InternalServerError_500<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.InternalServerError_500, data: default);
    
    /// <summary>
    /// Creates a Not Implemented response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> NotImplemented_501<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.NotImplemented_501, data: default);
    
    /// <summary>
    /// Creates a Service Unavailable response builder.
    /// </summary>
    public static ICommandResponseBuilder<TData> ServiceUnavailable_503<TData>() => new CommandResponseBuilder<TData>(RequestResponseStatus.ServiceUnavailable_503, data: default);
}

/// <summary>
/// Concrete command response containing data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of response data.</typeparam>
public sealed class CommandResponse<TData> : RequestResponse<TData>, ICommandResponse<TData>
{
    /// <summary>
    /// Creates a command response.
    /// </summary>
    /// <param name="status">Response status.</param>
    public CommandResponse(RequestResponseStatus status)
        : base(status)
    {
    }

    /// <summary>
    /// Creates a command response.
    /// </summary>
    /// <param name="status">Response status.</param>
    /// <param name="metadata">Response metadata.</param>
    /// <param name="data">Response data.</param>
    public CommandResponse(RequestResponseStatus status, IReadOnlyDictionary<string, object>? metadata, TData data)
        : base(status, metadata, data)
    {
    }
}
