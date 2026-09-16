using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Declarations;

/// <summary>
/// Defines read-only channel configuration properties for RabbitMQ operations.
/// </summary>
public interface IReadOnlyChannelConfiguration
{
    /// <summary>
    /// Gets the channel creation options.
    /// </summary>
    CreateChannelOptions? Options { get; }

    /// <summary>
    /// Gets the read-only exchange configuration.
    /// </summary>
    IReadOnlyExchangeConfiguration Exchange { get; }

    /// <summary>
    /// Gets the read-only queue configuration.
    /// </summary>
    IReadOnlyQueueConfiguration Queue { get; }

    /// <summary>
    /// Gets the prefetch size.
    /// </summary>
    uint PrefetchSize { get; }

    /// <summary>
    /// Gets the prefetch count.
    /// </summary>
    ushort PrefetchCount { get; }

    /// <summary>
    /// Gets a value indicating whether the QoS settings apply globally.
    /// </summary>
    bool Global { get; }

    /// <summary>
    /// Gets a value indicating whether messages should be acknowledged automatically.
    /// </summary>
    bool AutoAck { get; }

    /// <summary>
    /// Gets or sets the consumer tag.
    /// </summary>
    public string? ConsumerTag { get; set; }
}