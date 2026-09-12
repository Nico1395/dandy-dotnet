namespace DandyRabbitMQ.Consumer;

/// <summary>
/// Consumes messages of the specified type.
/// </summary>
/// <typeparam name="TMessage">The message type.</typeparam>
public interface IConsumer<in TMessage>
{
    /// <summary>
    /// Processes a <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The message to consume.</param>
    /// <param name="context">The delivery context.</param>
    /// <param name="cancellationToken">The token used to cancel processing.</param>
    /// <returns>The acknowledgement result.</returns>
    Task<ConsumerResult> ConsumeAsync(TMessage message, ConsumerContext context, CancellationToken cancellationToken);
}
