namespace DandyRabbitMQ.Core.Messages;

/// <summary>
/// Provides default identifier and timestamp properties for messages.
/// </summary>
public abstract class Message : IMessage
{
    /// <summary>
    /// Gets or initializes the message identifier.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Gets or initializes the message timestamp.
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
