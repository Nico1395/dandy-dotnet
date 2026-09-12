using System.Net;

namespace DandyMediator.Responses;

/// <summary>
/// HTTP-like status codes used by mediator responses.
/// </summary>
public enum RequestResponseStatus
{
    /// <summary>
    /// Request succeeded.
    /// </summary>
    OK_200 = HttpStatusCode.OK,
    
    /// <summary>
    /// Resource was created.
    /// </summary>
    Created_201 = HttpStatusCode.Created,
    
    /// <summary>
    /// Request was accepted for processing.
    /// </summary>
    Accepted_202 = HttpStatusCode.Accepted,
    
    /// <summary>
    /// Request succeeded without a response body.
    /// </summary>
    NoContent_204 = HttpStatusCode.NoContent,
    
    /// <summary>
    /// Request was invalid.
    /// </summary>
    BadRequest_400 = HttpStatusCode.BadRequest,
    
    /// <summary>
    /// Authentication is required.
    /// </summary>
    Unauthorized_401 = HttpStatusCode.Unauthorized,
    
    /// <summary>
    /// Request is not permitted.
    /// </summary>
    Forbidden_403 = HttpStatusCode.Forbidden,
    
    /// <summary>
    /// Requested resource was not found.
    /// </summary>
    NotFound_404 = HttpStatusCode.NotFound,
    
    /// <summary>
    /// Request is not acceptable.
    /// </summary>
    NotAcceptable_406 = HttpStatusCode.NotAcceptable,
    
    /// <summary>
    /// Request conflicts with the current state.
    /// </summary>
    Conflict_409 = HttpStatusCode.Conflict,
    
    /// <summary>
    /// Request failed validation.
    /// </summary>
    UnprocessableEntity_422 = HttpStatusCode.UnprocessableEntity,
    
    /// <summary>
    /// Unexpected server error occurred.
    /// </summary>
    InternalServerError_500 = HttpStatusCode.InternalServerError,
    
    /// <summary>
    /// Requested operation is not implemented.
    /// </summary>
    NotImplemented_501 = HttpStatusCode.NotImplemented,
    
    /// <summary>
    /// Service is temporarily unavailable.
    /// </summary>
    ServiceUnavailable_503 = HttpStatusCode.ServiceUnavailable,
}
