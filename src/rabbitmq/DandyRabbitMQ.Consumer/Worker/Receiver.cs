using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using DandyDotnet.Serialization.Abstractions;
using DandyRabbitMQ.Consumer.Configuration;
using DandyRabbitMQ.Consumer.Interceptors;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Encoding;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DandyRabbitMQ.Consumer.Worker;

/// <summary>
/// Receives deliveries, invokes consumers, and acknowledges results.
/// </summary>
public class Receiver(
    ConsumerConfiguration consumerConfiguration,
    MessagesConfiguration messagesConfiguration,
    IServiceProvider serviceProvider,
    IConsumerPipeline consumerPipeline,
    IPayloadEncoder payloadEncoder,
    ISerializer serializer) : IReceiver
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> _executeAsync = [];

    /// <inheritdoc/>
    /// <param name="args">The delivery event arguments.</param>
    /// <param name="ackLock">The lock protecting channel acknowledgements.</param>
    /// <param name="channel">The RabbitMQ channel.</param>
    /// <param name="configuration">The channel configuration.</param>
    /// <param name="cancellationToken">The token used to cancel processing.</param>
    /// <returns>A task representing asynchronous delivery processing.</returns>
    public async Task ReceiveAsync(BasicDeliverEventArgs args, SemaphoreSlim ackLock, IChannel channel, ChannelConfiguration configuration, CancellationToken cancellationToken)
    {
        var result = ConsumerResult.Nack();     // Assume failure
        var context = new ConsumerContext(args, configuration);
        object? message = null;

        try
        {
            if (string.IsNullOrWhiteSpace(args.BasicProperties.Type))
                throw new InvalidOperationException("Message type is expected to be set.");

            if (!messagesConfiguration.MessagesByKey.TryGetValue(args.BasicProperties.Type, out var messageConfiguration))
                throw new InvalidOperationException("Failed to resolve message type.");

            var serialized = payloadEncoder.Decode(args.Body.Span);
            if (string.IsNullOrWhiteSpace(serialized))
                throw new InvalidOperationException("Failed to deserialize message.");

            message = serializer.Deserialize(serialized, messageConfiguration.RuntimeType);
            using var scope = serviceProvider.CreateScope();

            var executeAsync = GetExecuteAsync(messageConfiguration.RuntimeType);
            if (executeAsync.Invoke(consumerPipeline, parameters: [message, context, cancellationToken]) is not Task<ConsumerResult> task)
                throw new InvalidOperationException($"Failed to handle '{messageConfiguration.RuntimeType}'.");

            result = await task;
        }
        catch (Exception ex)
        {
            consumerConfiguration.OnExceptionWhenReceivingMessage?.Invoke(serviceProvider, ex);
        }

        await AckOrNackAsync(args, ackLock, channel, result, cancellationToken);
        await InterceptAckOrNackAsync(message, context, result, cancellationToken);
    }

    private static MethodInfo GetExecuteAsync(Type messageType)
    {
        return _executeAsync.GetOrAdd(messageType, type =>
        {
            var executeAsync = typeof(IConsumerPipeline).GetMethod(nameof(IConsumerPipeline.ExecuteAsync))?.MakeGenericMethod(type);
            if (executeAsync == null)
                throw new UnreachableException($"Failed to reflect method '{nameof(IConsumerPipeline.ExecuteAsync)}' from '{nameof(IConsumerPipeline)}'.");

            return executeAsync;
        });
    }

    private async Task AckOrNackAsync(BasicDeliverEventArgs args, SemaphoreSlim ackLock, IChannel channel, ConsumerResult result, CancellationToken cancellationToken)
    {
        try
        {
            // Using a lock so the channel can be used safely in multiple threads. Intentionally avoiding the cancellation token,
            // because when stopping the ack or nack should still go through to avoid message noise in the queue.
            await ackLock.WaitAsync(cancellationToken: CancellationToken.None);

            // Evaluate and act accordingly
            if (result.Status == ConsumerStatus.Ack)
            {
                await channel.BasicAckAsync(
                    args.DeliveryTag,
                    multiple: result.Multiple,
                    cancellationToken);
            }
            else
            {
                await channel.BasicNackAsync(
                    args.DeliveryTag,
                    multiple: result.Multiple,
                    requeue: result.Requeue,
                    cancellationToken);
            }
        }
        catch (Exception ex)
        {
            consumerConfiguration.OnExceptionWhenAckOrNack?.Invoke(serviceProvider, ex);
        }
        finally
        {
            ackLock.Release();
        }
    }

    private async Task InterceptAckOrNackAsync(object? message, ConsumerContext context, ConsumerResult result, CancellationToken cancellationToken)
    {
        if (message == null)
            return;

        try
        {
            var interceptor = serviceProvider.GetService<IConsumerInterceptor>();
            if (interceptor == null)
                return;

            if (result.Status == ConsumerStatus.Ack)
                await interceptor.OnAfterAckAsync(message, context, result, cancellationToken);
            else
                await interceptor.OnAfterNackAsync(message, context, result, cancellationToken);
        }
        catch (Exception ex)
        {
            consumerConfiguration.OnExceptionWhenIntercepting?.Invoke(serviceProvider, ex);
        }
    }
}
