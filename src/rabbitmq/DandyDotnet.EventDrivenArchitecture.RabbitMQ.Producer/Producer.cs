using DandyDotnet.Encoding.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Connectivity;
using DandyDotnet.Serialization.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;

internal sealed class Producer(
    ISerializer payloadSerializer,
    IEncoder encoder,
    IConnectionProvider connectionProvider,
    MessagesConfiguration messagesConfiguration) : IProducer
{
    private IChannel? _channel;

    public async Task ProduceAsync(string? exchange, IEnumerable<string>? routingKeys, object message, BasicProperties? properties, CancellationToken cancellationToken)
    {
        var dispatchInfo = DispatchInfo.Create(messagesConfiguration, exchange, routingKeys, message, properties);
        var payload = payloadSerializer.Serialize(message, dispatchInfo.RuntimeType);
        var encodedPayload = encoder.Encode(payload);
        var channel = await GetChannelAsync(cancellationToken);

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

    private async Task<IChannel> GetChannelAsync(CancellationToken cancellationToken)
    {
        if (_channel != null)
            return _channel;

        var connection = await connectionProvider.GetAsync(cancellationToken);
        return _channel = await connection.CreateChannelAsync(options: null, cancellationToken);
    }
}
