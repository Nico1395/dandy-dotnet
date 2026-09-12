namespace DandyMediator;

/// <summary>
/// Delegate for request handlers and request middleware.
/// </summary>
public delegate Task RequestHandlerDelegate();

/// <summary>
/// Delegate for request handlers and request middleware.
/// </summary>
/// <returns>A response of type <typeparamref name="TResponse"/>.</returns>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

/// <summary>
/// Request middleware for requests of type <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">Type of requests being intercepted by the middleware.</typeparam>
public interface IRequestMiddleware<in TRequest>
    where TRequest : IRequest
{
    /// <summary>
    /// Intercepts the given <paramref name="request"/>.
    /// </summary>
    /// <param name="request">Request being intercepted.</param>
    /// <param name="nextStep">Next delegate or step in the request pipeline.</param>
    /// <param name="cancellationToken">Cancellation token for aborting asynchronous operations.</param>
    Task InterceptAsync(TRequest request, RequestHandlerDelegate nextStep, CancellationToken cancellationToken);
}

/// <summary>
/// Request middleware for requests of type <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">Type of requests being intercepted by the middleware.</typeparam>
/// <typeparam name="TResponse">Type of response expected by the request.</typeparam>
public interface IRequestMiddleware<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Intercepts the given <paramref name="request"/>.
    /// </summary>
    /// <param name="request">Request being intercepted.</param>
    /// <param name="nextStep">Next delegate or step in the request pipeline.</param>
    /// <param name="cancellationToken">Cancellation token for aborting asynchronous operations.</param>
    /// <returns>Response expected by the request.</returns>
    Task<TResponse> InterceptAsync(TRequest request, RequestHandlerDelegate<TResponse> nextStep, CancellationToken cancellationToken);
}
