namespace DandyMediator;

/// <summary>
/// Service handles requests of type <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">Type of request being handled.</typeparam>
public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
{
    /// <summary>
    /// Handles the given <paramref name="request"/>.
    /// </summary>
    /// <param name="request">Request being handled.</param>
    /// <param name="cancellationToken">Cancellation token for aborting asynchronous operations.</param>
    Task HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Service handles requests of type <typeparamref name="TRequest"/> and generates a response of type <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">Type of request being handled.</typeparam>
/// <typeparam name="TResponse">Type of response being generated.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the given <paramref name="request"/>.
    /// </summary>
    /// <param name="request">Request being handled.</param>
    /// <param name="cancellationToken">Cancellation token for aborting asynchronous operations.</param>
    /// <returns>Generated response.</returns>
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}
