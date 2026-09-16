using System.Threading.Channels;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;

public sealed class IntegrationMessageConsumer : IConsumer<IntegrationMessage>
{
    private readonly Channel<(IntegrationMessage Message, ConsumerContext Context)> _deliveries =
        Channel.CreateUnbounded<(IntegrationMessage, ConsumerContext)>();

    public async Task<ConsumerResult> ConsumeAsync(IntegrationMessage message, ConsumerContext context, CancellationToken cancellationToken)
    {
        await _deliveries.Writer.WriteAsync((message, context), cancellationToken);
        return message.Content == "retry" && !context.DeliverArgs.Redelivered
            ? ConsumerResult.NackRequeue()
            : ConsumerResult.Ack();
    }

    public async Task<(IntegrationMessage Message, ConsumerContext Context)> WaitForMessageAsync(Guid id)
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        while (true)
        {
            var delivery = await _deliveries.Reader.ReadAsync(timeout.Token);
            if (delivery.Message.Id == id)
                return delivery;
        }
    }
}