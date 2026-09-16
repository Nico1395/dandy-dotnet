using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions.Interceptors;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Consumer;

public sealed class ReceiverTests
{
    [Theory]
    [InlineData("Ack", false, false)]
    [InlineData("AckMultiple", false, true)]
    [InlineData("Nack", false, false)]
    [InlineData("NackRequeue", true, false)]
    [InlineData("NackMultiple", false, true)]
    [InlineData("NackRequeueMultiple", true, true)]
    public async Task ReceiveAsync_ForwardsResultFlagsAndDeliveryToChannelAndInterceptor(string factory, bool requeue, bool multiple)
    {
        var expected = CreateResult(factory);
        var pipeline = new RecordingPipeline { Result = expected };
        var interceptor = new RecordingInterceptor();
        using var provider = CreateServices(interceptor);
        var receiver = new Receiver(new ConsumerConfiguration(), Messages(), provider, pipeline);
        var (channel, recording) = RecordingChannel.Create();
        using var ackLock = new SemaphoreSlim(1);
        using var cancellation = new CancellationTokenSource();
        var args = Delivery(nameof(TestMessage), "{\"Content\":\"hello\"}");
        var configuration = new ChannelConfiguration("exchange", "queue");

        await receiver.ReceiveAsync(args, ackLock, channel, configuration, cancellation.Token);

        var call = Assert.Single(recording.Calls);
        var ack = factory.StartsWith("Ack", StringComparison.Ordinal);
        Assert.Equal(ack ? nameof(IChannel.BasicAckAsync) : nameof(IChannel.BasicNackAsync), call.Method);
        Assert.Equal(42UL, call.Arguments["deliveryTag"]);
        Assert.Equal(multiple, call.Arguments["multiple"]);
        if (!ack)
            Assert.Equal(requeue, call.Arguments["requeue"]);
        Assert.Equal(cancellation.Token, call.Arguments["cancellationToken"]);
        Assert.Equal(1, pipeline.Calls);
        Assert.Equal("hello", Assert.IsType<TestMessage>(pipeline.Message).Content);
        Assert.Same(args, pipeline.Context!.DeliverArgs);
        Assert.Same(configuration, pipeline.Context.ChannelConfiguration);
        Assert.Equal(cancellation.Token, pipeline.Token);
        Assert.Equal(1, interceptor.Calls);
        Assert.Equal(ack, interceptor.WasAck);
        Assert.Same(pipeline.Message, interceptor.Message);
        Assert.Same(pipeline.Context, interceptor.Context);
        Assert.Same(expected, interceptor.Result);
        Assert.Equal(cancellation.Token, interceptor.Token);
        Assert.Equal(1, ackLock.CurrentCount);
    }

    [Theory]
    [InlineData(null, "{}", typeof(InvalidOperationException))]
    [InlineData("", "{}", typeof(InvalidOperationException))]
    [InlineData("unknown-type", "{}", typeof(InvalidOperationException))]
    [InlineData(nameof(TestMessage), "", typeof(InvalidOperationException))]
    [InlineData(nameof(TestMessage), "not-json", typeof(System.Text.Json.JsonException))]
    public async Task ReceiveAsync_WithInvalidDelivery_NacksWithoutInvokingPipelineOrInterceptor(string? type, string body, Type exceptionType)
    {
        var pipeline = new RecordingPipeline();
        var interceptor = new RecordingInterceptor();
        var failures = new List<Exception>();
        using var provider = CreateServices(interceptor);
        var configuration = ConfigureConsumer(builder => builder.OnExceptionWhenReceivingMessage((_, exception) => failures.Add(exception)));
        var receiver = new Receiver(configuration, Messages(), provider, pipeline);
        var (channel, recording) = RecordingChannel.Create();
        using var ackLock = new SemaphoreSlim(1);

        await receiver.ReceiveAsync(Delivery(type, body), ackLock, channel,
            new ChannelConfiguration("exchange", "queue"), CancellationToken.None);

        var call = Assert.Single(recording.Calls);
        Assert.Equal(nameof(IChannel.BasicNackAsync), call.Method);
        Assert.Equal(42UL, call.Arguments["deliveryTag"]);
        Assert.Equal(false, call.Arguments["multiple"]);
        Assert.Equal(false, call.Arguments["requeue"]);
        Assert.IsType(exceptionType, Assert.Single(failures));
        Assert.Equal(0, pipeline.Calls);
        Assert.Equal(0, interceptor.Calls);
    }

    [Fact]
    public async Task ReceiveAsync_WhenPipelineFails_ReportsOriginalExceptionAndNacksWithoutRequeue()
    {
        var exception = new InvalidOperationException("consumer failure");
        var pipeline = new RecordingPipeline { Exception = exception };
        var failures = new List<Exception>();
        var interceptor = new RecordingInterceptor();
        using var provider = CreateServices(interceptor);
        var receiver = new Receiver(ConfigureConsumer(builder => builder.OnExceptionWhenReceivingMessage((_, error) => failures.Add(error))), Messages(), provider, pipeline);
        var (channel, recording) = RecordingChannel.Create();
        using var ackLock = new SemaphoreSlim(1);

        await receiver.ReceiveAsync(Delivery(nameof(TestMessage), "{}"), ackLock, channel,
            new ChannelConfiguration("exchange", "queue"), CancellationToken.None);

        Assert.Same(exception, Assert.Single(failures));
        var call = Assert.Single(recording.Calls);
        Assert.Equal(nameof(IChannel.BasicNackAsync), call.Method);
        Assert.Equal(false, call.Arguments["requeue"]);
        Assert.Equal(false, call.Arguments["multiple"]);
        Assert.Equal(1, interceptor.Calls);
        Assert.False(interceptor.WasAck);
    }

    [Fact]
    public async Task ReceiveAsync_WhenAcknowledgementFails_ReportsExceptionAndReleasesLock()
    {
        var exception = new InvalidOperationException("channel failure");
        var failures = new List<Exception>();
        using var provider = CreateServices(new RecordingInterceptor());
        var receiver = new Receiver(ConfigureConsumer(builder => builder.OnExceptionWhenAckOrNack((_, error) => failures.Add(error))), Messages(), provider, new RecordingPipeline());
        var (channel, recording) = RecordingChannel.Create();
        recording.AcknowledgementException = exception;
        using var ackLock = new SemaphoreSlim(1);

        await receiver.ReceiveAsync(Delivery(nameof(TestMessage), "{}"), ackLock, channel,
            new ChannelConfiguration("exchange", "queue"), CancellationToken.None);

        Assert.Same(exception, Assert.Single(failures));
        Assert.Single(recording.Calls);
        Assert.Equal(1, ackLock.CurrentCount);
    }

    private static ConsumerConfiguration ConfigureConsumer(Action<ConsumerConfigurationBuilder> configure)
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(configure);
        using var provider = services.BuildServiceProvider();
        return provider.GetRequiredService<ConsumerConfiguration>();
    }

    private static ConsumerResult CreateResult(string factory) => factory switch
    {
        "Ack" => ConsumerResult.Ack(),
        "AckMultiple" => ConsumerResult.AckMultiple(),
        "Nack" => ConsumerResult.Nack(),
        "NackRequeue" => ConsumerResult.NackRequeue(),
        "NackMultiple" => ConsumerResult.NackMultiple(),
        "NackRequeueMultiple" => ConsumerResult.NackRequeueMultiple(),
        _ => throw new ArgumentOutOfRangeException(nameof(factory))
    };

    private static ServiceProvider CreateServices(RecordingInterceptor interceptor)
    {
        var services = new ServiceCollection();
        services.AddEncoder();
        services.AddSerializer(configuration => configuration.UseSystemTextJson());
        services.AddSingleton<IConsumerInterceptor>(interceptor);
        return services.BuildServiceProvider();
    }

    private static MessagesConfiguration Messages() => new MessagesConfigurationBuilder()
        .AddMessage(typeof(TestMessage), _ => { }).Build();

    private static BasicDeliverEventArgs Delivery(string? type, string body) => new("consumer", 42, false,
        "exchange", "routing", new BasicProperties { Type = type }, global::System.Text.Encoding.UTF8.GetBytes(body));

    public sealed class TestMessage
    {
        public string? Content { get; set; }
    }

    private sealed class RecordingPipeline : IConsumerPipeline
    {
        public ConsumerResult Result { get; init; } = ConsumerResult.Ack();
        public Exception? Exception { get; init; }
        public int Calls { get; private set; }
        public object? Message { get; private set; }
        public ConsumerContext? Context { get; private set; }
        public CancellationToken Token { get; private set; }

        public Task<ConsumerResult> ExecuteAsync<TMessage>(TMessage message, ConsumerContext context, CancellationToken cancellationToken)
        {
            Calls++;
            Message = message;
            Context = context;
            Token = cancellationToken;
            return Exception is null ? Task.FromResult(Result) : Task.FromException<ConsumerResult>(Exception);
        }
    }

    private sealed class RecordingInterceptor : IConsumerInterceptor
    {
        public int Calls { get; private set; }
        public bool WasAck { get; private set; }
        public object? Message { get; private set; }
        public ConsumerContext? Context { get; private set; }
        public ConsumerResult? Result { get; private set; }
        public CancellationToken Token { get; private set; }

        public Task OnAfterAckAsync(object message, ConsumerContext context, ConsumerResult result, CancellationToken cancellationToken) =>
            Record(true, message, context, result, cancellationToken);

        public Task OnAfterNackAsync(object message, ConsumerContext context, ConsumerResult result, CancellationToken cancellationToken) =>
            Record(false, message, context, result, cancellationToken);

        private Task Record(bool ack, object message, ConsumerContext context, ConsumerResult result, CancellationToken token)
        {
            Calls++;
            WasAck = ack;
            Message = message;
            Context = context;
            Result = result;
            Token = token;
            return Task.CompletedTask;
        }
    }
}