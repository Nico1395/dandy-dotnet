namespace DandyRabbitMQ.Core.Messages;

/// <summary>
/// Associates a message type with an exchange.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class MessageExchangeAttribute(string exchange) : MessageAttribute
{
    /// <summary>
    /// Gets the exchange name.
    /// </summary>
    public string Exchange { get; } = exchange;
}
