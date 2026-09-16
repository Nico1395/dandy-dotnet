using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Consumer;

public sealed class ConsumerPipelineTests
{
    [Fact]
    public async Task ExecuteAsync_ForwardsMessageContextTokenAndConsumerResult()
    {
        var message = new TestMessage();
        var context = CreateContext();
        using var cancellation = new CancellationTokenSource();
        var expected = ConsumerResult.NackRequeueMultiple();
        var calls = 0;
        var consumer = new DelegateConsumer((actualMessage, actualContext, token) =>
        {
            calls++;
            Assert.Same(message, actualMessage);
            Assert.Same(context, actualContext);
            Assert.Equal(cancellation.Token, token);
            return Task.FromResult(expected);
        });
        using var provider = new ServiceCollection().AddSingleton<IConsumer<TestMessage>>(consumer).BuildServiceProvider();

        var result = await new ConsumerPipeline(provider).ExecuteAsync(message, context, cancellation.Token);

        Assert.Equal(1, calls);
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ExecuteAsync_WithMiddleware_ExecutesInRegistrationOrderAndUnwindsInReverse()
    {
        var calls = new List<string>();
        var message = new TestMessage();
        var context = CreateContext();
        using var cancellation = new CancellationTokenSource();
        var expected = ConsumerResult.NackRequeue();
        var services = new ServiceCollection().AddSingleton<IConsumer<TestMessage>>(new DelegateConsumer((_, _, _) =>
        {
            calls.Add("consumer");
            return Task.FromResult(expected);
        }));
        foreach (var name in new[] { "first", "second" })
        {
            services.AddSingleton<IConsumerMiddleware<TestMessage>>(new DelegateMiddleware(async (actualMessage, actualContext, next, token) =>
            {
                Assert.Same(message, actualMessage);
                Assert.Same(context, actualContext);
                Assert.Equal(cancellation.Token, token);
                calls.Add($"{name}-before");
                var result = await next();
                calls.Add($"{name}-after");
                return result;
            }));
        }

        using var provider = services.BuildServiceProvider();

        var result = await new ConsumerPipeline(provider).ExecuteAsync(message, context, cancellation.Token);

        Assert.Equal(["first-before", "second-before", "consumer", "second-after", "first-after"], calls);
        Assert.Same(expected, result);
    }

    [Fact]
    public async Task ExecuteAsync_WhenMiddlewareShortCircuits_SkipsRemainingMiddlewareAndConsumer()
    {
        var consumerCalls = 0;
        var downstreamCalls = 0;
        var expected = ConsumerResult.NackRequeueMultiple();
        using var provider = new ServiceCollection()
            .AddSingleton<IConsumer<TestMessage>>(new DelegateConsumer((_, _, _) =>
            {
                consumerCalls++;
                return Task.FromResult(ConsumerResult.Ack());
            }))
            .AddSingleton<IConsumerMiddleware<TestMessage>>(new DelegateMiddleware((_, _, _, _) => Task.FromResult(expected)))
            .AddSingleton<IConsumerMiddleware<TestMessage>>(new DelegateMiddleware((_, _, next, _) =>
            {
                downstreamCalls++;
                return next();
            }))
            .BuildServiceProvider();

        var result = await new ConsumerPipeline(provider).ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None);

        Assert.Same(expected, result);
        Assert.Equal(0, downstreamCalls);
        Assert.Equal(0, consumerCalls);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ExecuteAsync_WhenConsumerOrMiddlewareFails_ForwardsOriginalExceptionAndRethrows(bool middlewareFails)
    {
        var exception = new InvalidOperationException("pipeline failure");
        var message = new TestMessage();
        var context = CreateContext();
        using var cancellation = new CancellationTokenSource();
        var handler = new RecordingExceptionHandler();
        var consumerCalls = 0;
        var services = new ServiceCollection()
            .AddSingleton<IConsumerExceptionHandler<TestMessage>>(handler)
            .AddSingleton<IConsumer<TestMessage>>(new DelegateConsumer((_, _, _) =>
            {
                consumerCalls++;
                return Task.FromException<ConsumerResult>(exception);
            }));
        if (middlewareFails)
            services.AddSingleton<IConsumerMiddleware<TestMessage>>(new DelegateMiddleware((_, _, _, _) => Task.FromException<ConsumerResult>(exception)));
        using var provider = services.BuildServiceProvider();

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new ConsumerPipeline(provider).ExecuteAsync(message, context, cancellation.Token));

        Assert.Same(exception, thrown);
        Assert.Equal(middlewareFails ? 0 : 1, consumerCalls);
        Assert.Equal(1, handler.Calls);
        Assert.Same(exception, handler.Exception);
        Assert.Same(message, handler.Message);
        Assert.Same(context, handler.Context);
        Assert.Equal(cancellation.Token, handler.Token);
    }

    [Fact]
    public async Task ExecuteAsync_WithNoConsumer_ReportsResolutionFailureToHandler()
    {
        var handler = new RecordingExceptionHandler();
        using var provider = new ServiceCollection().AddSingleton<IConsumerExceptionHandler<TestMessage>>(handler).BuildServiceProvider();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new ConsumerPipeline(provider).ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None));

        Assert.Same(exception, handler.Exception);
        Assert.Equal(1, handler.Calls);
    }

    [Fact]
    public async Task ExecuteAsync_WithoutExceptionHandler_PropagatesOriginalException()
    {
        var exception = new InvalidOperationException("unhandled");
        using var provider = new ServiceCollection().AddSingleton<IConsumer<TestMessage>>(
            new DelegateConsumer((_, _, _) => Task.FromException<ConsumerResult>(exception))).BuildServiceProvider();

        var thrown = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new ConsumerPipeline(provider).ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None));

        Assert.Same(exception, thrown);
    }

    [Fact]
    public async Task ExecuteAsync_WithScopedProvider_InvokesConsumerFromThatScope()
    {
        using var provider = new ServiceCollection()
            .AddScoped<ScopedConsumer>()
            .AddScoped<IConsumer<TestMessage>>(services => services.GetRequiredService<ScopedConsumer>())
            .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        using var first = provider.CreateScope();
        using var second = provider.CreateScope();
        var firstConsumer = first.ServiceProvider.GetRequiredService<ScopedConsumer>();
        var secondConsumer = second.ServiceProvider.GetRequiredService<ScopedConsumer>();

        await new ConsumerPipeline(first.ServiceProvider).ExecuteAsync(new TestMessage(), CreateContext(), CancellationToken.None);

        Assert.Equal(1, firstConsumer.Calls);
        Assert.Equal(0, secondConsumer.Calls);
        Assert.NotSame(firstConsumer, secondConsumer);
    }

    [Fact]
    public async Task ExecuteAsync_WithOverlappingMessages_KeepsEachInvocationIndependent()
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var messages = Enumerable.Range(0, 8).Select(_ => new TestMessage()).ToArray();
        var received = new System.Collections.Concurrent.ConcurrentBag<TestMessage>();
        var allStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var started = 0;
        using var provider = new ServiceCollection().AddSingleton<IConsumer<TestMessage>>(
            new DelegateConsumer(async (message, _, _) =>
            {
                received.Add(message);
                if (Interlocked.Increment(ref started) == messages.Length)
                    allStarted.TrySetResult();
                await release.Task;
                return ConsumerResult.Ack();
            })).BuildServiceProvider();
        var pipeline = new ConsumerPipeline(provider);
        var tasks = messages.Select(message => pipeline.ExecuteAsync(message, CreateContext(), CancellationToken.None)).ToArray();

        try
        {
            await allStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
            Assert.Equal(messages.Length, received.Count);
            Assert.All(messages, message => Assert.Contains(message, received));
            Assert.All(tasks, task => Assert.False(task.IsCompleted));
        }
        finally
        {
            release.SetResult();
        }

        var results = await Task.WhenAll(tasks).WaitAsync(TimeSpan.FromSeconds(5));

        Assert.All(results, result =>
        {
            Assert.Equal(ConsumerStatus.Ack, result.Status);
            Assert.False(result.Multiple);
            Assert.False(result.Requeue);
        });
    }

    private static ConsumerContext CreateContext() => new(
        new BasicDeliverEventArgs("consumer", 1, false, "exchange", "routing", new BasicProperties(), ReadOnlyMemory<byte>.Empty),
        new ChannelConfiguration("exchange", "queue"));

    private sealed class TestMessage;

    private sealed class DelegateConsumer(Func<TestMessage, ConsumerContext, CancellationToken, Task<ConsumerResult>> consume) : IConsumer<TestMessage>
    {
        public Task<ConsumerResult> ConsumeAsync(TestMessage message, ConsumerContext context, CancellationToken cancellationToken) => consume(message, context, cancellationToken);
    }

    private sealed class DelegateMiddleware(Func<TestMessage, ConsumerContext, ConsumerDelegate, CancellationToken, Task<ConsumerResult>> intercept) : IConsumerMiddleware<TestMessage>
    {
        public Task<ConsumerResult> InterceptAsync(TestMessage message, ConsumerContext context, ConsumerDelegate nextStep, CancellationToken cancellationToken) => intercept(message, context, nextStep, cancellationToken);
    }

    private sealed class ScopedConsumer : IConsumer<TestMessage>
    {
        public int Calls { get; private set; }

        public Task<ConsumerResult> ConsumeAsync(TestMessage message, ConsumerContext context, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(ConsumerResult.Ack());
        }
    }

    private sealed class RecordingExceptionHandler : IConsumerExceptionHandler<TestMessage>
    {
        public int Calls { get; private set; }
        public TestMessage? Message { get; private set; }
        public ConsumerContext? Context { get; private set; }
        public Exception? Exception { get; private set; }
        public CancellationToken Token { get; private set; }

        public Task HandleAsync(TestMessage message, ConsumerContext context, Exception exception, CancellationToken cancellationToken)
        {
            Calls++;
            Message = message;
            Context = context;
            Exception = exception;
            Token = cancellationToken;
            return Task.CompletedTask;
        }
    }
}