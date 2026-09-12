using DandyRabbitMQ.Consumer.Configuration;
using DandyRabbitMQ.Core.Connectivity;
using DandyRabbitMQ.Core.Declarations;
using DandyRabbitMQ.Core.Declarations.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DandyRabbitMQ.Consumer.Worker;

internal sealed class ConsumerWorker(
    ConsumerConfiguration consumerConfiguration,
    DeclarationsConfiguration declarationsConfiguration,
    IServiceProvider serviceProvider,
    IDeclarer declarer,
    IConnectionProvider connectionProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var channels = declarationsConfiguration.ChannelsByQueueName.Values.ToList();
            if (channels.Count == 0)
                return;

            var connection = await connectionProvider.GetAsync(stoppingToken);
            foreach (var channelConfiguration in channels)
            {
                var channel = await connection.CreateChannelAsync(channelConfiguration.Options, stoppingToken);
                await InitializeChannelAsync(channel, channelConfiguration, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            consumerConfiguration.OnExceptionWhenInitializingWorker?.Invoke(serviceProvider, ex);
            throw;
        }
    }

    private async Task InitializeChannelAsync(IChannel channel, ChannelConfiguration configuration, CancellationToken cancellationToken)
    {
        var ackLock = new SemaphoreSlim(1, 1);

        await declarer.DeclareQueueAsync(configuration.Queue.Name, channel, cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (_, args) => serviceProvider
            .GetRequiredService<IReceiver>()
            .ReceiveAsync(args, ackLock, channel, configuration, cancellationToken);

        if (string.IsNullOrWhiteSpace(configuration.ConsumerTag))
        {
            await channel.BasicConsumeAsync(
                queue: configuration.Queue.Name,
                autoAck: configuration.AutoAck,
                consumer: consumer,
                cancellationToken: cancellationToken);
        }
        else
        {
            await channel.BasicConsumeAsync(
                queue: configuration.Queue.Name,
                autoAck: configuration.AutoAck,
                consumerTag: configuration.ConsumerTag,
                consumer: consumer,
                cancellationToken: cancellationToken);
        }
    }
}
