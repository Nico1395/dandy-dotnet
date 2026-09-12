using System.Collections.Concurrent;

namespace DandyRabbitMQ.Core.Declarations.Configuration;

/// <summary>
/// Contains configured RabbitMQ channels.
/// </summary>
public sealed class DeclarationsConfiguration
{
    internal ConcurrentDictionary<string, ChannelConfiguration> Channels { get; } = new();
    /// <summary>
    /// Gets channel configurations indexed by queue name.
    /// </summary>
    public IReadOnlyDictionary<string, ChannelConfiguration> ChannelsByQueueName => Channels;
}
