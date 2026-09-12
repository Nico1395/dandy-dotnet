namespace DandyRabbitMQ.Core.Messages.Configuration;

/// <summary>
/// Stores metadata used to publish a message type.
/// </summary>
public sealed class MessageConfiguration
{
    /// <summary>
    /// Gets or initializes the message runtime type.
    /// </summary>
    public required Type RuntimeType { get; init; }

    /// <summary>
    /// Gets or sets the message key.
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    /// Gets or sets the configured exchange.
    /// </summary>
    public string? Exchange { get; set; }

    /// <summary>
    /// Gets or sets the configured routing keys.
    /// </summary>
    public string[]? RoutingKeys { get; set; }

    /// <summary>
    /// Gets or initializes message metadata.
    /// </summary>
    public required IReadOnlyDictionary<string, object> Metadata { get; init; }
}
