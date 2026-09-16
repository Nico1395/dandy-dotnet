using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.Encoding.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Connectivity;
using DandyDotnet.Serialization.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;

internal sealed class Producer(
    IServiceProvider serviceProvider,
    IConnectionProvider connectionProvider,
    ProducerConfiguration producerConfiguration,
    MessagesConfiguration messagesConfiguration) : IProducer
{
    private IChannel? _channel;

    private IEncoder? _encoder;
    private ISerializer? _serializer;

    public async Task ProduceAsync(string? exchange, IEnumerable<string>? routingKeys, object message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        var dispatchInfo = DispatchInfo.Create(messagesConfiguration, exchange, routingKeys, message, properties);
        var payload = GetSerializer().Serialize(message, dispatchInfo.RuntimeType);
        var encodedPayload = GetEncoder().Encode(payload);
        var channel = await GetChannelAsync(cancellationToken);

        try
        {
            foreach (var routingKey in dispatchInfo.RoutingKeys)
            {
                await channel.BasicPublishAsync(
                    exchange: dispatchInfo.Exchange,
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: dispatchInfo.Properties,
                    body: encodedPayload,
                    cancellationToken: cancellationToken);
            }
        }
        catch
        {
            _channel = null;
            throw;
        }
    }

    private async Task<IChannel> GetChannelAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
            return _channel;

        var connection = await connectionProvider.GetAsync(cancellationToken);
        return _channel = await connection.CreateChannelAsync(options: null, cancellationToken);
    }

    private IEncoder GetEncoder()
    {
        return _encoder ??= serviceProvider.GetRequiredKeyedOrDefaultService<IEncoder>(producerConfiguration.EncodingConfiguration?.ServiceKey);
    }

    private ISerializer GetSerializer()
    {
        return _serializer ??= serviceProvider.GetRequiredKeyedOrDefaultService<ISerializer>(producerConfiguration.SerializerConfiguration?.ServiceKey);
    }
}
