using DandyDotnet.Tests.Core.Fixtures;
using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Mocks;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Testcontainers.RabbitMq;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;

public sealed class IntegrationFixture : Fixture
{
    private RabbitMqContainer? _rabbitMq;
    private IHostedService? _consumerWorker;

    public string ExchangeName { get; } = $"tests-{Guid.NewGuid():N}";
    public string QueueName { get; } = $"tests-{Guid.NewGuid():N}";
    public string RoutingKey { get; } = "messages";

    protected override void ConfigureServices(IServiceCollection services)
    {
        _rabbitMq = new RabbitMqBuilder("rabbitmq:4-management")
            .WithUsername("tests")
            .WithPassword("tests")
            .Build();
        _rabbitMq.StartAsync().GetAwaiter().GetResult();
        services.AddSerializer(config => config.UseSystemTextJson());
        services.AddEncoder();

        services.AddRabbitMQConsumer(config =>
        {
            ConfigureConnection(config.Connectivity);
            config.OnExceptionWhenInitializingWorker((_, exception) => Console.WriteLine($"RabbitMQ consumer startup failed: {exception}"));
            config.Messages.AddMessage(typeof(IntegrationMessage), message =>
            {
                message.SetKey(nameof(IntegrationMessage));
                message.SetExchange(ExchangeName);
                message.SetRoutingKeys(RoutingKey);
            });
            config.Declarations.SubscribeChannel(ExchangeName, QueueName, channel =>
            {
                channel.Queue.RoutingKeys = [RoutingKey];
                channel.Queue.Arguments = new Dictionary<string, object?>();
            });
            config.ScanInAssemblies(typeof(IntegrationFixture).Assembly);
        });

        services.AddRabbitMQProducer(config =>
        {
            ConfigureConnection(config.Connectivity);
            config.Messages.AddMessage(typeof(IntegrationMessage), message =>
            {
                message.SetKey(nameof(IntegrationMessage));
                message.SetExchange(ExchangeName);
                message.SetRoutingKeys(RoutingKey);
            });
            config.Declarations.SubscribeChannel(ExchangeName, QueueName, channel =>
            {
                channel.Queue.RoutingKeys = [RoutingKey];
                channel.Queue.Arguments = new Dictionary<string, object?>();
            });
        });
    }

    protected override async Task OnInitializeAsync()
    {
        _consumerWorker = ServiceProvider.GetServices<IHostedService>().Single();
        await _consumerWorker.StartAsync(CancellationToken.None);

        var timeout = DateTime.UtcNow.AddSeconds(10);
        while (DateTime.UtcNow < timeout)
        {
            try
            {
                var connection = await GetConnectionProvider().GetAsync(CancellationToken.None);
                await using var channel = await connection.CreateChannelAsync();
                await channel.QueueDeclarePassiveAsync(QueueName);
                return;
            }
            catch (Exception)
            {
                await Task.Delay(50);
            }
        }

        throw new TimeoutException("The integration consumer did not declare its queue.");
    }

    public IProducer CreateProducer() => ServiceProvider.GetRequiredService<IProducer>();

    public IServiceScope CreateProducerScope() => ServiceProvider.CreateScope();

    public IConnectionProvider GetConnectionProvider() => ServiceProvider.GetRequiredService<IConnectionProvider>();

    public async Task<uint> GetQueueMessageCountAsync()
    {
        var connection = await GetConnectionProvider().GetAsync(CancellationToken.None);
        await using var channel = await connection.CreateChannelAsync();
        return (await channel.QueueDeclarePassiveAsync(QueueName)).MessageCount;
    }

    public async Task<ProbeQueue> CreateProbeQueueAsync(params string[] routingKeys)
    {
        var connection = await GetConnectionProvider().GetAsync(CancellationToken.None);
        var channel = await connection.CreateChannelAsync();
        await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true, autoDelete: false);
        var queue = await channel.QueueDeclareAsync("", durable: false, exclusive: true, autoDelete: true);

        foreach (var routingKey in routingKeys.Distinct())
            await channel.QueueBindAsync(queue.QueueName, ExchangeName, routingKey);

        return new ProbeQueue(channel, queue.QueueName);
    }

    public sealed class ProbeQueue(IChannel channel, string queueName) : IAsyncDisposable
    {
        public IChannel Channel { get; } = channel;
        public string QueueName { get; } = queueName;
        public ValueTask DisposeAsync() => Channel.DisposeAsync();
    }

    public override async Task DisposeAsync()
    {
        if (_consumerWorker is not null)
            await _consumerWorker.StopAsync(CancellationToken.None);

        await base.DisposeAsync();
        if (_rabbitMq is not null)
            await _rabbitMq.DisposeAsync();
    }

    private void ConfigureConnection(ConnectivityConfigurationBuilder config)
    {
        config.ConnectToCluster(
            "tests",
            "tests",
            [new Uri($"amqp://localhost:{_rabbitMq!.GetMappedPublicPort(5672)}")],
            TimeSpan.FromSeconds(1));
    }
}
