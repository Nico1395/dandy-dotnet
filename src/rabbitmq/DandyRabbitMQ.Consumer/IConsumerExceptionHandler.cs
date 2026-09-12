namespace DandyRabbitMQ.Consumer;

/// <summary>
/// Handles exceptions raised while consuming messages of the specified type.
/// </summary>
/// <typeparam name="TMessage">The message type.</typeparam>
public interface IConsumerExceptionHandler<in TMessage>
{
    /// <summary>
    /// Handles a consumer <paramref name="exception"/>.
    /// </summary>
    /// <param name="message">The message being consumed.</param>
    /// <param name="context">The delivery context.</param>
    /// <param name="exception">The exception raised during consumption.</param>
    /// <param name="cancellationToken">The token used to cancel exception handling.</param>
    /// <returns>A task representing the asynchronous handling operation.</returns>
    Task HandleAsync(TMessage message, ConsumerContext context, Exception exception, CancellationToken cancellationToken);
}
