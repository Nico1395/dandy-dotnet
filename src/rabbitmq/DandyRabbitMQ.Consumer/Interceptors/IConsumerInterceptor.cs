namespace DandyRabbitMQ.Consumer.Interceptors;

/// <summary>
/// Observes acknowledgement operations performed for consumed messages.
/// </summary>
public interface IConsumerInterceptor
{
    /// <summary>
    /// Runs after a <paramref name="message"/> is acknowledged.
    /// </summary>
    /// <param name="message">The consumed message.</param>
    /// <param name="context">The delivery context.</param>
    /// <param name="result">The acknowledgement result.</param>
    /// <param name="cancellationToken">The token used to cancel interception.</param>
    /// <returns>A task representing the asynchronous interception.</returns>
    Task OnAfterAckAsync(object message, ConsumerContext context, ConsumerResult result, CancellationToken cancellationToken);

    /// <summary>
    /// Runs after a <paramref name="message"/> is negatively acknowledged.
    /// </summary>
    /// <param name="message">The consumed message.</param>
    /// <param name="context">The delivery context.</param>
    /// <param name="result">The acknowledgement result.</param>
    /// <param name="cancellationToken">The token used to cancel interception.</param>
    /// <returns>A task representing the asynchronous interception.</returns>
    Task OnAfterNackAsync(object message, ConsumerContext context, ConsumerResult result, CancellationToken cancellationToken);
}
