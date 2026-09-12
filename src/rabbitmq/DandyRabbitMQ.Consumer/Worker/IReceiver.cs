using DandyRabbitMQ.Core.Declarations.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DandyRabbitMQ.Consumer.Worker;

/// <summary>
/// Receives and processes RabbitMQ deliveries.
/// </summary>
public interface IReceiver
{
    /// <summary>
    /// Processes a RabbitMQ delivery.
    /// </summary>
    /// <param name="args">The delivery event arguments.</param>
    /// <param name="ackLock">The lock protecting channel acknowledgements.</param>
    /// <param name="channel">The RabbitMQ channel.</param>
    /// <param name="configuration">The channel configuration.</param>
    /// <param name="cancellationToken">The token used to cancel processing.</param>
    /// <returns>A task representing asynchronous delivery processing.</returns>
    Task ReceiveAsync(BasicDeliverEventArgs args, SemaphoreSlim ackLock, IChannel channel, ChannelConfiguration configuration, CancellationToken cancellationToken);
}
