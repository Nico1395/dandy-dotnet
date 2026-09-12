namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Declarations;

public interface IReadOnlyQueueConfiguration
{
    string Name { get; }
    string[]? RoutingKeys { get; }
    bool Durable { get; }
    bool Exclusive { get; }
    bool AutoDelete { get; }
    bool NoWait { get; }
    IReadOnlyDictionary<string, object?> Arguments { get; }
}