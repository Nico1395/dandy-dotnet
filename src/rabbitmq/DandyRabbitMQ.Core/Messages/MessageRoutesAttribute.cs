namespace DandyRabbitMQ.Core.Messages;

/// <summary>
/// Associates routing keys with a message type.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MessageRoutesAttribute(params string[] keys) : MessageAttribute
{
    /// <summary>
    /// Gets the routing keys.
    /// </summary>
    public string[] Keys { get; } = keys;
}
