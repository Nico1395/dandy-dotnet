using DandyRabbitMQ.Core.Declarations.Configuration;
using RabbitMQ.Client.Events;

namespace DandyRabbitMQ.Consumer;

/// <summary>
/// Provides message delivery data and declaration settings to a consumer.
/// </summary>
public sealed class ConsumerContext
{
    internal ConsumerContext(
        BasicDeliverEventArgs deliverArgs,
        ChannelConfiguration channelConfiguration)
    {
        DeliverArgs = deliverArgs;
        ChannelConfiguration = channelConfiguration;
    }

    /// <summary>
    /// Gets the RabbitMQ delivery event arguments.
    /// </summary>
    public BasicDeliverEventArgs DeliverArgs { get; }

    /// <summary>
    /// Gets the channel declaration configuration associated with the delivery.
    /// </summary>
    public ChannelConfiguration ChannelConfiguration { get; }
}
