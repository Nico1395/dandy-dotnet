namespace DandyRabbitMQ.Consumer;

/// <summary>
/// Specifies the acknowledgement operation for a consumed message.
/// </summary>
public enum ConsumerStatus
{
    /// <summary>
    /// Acknowledges the message.
    /// </summary>
    Ack,

    /// <summary>
    /// Negatively acknowledges the message.
    /// </summary>
    Nack,
}
