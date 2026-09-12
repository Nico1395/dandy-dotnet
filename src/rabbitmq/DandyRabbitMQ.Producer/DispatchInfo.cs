using DandyRabbitMQ.Core.Messages.Configuration;
using RabbitMQ.Client;

namespace DandyRabbitMQ.Producer;

/// <summary>
/// Contains the resolved values used to publish a message.
/// </summary>
public readonly struct DispatchInfo
{
    private DispatchInfo(Type runtimeType, string exchange, string[] routingKeys, BasicProperties properties)
    {
        RuntimeType = runtimeType;
        Exchange = exchange;
        RoutingKeys = routingKeys;
        Properties = properties;
    }

    /// <summary>
    /// Gets the runtime type used to serialize the message.
    /// </summary>
    public Type RuntimeType { get; }

    /// <summary>
    /// Gets the exchange to which the message is published.
    /// </summary>
    public string Exchange { get; }

    /// <summary>
    /// Gets the routing keys used for publication.
    /// </summary>
    public string[] RoutingKeys { get; }

    /// <summary>
    /// Gets the AMQP properties used for publication.
    /// </summary>
    public BasicProperties Properties { get; }

    /// <summary>
    /// Resolves publication settings from <paramref name="message"/> configuration and explicit values.
    /// </summary>
    /// <param name="messagesConfiguration">The configured message metadata.</param>
    /// <param name="exchange">The exchange, or <see langword="null"/> to use configured metadata.</param>
    /// <param name="routingKeys">The routing keys, or <see langword="null"/> to use configured metadata.</param>
    /// <param name="message">The message to publish.</param>
    /// <param name="properties">The AMQP properties, or <see langword="null"/> to create default properties.</param>
    /// <returns>The resolved dispatch information.</returns>
    /// <exception cref="InvalidOperationException">Thrown when required <paramref name="message"/>, routing key, or <paramref name="exchange"/> metadata cannot be resolved.</exception>
    public static DispatchInfo Create(MessagesConfiguration messagesConfiguration, string? exchange, IEnumerable<string>? routingKeys, object message, BasicProperties? properties)
    {
        properties ??= new BasicProperties();

        var messageType = message.GetType();
        var messageConfiguration = messagesConfiguration.MessagesByRuntimeType.GetValueOrDefault(messageType);

        // Determine message type
        properties.Type ??= messageConfiguration?.Key ?? messageType.Name;

        // Determine routing keys
        string[] targetRoutingKeys;
        if (routingKeys == null)
        {
            if (messageConfiguration == null)
                throw new InvalidOperationException($"Failed to resolve message configuration for type '{messageType}'. You should add message types via assembly scanning or manual configuration, even if they are decorated with attributes, so their metadata is cached.");

            targetRoutingKeys = messageConfiguration.RoutingKeys ?? throw new InvalidOperationException("Failed to resolve routing keys. None were provided and none were configured.");
        }
        else
        {
            targetRoutingKeys = routingKeys.ToArray();
        }

        targetRoutingKeys = targetRoutingKeys.Where(routingKey => !string.IsNullOrWhiteSpace(routingKey)).Distinct().ToArray();
        if (targetRoutingKeys.Length == 0)
            throw new InvalidOperationException("Routing keys were resolved, but they are empty.");

        // Determine exchange
        if (exchange == null)
        {
            if (messageConfiguration == null)
                throw new InvalidOperationException($"Failed to resolve message configuration for type '{messageType}'. You should add message types via assembly scanning or manual configuration, even if they are decorated with attributes, so their metadata is cached.");

            exchange ??= messageConfiguration.Exchange;
        }

        if (string.IsNullOrWhiteSpace(exchange))
            throw new InvalidOperationException("Exchange cannot be null or whitespace.");

        // Determine runtime type
        var runtimeType = messageConfiguration?.RuntimeType ?? messageType;

        return new DispatchInfo(runtimeType, exchange, targetRoutingKeys, properties);
    }
}
