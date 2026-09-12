namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;

/// <summary>
/// Defines metadata required for a publishable message.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Gets the message identifier.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the message timestamp.
    /// </summary>
    DateTime Timestamp { get; }
}
