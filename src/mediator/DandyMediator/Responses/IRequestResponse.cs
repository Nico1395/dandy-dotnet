using System.Net;

namespace DandyMediator.Responses;

/// <summary>
/// Response of an <see cref="IResponseRequest{TResponse}"/>.
/// </summary>
/// <remarks>
///     <para>
///         Its completely optional to use this type of response for requests. However, this contains a standardized contract
///         and plenty of utility such as a builder pattern and a plethora of extenions.
///     </para>
/// </remarks>
public interface IRequestResponse
{
    /// <summary>
    /// Response status the request resulted in.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Models most of the HTTP response codes of <see cref="HttpStatusCode"/>.
    ///     </para>
    /// </remarks>
    RequestResponseStatus Status { get; }

    /// <summary>
    /// Dictionary for returning metadata when processing a request.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Should ideally not be used to transport response data (such as an ID generated during the handling of a request or so).
    ///         Use <see cref="IRequestResponse{TData}.Data"/> for such cases instead.
    ///     </para>
    ///     <para>
    ///         Is being used to communicate validation errors by the validation plugin.
    ///     </para>
    /// </remarks>
    IReadOnlyDictionary<string, object> Metadata { get; }
}

/// <summary>
/// Response of an <see cref="IResponseRequest{TResponse}"/> containg additional data of type <typeparamref name="TData"/>.
/// </summary>
/// <typeparam name="TData">Type of response data.</typeparam>
/// <remarks>
///     <para>
///         It's completely optional to use this type of response for requests. However, this contains a standardized contract
///         and plenty of utilities such as a builder pattern and a plethora of extenions.
///     </para>
/// </remarks>
public interface IRequestResponse<out TData> : IRequestResponse
{
    /// <summary>
    /// Data to be returned.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Should not be <see langword="null"/> if the <see cref="IRequestResponse.Status"/> is in the 200s.
    ///     </para>
    /// </remarks>
    TData? Data { get; }
}
