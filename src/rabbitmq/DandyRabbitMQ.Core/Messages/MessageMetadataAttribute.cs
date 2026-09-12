namespace DandyRabbitMQ.Core.Messages;

/// <summary>
/// Associates arbitrary metadata with a message type.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class MessageMetadataAttribute(string key, object value) : MessageAttribute
{
    /// <summary>
    /// Gets the metadata key.
    /// </summary>
    public string Key { get; } = key;

    /// <summary>
    /// Gets the metadata value.
    /// </summary>
    public object Value { get; } = value;
}
