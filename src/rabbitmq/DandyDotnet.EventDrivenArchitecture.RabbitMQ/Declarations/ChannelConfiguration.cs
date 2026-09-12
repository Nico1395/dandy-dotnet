using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Declarations;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;

/// <summary>
/// Configures a RabbitMQ channel, exchange, queue, and consumer settings.
/// </summary>
public sealed class ChannelConfiguration(string exchangeName, string queueName) : IReadOnlyChannelConfiguration
{
    /// <summary>
    /// Gets or sets the options used when creating the channel.
    /// </summary>
    public CreateChannelOptions? Options { get; set; }

    /// <summary>
    /// Gets or sets the exchange declaration.
    /// </summary>
    public IReadOnlyExchangeConfiguration Exchange { get; set; } = new ExchangeConfiguration(exchangeName);

    /// <summary>
    /// Gets or sets the queue declaration.
    /// </summary>
    public IReadOnlyQueueConfiguration Queue { get; set; } = new QueueConfiguration(queueName);

    /// <summary>
    /// Gets or sets the channel prefetch size.
    /// </summary>
    public uint PrefetchSize { get; set; } = 0;

    /// <summary>
    /// Gets or sets the channel prefetch count.
    /// </summary>
    public ushort PrefetchCount { get; set; } = 32;

    /// <summary>
    /// Gets or sets a value indicating whether the quality-of-service setting is global.
    /// </summary>
    public bool Global { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether messages are acknowledged automatically.
    /// </summary>
    public bool AutoAck { get; set; }

    /// <summary>
    /// Gets or sets the consumer tag.
    /// </summary>
    public string? ConsumerTag { get; set; }
}
