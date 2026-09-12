using RabbitMQ.Client;

namespace DandyRabbitMQ.Core.Declarations;

/// <summary>
/// Declares configured exchanges, queues, and bindings.
/// </summary>
public interface IDeclarer
{
    /// <summary>
    /// Declares the queue and its associated RabbitMQ topology.
    /// </summary>
    /// <param name="queueName">The configured queue name.</param>
    /// <param name="channel">The channel to use, or <see langword="null"/> to create one.</param>
    /// <param name="cancellationToken">The token used to cancel declaration.</param>
    /// <returns>A task representing the asynchronous declaration.</returns>
    Task DeclareQueueAsync(string queueName, IChannel? channel, CancellationToken cancellationToken);
}
