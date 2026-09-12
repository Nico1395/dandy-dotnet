namespace DandyMediator;

/// <summary>
/// Service for handling exceptions thrown while handling a request of type <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">Type of notification the exception has been thrown for.</typeparam>
public interface IRequestExceptionHandler<in TRequest>
    where TRequest : IRequest
{
    /// <summary>
    /// Handles the given <paramref name="exception"/> thrown in a handler or in middleware for the <paramref name="request"/>.
    /// </summary>
    /// <param name="request">Notification the exception has been thrown for.</param>
    /// <param name="exception">Thrown exception.</param>
    /// <param name="cancellationToken">Cancellation token for aborting asynchronous operations.</param>
    Task HandleAsync(TRequest request, Exception exception, CancellationToken cancellationToken);
}

/// <summary>
/// Service for handling exceptions thrown while handling a request of type <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">Type of notification the exception has been thrown for.</typeparam>
/// <typeparam name="TResponse">Type of response expected from the <typeparamref name="TRequest"/>.</typeparam>
public interface IRequestExceptionHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the given <paramref name="exception"/> thrown in a handler or in middleware for the <paramref name="request"/>.
    /// </summary>
    /// <param name="request">Notification the exception has been thrown for.</param>
    /// <param name="exception">Thrown exception.</param>
    /// <param name="cancellationToken">Cancellation token for aborting asynchronous operations.</param>
    Task HandleAsync(TRequest request, Exception exception, CancellationToken cancellationToken);
}
