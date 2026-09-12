using RabbitMQ.Client;

namespace DandyRabbitMQ.Producer;

/// <summary>
/// Publishes messages to RabbitMQ.
/// </summary>
public interface IProducer
{
    /// <summary>
    /// Publishes a <paramref name="message"/> to the specified <paramref name="exchange"/> and routing keys.
    /// </summary>
    /// <param name="exchange">The target exchange, or <see langword="null"/> to resolve it from message metadata.</param>
    /// <param name="routingKeys">The target routing keys, or <see langword="null"/> to resolve them from message metadata.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties, or <see langword="null"/> to use default properties.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    Task ProduceAsync(string? exchange, IEnumerable<string>? routingKeys, object message, BasicProperties? properties, CancellationToken cancellationToken);
}
