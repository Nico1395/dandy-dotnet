using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Declarations;
using RabbitMQ.Client.Events;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;

/// <summary>
/// Provides message delivery data and declaration settings to a consumer.
/// </summary>
public sealed class ConsumerContext(
    BasicDeliverEventArgs deliverArgs,
    IReadOnlyChannelConfiguration channelConfiguration)
{
    /// <summary>
    /// Gets the RabbitMQ delivery event arguments.
    /// </summary>
    public BasicDeliverEventArgs DeliverArgs { get; } = deliverArgs;

    /// <summary>
    /// Gets the channel declaration configuration associated with the delivery.
    /// </summary>
    public IReadOnlyChannelConfiguration ChannelConfiguration { get; } = channelConfiguration;

    public bool IsRetry()
    {
        return DeliverArgs.Redelivered;
    }
}
