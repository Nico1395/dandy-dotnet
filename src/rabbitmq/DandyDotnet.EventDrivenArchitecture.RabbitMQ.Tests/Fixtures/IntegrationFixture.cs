using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Testcontainers.RabbitMq;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;

public sealed class IntegrationFixture : IAsyncLifetime
{
    private readonly RabbitMqContainer _rabbitMq = new RabbitMqBuilder("rabbitmq:4-management")
        .WithUsername("tests").WithPassword("tests").Build();

    private ServiceProvider? _provider;
    private IHostedService? _worker;

    public string ExchangeName { get; } = $"tests-{Guid.NewGuid():N}";
    public string QueueName { get; } = $"tests-{Guid.NewGuid():N}";
    public string RoutingKey => "messages";
    public IntegrationMessageConsumer Consumer { get; } = new();

    public async Task InitializeAsync()
    {
        try
        {
            await _rabbitMq.StartAsync();
            var services = new ServiceCollection();
            services.AddSerializer(config => config.UseSystemTextJson());
            services.AddEncoder();
            services.AddSingleton<IConsumer<IntegrationMessage>>(Consumer);
            services.AddRabbitMQConsumer(config =>
            {
                ConfigureConnection(config.Connectivity);
                config.Messages.AddMessage(typeof(IntegrationMessage), message => message
                    .SetKey(nameof(IntegrationMessage)).SetExchange(ExchangeName).SetRoutingKeys(RoutingKey));
                config.Declarations.SubscribeChannel(ExchangeName, QueueName, channel =>
                {
                    channel.Queue.RoutingKeys = [RoutingKey];
                    channel.Queue.Arguments = new Dictionary<string, object?>();
                });
            });
            services.AddRabbitMQProducer(config => ConfigureConnection(config.Connectivity));
            _provider = services.BuildServiceProvider();

            // Declare before starting the worker so readiness checks never close a channel on a missing queue.
            var connection = await GetConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();
            await _provider.GetRequiredService<DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Declarations.IDeclarer>()
                .DeclareQueueAsync(QueueName, channel, CancellationToken.None);
            _worker = _provider.GetServices<IHostedService>().Single();
            await _worker.StartAsync(CancellationToken.None);
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            while ((await channel.QueueDeclarePassiveAsync(QueueName, timeout.Token)).ConsumerCount == 0)
                await Task.Delay(25, timeout.Token);
        }
        catch
        {
            await DisposeAsync();
            throw;
        }
    }

    public IServiceScope CreateProducerScope() => _provider!.CreateScope();

    public async Task<ProbeQueue> CreateProbeQueueAsync(params string[] routingKeys)
    {
        var connection = await GetConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        try
        {
            var queue = await channel.QueueDeclareAsync("", durable: false, exclusive: true, autoDelete: true);
            foreach (var routingKey in routingKeys.Distinct())
                await channel.QueueBindAsync(queue.QueueName, ExchangeName, routingKey);
            return new ProbeQueue(channel, queue.QueueName);
        }
        catch
        {
            await channel.DisposeAsync();
            throw;
        }
    }

    public sealed class ProbeQueue(IChannel channel, string queueName) : IAsyncDisposable
    {
        public IChannel Channel { get; } = channel;
        public string QueueName { get; } = queueName;
        public ValueTask DisposeAsync() => Channel.DisposeAsync();
    }

    public async Task DisposeAsync()
    {
        try
        {
            if (_worker is not null)
            {
                using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                await _worker.StopAsync(timeout.Token);
                _worker = null;
            }
        }
        finally
        {
            try
            {
                if (_provider is not null)
                {
                    await _provider.DisposeAsync();
                    _provider = null;
                }
            }
            finally
            {
                await _rabbitMq.DisposeAsync();
            }
        }
    }

    private Task<IConnection> GetConnectionAsync() => _provider!.GetRequiredService<IConnectionProvider>().GetAsync(CancellationToken.None);

    private void ConfigureConnection(ConnectivityConfigurationBuilder config) => config.ConnectToCluster(
        "tests", "tests", [new Uri($"amqp://{_rabbitMq.Hostname}:{_rabbitMq.GetMappedPublicPort(5672)}")], TimeSpan.FromSeconds(1));
}