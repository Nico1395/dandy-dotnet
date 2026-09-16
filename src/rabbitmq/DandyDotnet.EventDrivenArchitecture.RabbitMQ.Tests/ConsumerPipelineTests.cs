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

    [Fact]
    public async Task MiddlewareReturningAck_SkipsConsumerWhenDesignedToShortCircuit()
    {
        var consumer = new TestConsumer();
        var services = new ServiceCollection()
            .AddSingleton<IConsumer<TestMessage>>(consumer)
            .AddSingleton<IConsumerMiddleware<TestMessage>>(new ShortCircuitMiddleware())
            .BuildServiceProvider();

        var result = await new ConsumerPipeline(services).ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None);

        Assert.Equal(ConsumerStatus.Ack, result.Status);
        Assert.False(consumer.Called);
    }

    [Fact]
    public async Task MiddlewareReturningResult_PreservesConsumerResult()
    {
        var result = await new ConsumerPipeline(new ServiceCollection()
            .AddSingleton<IConsumer<TestMessage>>(new TestConsumer())
            .AddSingleton<IConsumerMiddleware<TestMessage>>(new ResultMiddleware())
            .BuildServiceProvider()).ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None);

        Assert.Equal(ConsumerStatus.Nack, result.Status);
        Assert.True(result.Requeue);
    }

    [Fact]
    public async Task Pipeline_PassesCancellationTokenToConsumerAndMiddleware()
    {
        using var cancellation = new CancellationTokenSource();
        var consumer = new TokenConsumer(cancellation.Token);
        var middleware = new TokenMiddleware(cancellation.Token);
        var services = new ServiceCollection()
            .AddSingleton<IConsumer<TestMessage>>(consumer)
            .AddSingleton<IConsumerMiddleware<TestMessage>>(middleware)
            .BuildServiceProvider();

        await new ConsumerPipeline(services).ExecuteAsync(new TestMessage(), CreateContext(), cancellation.Token);

        Assert.True(consumer.Received && middleware.Received);
    }

    [Fact]
    public async Task MiddlewareThrowingException_InvokesExceptionHandler()
    {
        var handler = new TestExceptionHandler();
        var services = new ServiceCollection()
            .AddSingleton<IConsumer<TestMessage>>(new TestConsumer())
            .AddSingleton<IConsumerMiddleware<TestMessage>>(new ThrowingMiddleware())
            .AddSingleton<IConsumerExceptionHandler<TestMessage>>(handler)
            .BuildServiceProvider();

        await Assert.ThrowsAsync<InvalidOperationException>(() => new ConsumerPipeline(services).ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None));
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

    private sealed class ShortCircuitMiddleware : IConsumerMiddleware<TestMessage>
    {
        public Task<ConsumerResult> InterceptAsync(TestMessage message, ConsumerContext context, ConsumerDelegate nextStep, CancellationToken cancellationToken) => Task.FromResult(ConsumerResult.Ack());
    }

    private sealed class ResultMiddleware : IConsumerMiddleware<TestMessage>
    {
        public Task<ConsumerResult> InterceptAsync(TestMessage message, ConsumerContext context, ConsumerDelegate nextStep, CancellationToken cancellationToken) => Task.FromResult(ConsumerResult.NackRequeue());
    }

    private sealed class ThrowingMiddleware : IConsumerMiddleware<TestMessage>
    {
        public Task<ConsumerResult> InterceptAsync(TestMessage message, ConsumerContext context, ConsumerDelegate nextStep, CancellationToken cancellationToken) => throw new InvalidOperationException();
    }

    private sealed class TokenMiddleware(CancellationToken expected) : IConsumerMiddleware<TestMessage>
    {
        public bool Received { get; private set; }
        public Task<ConsumerResult> InterceptAsync(TestMessage message, ConsumerContext context, ConsumerDelegate nextStep, CancellationToken cancellationToken)
        {
            Received = cancellationToken == expected;
            return nextStep();
        }
    }

    private sealed class TokenConsumer(CancellationToken expected) : IConsumer<TestMessage>
    {
        public bool Received { get; private set; }
        public Task<ConsumerResult> ConsumeAsync(TestMessage message, ConsumerContext context, CancellationToken cancellationToken)
        {
            Received = cancellationToken == expected;
            return Task.FromResult(ConsumerResult.Ack());
        }
    }
}
