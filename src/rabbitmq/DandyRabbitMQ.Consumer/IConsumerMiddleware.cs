namespace DandyRabbitMQ.Consumer;

/// <summary>
/// Intercepts consumption of messages of the specified type.
/// </summary>
/// <typeparam name="TMessage">The message type.</typeparam>
public interface IConsumerMiddleware<in TMessage>
{
    /// <summary>
    /// Processes a <paramref name="message"/> and optionally invokes the next pipeline step.
    /// </summary>
    /// <param name="message">The message to process.</param>
    /// <param name="context">The delivery context.</param>
    /// <param name="nextStep">The next middleware or consumer step.</param>
    /// <param name="cancellationToken">The token used to cancel processing.</param>
    /// <returns>The acknowledgement result.</returns>
    Task<ConsumerResult> InterceptAsync(TMessage message, ConsumerContext context, ConsumerDelegate nextStep, CancellationToken cancellationToken);
}
