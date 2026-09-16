using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class ConsumerPipelineTests
{
    [Fact]
    public async Task ExecuteAsync_InvokesRegisteredConsumer()
    {
        var consumer = new TestConsumer();
        var services = new ServiceCollection().AddSingleton<IConsumer<TestMessage>>(consumer).BuildServiceProvider();
        var pipeline = new ConsumerPipeline(services);

        var result = await pipeline.ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None);

        Assert.Equal(ConsumerStatus.Ack, result.Status);
        Assert.True(consumer.Called);
    }

    [Fact]
    public async Task ExecuteAsync_InvokesMiddlewareAroundConsumer()
    {
        var calls = new List<string>();
        var services = new ServiceCollection()
            .AddSingleton<IConsumer<TestMessage>>(new TestConsumer(calls))
            .AddSingleton<IConsumerMiddleware<TestMessage>>(new TestMiddleware(calls))
            .BuildServiceProvider();

        await new ConsumerPipeline(services).ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None);

        Assert.Equal(["before", "consumer", "after"], calls);
    }

    [Fact]
    public async Task ExecuteAsync_InvokesExceptionHandlerAndRethrows()
    {
        var handler = new TestExceptionHandler();
        var services = new ServiceCollection()
            .AddSingleton<IConsumer<TestMessage>>(new ThrowingConsumer())
            .AddSingleton<IConsumerExceptionHandler<TestMessage>>(handler)
            .BuildServiceProvider();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new ConsumerPipeline(services).ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None));
        Assert.True(handler.Called);
    }

    private static ConsumerContext CreateContext()
    {
        var args = new BasicDeliverEventArgs("consumer", 1, false, "exchange", "routing", new BasicProperties(), ReadOnlyMemory<byte>.Empty);
        return new ConsumerContext(args, new ChannelConfiguration("exchange", "queue"));
    }

    private sealed class TestMessage
    {
    }
    private sealed class TestConsumer(List<string>? calls = null) : IConsumer<TestMessage>
    {
        public bool Called { get; private set; }
        public Task<ConsumerResult> ConsumeAsync(TestMessage message, ConsumerContext context, CancellationToken cancellationToken)
        {
            Called = true;
            calls?.Add("consumer");
            return Task.FromResult(ConsumerResult.Ack());
        }
    }
    private sealed class ThrowingConsumer : IConsumer<TestMessage>
    {
        public Task<ConsumerResult> ConsumeAsync(TestMessage message, ConsumerContext context, CancellationToken cancellationToken) => throw new InvalidOperationException();
    }
    private sealed class TestMiddleware(List<string> calls) : IConsumerMiddleware<TestMessage>
    {
        public async Task<ConsumerResult> InterceptAsync(TestMessage message, ConsumerContext context, ConsumerDelegate nextStep, CancellationToken cancellationToken)
        {
            calls.Add("before"); var result = await nextStep(); calls.Add("after"); return result;
        }
    }
    private sealed class TestExceptionHandler : IConsumerExceptionHandler<TestMessage>
    {
        public bool Called { get; private set; }
        public Task HandleAsync(TestMessage message, ConsumerContext context, Exception exception, CancellationToken cancellationToken) { Called = true; return Task.CompletedTask; }
    }
}
