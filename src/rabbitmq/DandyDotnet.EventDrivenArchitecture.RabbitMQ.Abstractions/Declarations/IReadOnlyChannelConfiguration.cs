using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Declarations;

public interface IReadOnlyChannelConfiguration
{
    CreateChannelOptions? Options { get; }
    IReadOnlyExchangeConfiguration Exchange { get; }
    IReadOnlyQueueConfiguration Queue { get; }
    uint PrefetchSize { get; }
    ushort PrefetchCount { get; }
    bool Global { get; }
    bool AutoAck { get; }
    public string? ConsumerTag { get; set; }
}