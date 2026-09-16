namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Declarations;

/// <summary>
/// Defines read-only queue configuration properties.
/// </summary>
public interface IReadOnlyQueueConfiguration
{
    /// <summary>
    /// Gets the queue name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the routing keys associated with the queue.
    /// </summary>
    string[]? RoutingKeys { get; }

    /// <summary>
    /// Gets a value indicating whether the queue is durable.
    /// </summary>
    bool Durable { get; }

    /// <summary>
    /// Gets a value indicating whether the queue is exclusive to the connection.
    /// </summary>
    bool Exclusive { get; }

    /// <summary>
    /// Gets a value indicating whether the queue should auto-delete when no consumers are connected.
    /// </summary>
    bool AutoDelete { get; }

    /// <summary>
    /// Gets a value indicating whether to use no-wait when declaring the queue.
    /// </summary>
    bool NoWait { get; }

    /// <summary>
    /// Gets additional optional arguments for queue declaration.
    /// </summary>
    IReadOnlyDictionary<string, object?> Arguments { get; }
}