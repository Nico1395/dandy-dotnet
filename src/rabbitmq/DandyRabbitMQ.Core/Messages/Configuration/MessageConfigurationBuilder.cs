using System.Collections.Concurrent;

namespace DandyRabbitMQ.Core.Messages.Configuration;

/// <summary>
/// Builds configuration for one message type.
/// </summary>
public sealed class MessageConfigurationBuilder
{
    private readonly ConcurrentDictionary<string, object> _metadata;
    private readonly MessageConfiguration _message;
    /// <summary>
    /// Initializes a builder for a message type.
    /// </summary>
    /// <param name="messageType">The message runtime type.</param>
    public MessageConfigurationBuilder(Type messageType)
    {
        _metadata = [];
        _message = new MessageConfiguration
        {
            RuntimeType = messageType,
            Key = messageType.Name,        // Defaulting to the type name. This can be overridden with the builder or with an attribute
            Metadata = _metadata,
        };
    }

    /// <summary>
    /// Sets the message <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The message key.</param>
    /// <returns>This builder.</returns>
    public MessageConfigurationBuilder SetKey(string key)
    {
        _message.Key = key;
        return this;
    }

    /// <summary>
    /// Sets the message <paramref name="exchange"/>.
    /// </summary>
    /// <param name="exchange">The exchange name.</param>
    /// <returns>This builder.</returns>
    public MessageConfigurationBuilder SetExchange(string exchange)
    {
        _message.Exchange = exchange;
        return this;
    }

    /// <summary>
    /// Sets the message routing keys.
    /// </summary>
    /// <param name="routingKeys">The routing keys.</param>
    /// <returns>This builder.</returns>
    public MessageConfigurationBuilder SetRoutingKeys(params string[] routingKeys)
    {
        _message.RoutingKeys = routingKeys;
        return this;
    }

    /// <summary>
    /// Adds message metadata.
    /// </summary>
    /// <param name="key">The metadata key.</param>
    /// <param name="value">The metadata value.</param>
    /// <returns>This builder.</returns>
    public MessageConfigurationBuilder AddMetadata(string key, object value)
    {
        _metadata[key] = value;
        return this;
    }

    /// <summary>
    /// Builds the message configuration.
    /// </summary>
    /// <returns>The message configuration.</returns>
    public MessageConfiguration Build()
    {
        return _message;
    }
}
