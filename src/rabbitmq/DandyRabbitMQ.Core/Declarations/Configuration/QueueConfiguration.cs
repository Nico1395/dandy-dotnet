namespace DandyRabbitMQ.Core.Declarations.Configuration;

/// <summary>
/// Configures a RabbitMQ queue declaration.
/// </summary>
public sealed class QueueConfiguration(string name)
{
    /// <summary>
    /// Gets the queue name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets or sets the routing keys bound to the queue.
    /// </summary>
    public string[]? RoutingKeys { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the queue is durable.
    /// </summary>
    public bool Durable { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the queue is exclusive.
    /// </summary>
    public bool Exclusive { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the queue is deleted automatically.
    /// </summary>
    public bool AutoDelete { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether declaration should not wait for a response.
    /// </summary>
    public bool NoWait { get; set; }

    /// <summary>
    /// Gets or sets the queue declaration arguments.
    /// </summary>
    public Dictionary<string, object?> Arguments { get; set; } = new()
    {
        { "x-queue-type", "quorum" },
    };
}
