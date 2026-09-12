using DandyRabbitMQ.Core.Messages;
using RabbitMQ.Client;

namespace DandyRabbitMQ.Producer;

/// <summary>
/// Provides convenience overloads for publishing messages with identifiers and timestamps.
/// </summary>
public static class ProducerExtensions
{
    /// <summary>
    /// Publishes a <paramref name="message"/> with a string identifier and <paramref name="timestamp"/>.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKeys">The target routing keys, or <see langword="null"/>.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, IEnumerable<string>? routingKeys, string id, DateTime timestamp, object message, CancellationToken cancellationToken)
    {
        var properties = new BasicProperties
        {
            MessageId = id,
            Timestamp = new AmqpTimestamp(timestamp.Ticks),
        };

        return producer.ProduceAsync(exchange, routingKeys, message, properties, cancellationToken);
    }

    /// <summary>
    /// Publishes a <paramref name="message"/> with one routing key, string identifier, and <paramref name="timestamp"/>.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKey">The target routing key.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, string routingKey, string id, DateTime timestamp, object message, CancellationToken cancellationToken)
    {
        var properties = new BasicProperties
        {
            MessageId = id,
            Timestamp = new AmqpTimestamp(timestamp.Ticks),
        };

        return producer.ProduceAsync(exchange, [routingKey], message, properties, cancellationToken);
    }

    /// <summary>
    /// Publishes a <paramref name="message"/> with custom <paramref name="properties"/>, string identifier, and <paramref name="timestamp"/>.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKeys">The target routing keys, or <see langword="null"/>.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties to update and use, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, IEnumerable<string>? routingKeys, string id, DateTime timestamp, object message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        properties ??= new BasicProperties();

        properties.MessageId = id;
        properties.Timestamp = new AmqpTimestamp(timestamp.Ticks);

        return producer.ProduceAsync(exchange, routingKeys, message, properties, cancellationToken);
    }

    /// <summary>
    /// Publishes a <paramref name="message"/> with one routing key, custom <paramref name="properties"/>, string identifier, and <paramref name="timestamp"/>.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKey">The target routing key.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties to update and use, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, string routingKey, string id, DateTime timestamp, object message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        properties ??= new BasicProperties();

        properties.MessageId = id;
        properties.Timestamp = new AmqpTimestamp(timestamp.Ticks);

        return producer.ProduceAsync(exchange, [routingKey], message, properties, cancellationToken);
    }

    /// <summary>
    /// Publishes a <paramref name="message"/> using a generated string representation of a GUID identifier.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, Guid id, DateTime timestamp, object message, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange: null, routingKeys: null, id.ToString(), timestamp, message, cancellationToken);
    }

    /// <summary>
    /// Publishes a <paramref name="message"/> with routing keys and a GUID identifier.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKeys">The target routing keys, or <see langword="null"/>.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, IEnumerable<string>? routingKeys, Guid id, DateTime timestamp, object message, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange, routingKeys, id.ToString(), timestamp, message, cancellationToken);
    }

    /// <summary>
    /// Publishes a <paramref name="message"/> with one routing key and a GUID identifier.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKey">The target routing key.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, string routingKey, Guid id, DateTime timestamp, object message, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange, routingKey, id.ToString(), timestamp, message, cancellationToken);
    }

    /// <summary>
    /// Publishes a <paramref name="message"/> with custom <paramref name="properties"/> and a GUID identifier.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties to update and use, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, Guid id, DateTime timestamp, object message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange: null, routingKeys: null, id.ToString(), timestamp, message, properties, cancellationToken);
    }

    /// <summary>
    /// Publishes a <paramref name="message"/> with routing keys, custom <paramref name="properties"/>, and a GUID identifier.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKeys">The target routing keys, or <see langword="null"/>.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties to update and use, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, IEnumerable<string>? routingKeys, Guid id, DateTime timestamp, object message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange, routingKeys, id.ToString(), timestamp, message, properties, cancellationToken);
    }

    /// <summary>
    /// Publishes a <paramref name="message"/> with one routing key, custom <paramref name="properties"/>, and a GUID identifier.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKey">The target routing key.</param>
    /// <param name="id">The message identifier.</param>
    /// <param name="timestamp">The message timestamp.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties to update and use, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, string routingKey, Guid id, DateTime timestamp, object message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange, routingKey, id.ToString(), timestamp, message, properties, cancellationToken);
    }

    /// <summary>
    /// Publishes an <see cref="IMessage"/> using its identifier and timestamp.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKeys">The target routing keys, or <see langword="null"/>.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, IEnumerable<string>? routingKeys, IMessage message, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange, routingKeys, message.Id, message.Timestamp, message, cancellationToken);
    }

    /// <summary>
    /// Publishes an <see cref="IMessage"/> to one routing key.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKey">The target routing key.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, string routingKey, IMessage message, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange, routingKey, message.Id, message.Timestamp, message, cancellationToken);
    }

    /// <summary>
    /// Publishes an <see cref="IMessage"/> using its metadata.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, IMessage message, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(message.Id, message.Timestamp, message, cancellationToken);
    }

    /// <summary>
    /// Publishes an <see cref="IMessage"/> with custom <paramref name="properties"/>.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKeys">The target routing keys, or <see langword="null"/>.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties to update and use, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, IEnumerable<string>? routingKeys, IMessage message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange, routingKeys, message.Id, message.Timestamp, message, properties, cancellationToken);
    }

    /// <summary>
    /// Publishes an <see cref="IMessage"/> to one routing key with custom <paramref name="properties"/>.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="exchange">The target exchange, or <see langword="null"/>.</param>
    /// <param name="routingKey">The target routing key.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties to update and use, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, string? exchange, string routingKey, IMessage message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(exchange, routingKey, message.Id, message.Timestamp, message, properties, cancellationToken);
    }

    /// <summary>
    /// Publishes an <see cref="IMessage"/> with custom <paramref name="properties"/> using its metadata.
    /// </summary>
    /// <param name="producer">The producer to use.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties to update and use, or <see langword="null"/>.</param>
    /// <param name="cancellationToken">The token used to cancel publication.</param>
    /// <returns>A task representing the asynchronous publication.</returns>
    public static Task ProduceAsync(this IProducer producer, IMessage message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        return producer.ProduceAsync(message.Id, message.Timestamp, message, properties, cancellationToken);
    }
}
