namespace DandyRabbitMQ.Core.Messages;

/// <summary>
/// Associates a message type with a message key.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MessageKeyAttribute(string key) : MessageAttribute
{
    /// <summary>
    /// Gets the message key.
    /// </summary>
    public string Key { get; } = key;
}
