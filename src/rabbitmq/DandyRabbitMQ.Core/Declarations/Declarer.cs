using DandyRabbitMQ.Core.Connectivity;
using DandyRabbitMQ.Core.Declarations.Configuration;
using RabbitMQ.Client;

namespace DandyRabbitMQ.Core.Declarations;

/// <summary>
/// Declares configured RabbitMQ topology.
/// </summary>
public class Declarer(
    IConnectionProvider connectionProvider,
    DeclarationsConfiguration declarationsConfiguration) : IDeclarer
{
    /// <summary>
    /// Declares the queue and its associated exchange and bindings.
    /// </summary>
    /// <param name="queueName">The configured queue name.</param>
    /// <param name="channel">The channel to use, or <see langword="null"/> to create one.</param>
    /// <param name="cancellationToken">The token used to cancel declaration.</param>
    /// <returns>A task representing the asynchronous declaration.</returns>
    /// <exception cref="ArgumentException">Thrown when no configuration exists for <paramref name="queueName"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when queue routing keys are not configured.</exception>
    public async Task DeclareQueueAsync(string queueName, IChannel? channel, CancellationToken cancellationToken)
    {
        if (!declarationsConfiguration.ChannelsByQueueName.TryGetValue(queueName, out var channelConfiguration))
            throw new ArgumentException($"No channel configuration found for queue name '{queueName}'", nameof(queueName));

        if (channel == null)
        {
            var connection = await connectionProvider.GetAsync(cancellationToken);
            channel = await connection.CreateChannelAsync(channelConfiguration.Options, cancellationToken);
        }

        await channel.ExchangeDeclareAsync(
            exchange: channelConfiguration.Exchange.Name,
            type: channelConfiguration.Exchange.ExchangeType,
            durable: channelConfiguration.Exchange.Durable,
            autoDelete: channelConfiguration.Exchange.AutoDelete,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: channelConfiguration.Queue.Name,
            durable: channelConfiguration.Queue.Durable,
            exclusive: channelConfiguration.Queue.Exclusive,
            autoDelete: channelConfiguration.Queue.AutoDelete,
            arguments: channelConfiguration.Queue.Arguments,
            noWait: channelConfiguration.Queue.NoWait,
            cancellationToken);

        await channel.BasicQosAsync(
            prefetchSize: channelConfiguration.PrefetchSize,
            prefetchCount: channelConfiguration.PrefetchCount,
            global: channelConfiguration.Global,
            cancellationToken);

        if (channelConfiguration.Queue.RoutingKeys == null)
            throw new InvalidOperationException("Routing keys are not set.");

        foreach (var routingKey in channelConfiguration.Queue.RoutingKeys)
        {
            await channel.QueueBindAsync(
                queue: channelConfiguration.Queue.Name,
                exchange: channelConfiguration.Exchange.Name,
                routingKey: routingKey,
                arguments: channelConfiguration.Queue.Arguments,
                noWait: channelConfiguration.Queue.NoWait,
                cancellationToken: cancellationToken);
        }
    }
}
