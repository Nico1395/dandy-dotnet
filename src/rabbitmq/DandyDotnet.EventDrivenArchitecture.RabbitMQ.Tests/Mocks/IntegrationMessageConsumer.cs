using System.Collections.Concurrent;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Mocks;

public sealed class IntegrationMessageConsumer : IConsumer<IntegrationMessage>
{
    public static ConcurrentQueue<(IntegrationMessage Message, ConsumerContext Context)> Received { get; } = new();
    public static ConsumerResult Result { get; set; } = ConsumerResult.Ack();

    public Task<ConsumerResult> ConsumeAsync(IntegrationMessage message, ConsumerContext context, CancellationToken cancellationToken)
    {
        Received.Enqueue((message, context));
        return Task.FromResult(Result);
    }

    public static void Reset()
    {
        while (Received.TryDequeue(out _))
        {
        }

        Result = ConsumerResult.Ack();
    }
}
