namespace DandyMediator;

/// <summary>
/// Markup interface for a request.
/// </summary>
public interface IRequest
{
}

/// <summary>
/// Markup interface for a request with a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">Type of response expected to be returned by the request.</typeparam>
public interface IRequest<TResponse>
{
}
