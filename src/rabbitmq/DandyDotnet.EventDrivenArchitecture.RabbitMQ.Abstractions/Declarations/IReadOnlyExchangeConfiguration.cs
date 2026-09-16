namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Declarations;

/// <summary>
/// Defines read-only exchange configuration properties.
/// </summary>
public interface IReadOnlyExchangeConfiguration
{
    /// <summary>
    /// Gets the exchange name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the exchange type (e.g., direct, fanout, topic, headers).
    /// </summary>
    string ExchangeType { get; }

    /// <summary>
    /// Gets a value indicating whether the exchange is durable.
    /// </summary>
    bool Durable { get; }

    /// <summary>
    /// Gets a value indicating whether the exchange should auto-delete when no queues are bound to it.
    /// </summary>
    bool AutoDelete { get; }
}