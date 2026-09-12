using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace DandyMediator.Responses;

/// <summary>
/// Contains extensions for inspecting and converting responses.
/// </summary>
public static class RequestResponseExtensions
{
    /// <summary>
    /// Checks whether the response has a success status.
    /// </summary>
    public static bool IsSuccess_2xx(this IRequestResponse response) => response.Status <= RequestResponseStatus.NoContent_204;
    
    /// <summary>
    /// Checks whether the response has a client error status.
    /// </summary>
    public static bool IsClientSide_4xx(this IRequestResponse response) => response.Status is >= RequestResponseStatus.BadRequest_400 and < RequestResponseStatus.InternalServerError_500;
    
    /// <summary>
    /// Checks whether the response has a server error status.
    /// </summary>
    public static bool IsServerSide_5xx(this IRequestResponse response) => response.Status >= RequestResponseStatus.InternalServerError_500;
    
    /// <summary>
    /// Checks whether the response has the specified status.
    /// </summary>
    public static bool IsStatus(this IRequestResponse response, RequestResponseStatus status) => response.Status == status;
    
    /// <summary>
    /// Checks for OK.
    /// </summary>
    public static bool IsOK_200(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.OK_200);
    
    /// <summary>
    /// Checks for Created.
    /// </summary>
    public static bool IsCreated_201(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.Created_201);
    
    /// <summary>
    /// Checks for Accepted.
    /// </summary>
    public static bool IsAccepted_202(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.Accepted_202);
    
    /// <summary>
    /// Checks for No Content.
    /// </summary>
    public static bool IsNoContent_204(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.NoContent_204);
    
    /// <summary>
    /// Checks for Bad Request.
    /// </summary>
    public static bool IsBadRequest_400(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.BadRequest_400);
    
    /// <summary>
    /// Checks for Unauthorized.
    /// </summary>
    public static bool IsUnauthorized_401(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.Unauthorized_401);
    
    /// <summary>
    /// Checks for Forbidden.
    /// </summary>
    public static bool IsForbidden_403(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.Forbidden_403);
    
    /// <summary>
    /// Checks for Not Found.
    /// </summary>
    public static bool IsNotFound_404(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.NotFound_404);
    
    /// <summary>
    /// Checks for Not Acceptable.
    /// </summary>
    public static bool IsNotAcceptable_406(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.NotAcceptable_406);
    
    /// <summary>
    /// Checks for Conflict.
    /// </summary>
    public static bool IsConflict_409(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.Conflict_409);
    
    /// <summary>
    /// Checks for Unprocessable Entity.
    /// </summary>
    public static bool IsUnprocessableEntity_422(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.UnprocessableEntity_422);
    
    /// <summary>
    /// Checks for Internal Server Error.
    /// </summary>
    public static bool IsInternalServerError_500(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.InternalServerError_500);
    
    /// <summary>
    /// Checks for Not Implemented.
    /// </summary>
    public static bool IsNotImplemented_501(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.NotImplemented_501);
    
    /// <summary>
    /// Checks for Service Unavailable.
    /// </summary>
    public static bool IsServiceUnavailable_503(this IRequestResponse response) => response.IsStatus(RequestResponseStatus.ServiceUnavailable_503);

    /// <summary>
    /// Checks whether a boolean response contains the expected value.
    /// </summary>
    public static bool ResultedIn(this IRequestResponse<bool> response, bool expected)
    {
        return response.IsSuccess_2xx() && response.Data == expected;
    }

    /// <summary>
    /// Checks whether a boolean response is true.
    /// </summary>
    public static bool ResultedInTrue(this IRequestResponse<bool> response)
    {
        return response.ResultedIn(true);
    }

    /// <summary>
    /// Checks whether a boolean response is false.
    /// </summary>
    public static bool ResultedInFalse(this IRequestResponse<bool> response)
    {
        return response.ResultedIn(false);
    }

    /// <summary>
    /// Tries to get response metadata.
    /// </summary>
    public static bool TryGetMetadata(this IRequestResponse response, string key, [MaybeNullWhen(false)] out object? value)
    {
        return response.Metadata.TryGetValue(key, out value);
    }

    /// <summary>
    /// Checks whether response metadata contains a key.
    /// </summary>
    public static bool HasMetadataKey(this IRequestResponse response, string key)
    {
        return response.Metadata.ContainsKey(key);
    }

    /// <summary>
    /// Gets a metadata value.
    /// </summary>
    public static object GetMetadataValue(this IRequestResponse response, string key)
    {
        return response.Metadata[key];
    }

    /// <summary>
    /// Gets a metadata value or <see langword="null"/>.
    /// </summary>
    public static object? GetMetadataValueOrDefault(this IRequestResponse response, string key)
    {
        return response.Metadata.GetValueOrDefault(key);
    }

    /// <summary>
    /// Gets a metadata value or a default value.
    /// </summary>
    public static object? GetMetadataValueOrDefault(this IRequestResponse response, string key, object? defaultValue)
    {
        return response.GetMetadataValueOrDefault(key) ?? defaultValue;
    }

    /// <summary>
    /// Checks whether the request was valid.
    /// </summary>
    public static bool WasValidRequest(this IRequestResponse response)
    {
        return !response.IsUnprocessableEntity_422();
    }

    /// <summary>
    /// Checks whether the request was invalid.
    /// </summary>
    public static bool WasInvalidRequest(this IRequestResponse response)
    {
        return response.IsUnprocessableEntity_422();
    }

    /// <summary>
    /// Converts a response to an ASP.NET Core result.
    /// </summary>
    public static IResult ToResult(this IRequestResponse response)
    {
        var statusCode = (int)(HttpStatusCode)response.Status;
        if (statusCode >= 400)
            return CreateProblemResult(response, statusCode);

        if (response.Status == RequestResponseStatus.NoContent_204)
            return Results.NoContent();

        return Results.StatusCode(statusCode);
    }

    /// <summary>
    /// Converts a data response to an ASP.NET Core result.
    /// </summary>
    public static IResult ToResult<T>(this IRequestResponse<T> response)
    {
        var statusCode = (int)(HttpStatusCode)response.Status;
        if (statusCode >= 400)
            return CreateProblemResult(response, statusCode);

        return response.Status switch
        {
            RequestResponseStatus.OK_200 => Results.Ok(response.Data),
            RequestResponseStatus.Created_201 => Results.Created(GetLocation(response), response.Data),
            RequestResponseStatus.Accepted_202 => Results.Accepted(GetLocation(response), response.Data),
            RequestResponseStatus.NoContent_204 => Results.NoContent(),
            _ => Results.Json(response.Data, statusCode: statusCode)
        };
    }

    private static IResult CreateProblemResult(IRequestResponse response, int statusCode)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = response.Metadata.TryGetValue("title", out var title) ? title.ToString() : ReasonPhrases.GetReasonPhrase(statusCode),
            Detail = response.Metadata.TryGetValue("detail", out var detail) ? detail.ToString() : null
        };

        foreach (var kvp in response.Metadata)
        {
            if (kvp.Key is "title" or "detail")
                continue;

            problemDetails.Extensions[kvp.Key] = kvp.Value;
        }

        return Results.Problem(problemDetails);
    }

    private static string? GetLocation(IRequestResponse response)
    {
        return response.Metadata.TryGetValue("location", out var location) ? location.ToString() : null;
    }
}
