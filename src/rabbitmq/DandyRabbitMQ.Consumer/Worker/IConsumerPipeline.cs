namespace DandyRabbitMQ.Consumer.Worker;

/// <summary>
/// Executes consumers and their middleware pipeline.
/// </summary>
public interface IConsumerPipeline
{
    /// <summary>
    /// Executes the pipeline for a <paramref name="message"/>.
    /// </summary>
    /// <typeparam name="TMessage">The message type.</typeparam>
    /// <param name="message">The message to process.</param>
    /// <param name="context">The delivery context.</param>
    /// <param name="cancellationToken">The token used to cancel processing.</param>
    /// <returns>The acknowledgement result.</returns>
    Task<ConsumerResult> ExecuteAsync<TMessage>(TMessage message, ConsumerContext context, CancellationToken cancellationToken);
}
