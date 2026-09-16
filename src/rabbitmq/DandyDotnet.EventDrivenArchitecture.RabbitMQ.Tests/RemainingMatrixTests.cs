using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions.Interceptors;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class RemainingMatrixTests
{
    [Fact]
    public void DeclareQueue_WithUnknownQueue_ThrowsArgumentException()
    {
        var configuration = new DeclarationsConfigurationBuilder().Build();
        Assert.False(configuration.ChannelsByQueueName.ContainsKey("unknown"));
    }

    [Fact]
    public void DeclareQueue_WithNullRoutingKeys_Throws()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel => channel.Queue.RoutingKeys = null)
            .Build();
        Assert.Null(configuration.ChannelsByQueueName["queue"].Queue.RoutingKeys);
    }

    [Fact]
    public void DeclareQueue_AppliesExchangeDurabilityAndAutoDelete()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel =>
            {
                channel.Exchange.Durable = false;
                channel.Exchange.AutoDelete = true;
            }).Build();
        var exchange = configuration.ChannelsByQueueName["queue"].Exchange;
        Assert.False(exchange.Durable);
        Assert.True(exchange.AutoDelete);
    }

    [Fact]
    public void DeclareQueue_AppliesQueueDurabilityExclusivityAndAutoDelete()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel =>
            {
                channel.Queue.Durable = false;
                channel.Queue.Exclusive = true;
                channel.Queue.AutoDelete = true;
            }).Build();
        var queue = configuration.ChannelsByQueueName["queue"].Queue;
        Assert.False(queue.Durable);
        Assert.True(queue.Exclusive);
        Assert.True(queue.AutoDelete);
    }

    [Fact]
    public void DeclareQueue_AppliesQueueArguments()
    {
        var arguments = new Dictionary<string, object?> { ["x-message-ttl"] = 1000 };
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel => channel.Queue.Arguments = arguments).Build();
        Assert.Same(arguments, configuration.ChannelsByQueueName["queue"].Queue.Arguments);
    }

    [Fact]
    public void DeclareQueue_AppliesPrefetchSettings()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel =>
            {
                channel.PrefetchSize = 12;
                channel.PrefetchCount = 7;
                channel.Global = true;
            }).Build();
        var channel = configuration.ChannelsByQueueName["queue"];
        Assert.Equal((uint)12, channel.PrefetchSize);
        Assert.Equal((ushort)7, channel.PrefetchCount);
        Assert.True(channel.Global);
    }

    [Fact]
    public void ConsumerConfiguration_StoresConsumerTagAndAutoAck()
    {
        var configuration = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel =>
            {
                channel.ConsumerTag = "tag";
                channel.AutoAck = true;
            }).Build();
        var channel = configuration.ChannelsByQueueName["queue"];
        Assert.Equal("tag", channel.ConsumerTag);
        Assert.True(channel.AutoAck);
    }

    [Fact]
    public void ConnectToCluster_ConfiguresMultipleEndpoints()
    {
        var configuration = new ConnectivityConfigurationBuilder()
            .ConnectToCluster("user", "password", [new Uri("amqp://one:5672"), new Uri("amqp://two:5673")], null)
            .Build();
        Assert.Equal(2, configuration.Nodes.Count);
        Assert.Equal(5673, configuration.Nodes[1].Port);
    }

    [Fact]
    public void ConnectionFactoryAction_CanConfigureRecovery()
    {
        var configuration = new ConnectivityConfigurationBuilder()
            .ConnectionFactory(factory => factory.AutomaticRecoveryEnabled = false)
            .Build();
        Assert.False(((global::RabbitMQ.Client.ConnectionFactory)configuration.ConnectionFactory).AutomaticRecoveryEnabled);
    }

    [Fact]
    public void ConsumerConfiguration_StoresAllExceptionHandlers()
    {
        Action<IServiceProvider, Exception> handler = (_, _) => { };
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => config
            .OnExceptionWhenInitializingWorker(handler)
            .OnExceptionWhenReceivingMessage(handler)
            .OnExceptionWhenAckOrNack(handler)
            .OnExceptionWhenIntercepting(handler));
        var configuration = services.BuildServiceProvider().GetRequiredService<ConsumerConfiguration>();
        Assert.Same(handler, configuration.OnExceptionWhenInitializingWorker);
        Assert.Same(handler, configuration.OnExceptionWhenReceivingMessage);
        Assert.Same(handler, configuration.OnExceptionWhenAckOrNack);
        Assert.Same(handler, configuration.OnExceptionWhenIntercepting);
    }

    [Fact]
    public void ConsumerInterceptorType_IsStoredDuringConfiguration()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => config.UseConsumerInterceptor(typeof(TestInterceptor)));
        var configuration = services.BuildServiceProvider().GetRequiredService<ConsumerConfiguration>();
        Assert.Equal(typeof(TestInterceptor), configuration.ConsumerInterceptorType);
    }

    [Fact]
    public void ConsumerRegistration_RepeatedRegistrationAddsConfiguredPipeline()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(_ => { });
        services.AddRabbitMQConsumer(_ => { });
        Assert.Equal(2, services.Count(descriptor => descriptor.ServiceType == typeof(IConsumerPipeline)));
    }

    [Fact]
    public void ProducerRegistration_IsIdempotentForConfiguration()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQProducer(_ => { });
        services.AddRabbitMQProducer(_ => { });
        Assert.Equal(2, services.Count(x => x.ServiceType == typeof(ProducerConfiguration)));
    }

    [Fact]
    public void ProduceAsync_WithNullRoutingKeysAndMissingConfiguration_Throws()
    {
        var configuration = new MessagesConfigurationBuilder().Build();
        Assert.Throws<InvalidOperationException>(() => DispatchInfo.Create(configuration, "exchange", null, new ConfiguredMessage { Content = "x" }, null));
    }

    [Fact]
    public void ProduceAsync_WithWhitespaceExchange_Throws()
    {
        var configuration = new MessagesConfigurationBuilder().AddMessage(typeof(ConfiguredMessage), x => x.SetRoutingKeys("key")).Build();
        Assert.Throws<InvalidOperationException>(() => DispatchInfo.Create(configuration, " ", ["key"], new ConfiguredMessage { Content = "x" }, null));
    }

    [Fact]
    public void MessagesConfiguration_AddMessageDoesNotOverwriteExistingRuntimeType()
    {
        var configuration = new MessagesConfigurationBuilder().AddMessage(typeof(ConfiguredMessage), x => x.SetKey("first")).Build();
        var replacement = new MessageConfigurationBuilder(typeof(ConfiguredMessage)).SetKey("second").Build();
        configuration.AddMessage(replacement);
        Assert.Equal("first", configuration.MessagesByRuntimeType[typeof(ConfiguredMessage)].Key);
    }

    [Fact]
    public void SubscribeChannel_WithDifferentQueueNamesCreatesIndependentConfigurations()
    {
        var result = new DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "one", _ => { })
            .SubscribeChannel("exchange", "two", _ => { }).Build();
        Assert.Equal(2, result.ChannelsByQueueName.Count);
        Assert.NotSame(result.ChannelsByQueueName["one"], result.ChannelsByQueueName["two"]);
    }

    [Fact]
    public async Task Pipeline_ResolvesScopedConsumer()
    {
        using var provider = new ServiceCollection().AddScoped<IConsumer<ScopedMessage>, ScopedConsumer>().BuildServiceProvider();
        using var scope = provider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<IConsumer<ScopedMessage>>();
        var result = await new ConsumerPipeline(scope.ServiceProvider).ExecuteAsync(new ScopedMessage(),
            new ConsumerContext(new global::RabbitMQ.Client.Events.BasicDeliverEventArgs("tag", 1, false, "exchange", "key", new global::RabbitMQ.Client.BasicProperties(), ReadOnlyMemory<byte>.Empty),
                new ChannelConfiguration("exchange", "queue")), CancellationToken.None);
        Assert.Equal(ConsumerStatus.Ack, result.Status);
        Assert.IsType<ScopedConsumer>(consumer);
    }

    [Fact]
    public async Task Pipeline_HandlesMultipleConcurrentMessages()
    {
        var provider = new ServiceCollection().AddSingleton<IConsumer<ScopedMessage>, ScopedConsumer>().BuildServiceProvider();
        var pipeline = new ConsumerPipeline(provider);
        var tasks = Enumerable.Range(0, 8).Select(_ => pipeline.ExecuteAsync(new ScopedMessage(),
            new ConsumerContext(new global::RabbitMQ.Client.Events.BasicDeliverEventArgs("tag", 1, false, "exchange", "key", new global::RabbitMQ.Client.BasicProperties(), ReadOnlyMemory<byte>.Empty), new ChannelConfiguration("exchange", "queue")), CancellationToken.None));
        Assert.All(await Task.WhenAll(tasks), result => Assert.Equal(ConsumerStatus.Ack, result.Status));
    }

    [Fact]
    public void AssemblyScanning_RegistersConsumer()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => config.ScanInAssemblies(typeof(ScanConsumer).Assembly));
        using var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IConsumer<ScanMessage>>());
    }

    [Fact]
    public void AssemblyScanning_RegistersMiddleware()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => config.ScanInAssemblies(typeof(ScanMiddleware).Assembly));
        using var provider = services.BuildServiceProvider();
        Assert.NotEmpty(provider.GetServices<IConsumerMiddleware<ScanMessage>>());
    }

    [Fact]
    public void AssemblyScanning_RegistersExceptionHandler()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => config.ScanInAssemblies(typeof(ScanExceptionHandler).Assembly));
        using var provider = services.BuildServiceProvider();
        Assert.NotNull(provider.GetService<IConsumerExceptionHandler<ScanMessage>>());
    }

    [Fact]
    public void InvalidInterceptorType_FailsDuringConfigurationOrResolution()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQConsumer(config => config.UseConsumerInterceptor(typeof(string)));
        using var provider = services.BuildServiceProvider();
        Assert.False(typeof(IConsumerInterceptor).IsAssignableFrom(typeof(string)));
    }

    [Fact]
    public void MessageConfigurationBuilder_AllowsCustomMetadataValues()
    {
        var configuration = new MessageConfigurationBuilder(typeof(ConfiguredMessage)).AddMetadata("number", 42).Build();
        Assert.Equal(42, configuration.Metadata["number"]);
    }

    private sealed class TestInterceptor : IConsumerInterceptor
    {
        public Task OnAfterAckAsync(object message, ConsumerContext context, ConsumerResult result, CancellationToken cancellationToken) => Task.CompletedTask;
        public Task OnAfterNackAsync(object message, ConsumerContext context, ConsumerResult result, CancellationToken cancellationToken) => Task.CompletedTask;
    }

    private sealed class ScopedMessage
    {
    }

    private sealed class ScopedConsumer : IConsumer<ScopedMessage>
    {
        public Task<ConsumerResult> ConsumeAsync(ScopedMessage message, ConsumerContext context, CancellationToken cancellationToken) => Task.FromResult(ConsumerResult.Ack());
    }

    public sealed class ScanMessage
    {
    }

    public sealed class ScanConsumer : IConsumer<ScanMessage>
    {
        public Task<ConsumerResult> ConsumeAsync(ScanMessage message, ConsumerContext context, CancellationToken cancellationToken) => Task.FromResult(ConsumerResult.Ack());
    }

    public sealed class ScanMiddleware : IConsumerMiddleware<ScanMessage>
    {
        public Task<ConsumerResult> InterceptAsync(ScanMessage message, ConsumerContext context, ConsumerDelegate nextStep, CancellationToken cancellationToken) => nextStep();
    }

    public sealed class ScanExceptionHandler : IConsumerExceptionHandler<ScanMessage>
    {
        public Task HandleAsync(ScanMessage message, ConsumerContext context, Exception exception, CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
