namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Declarations;

public interface IReadOnlyExchangeConfiguration
{
    string Name { get; }
    string ExchangeType { get; }
    bool Durable { get; }
    bool AutoDelete { get; }
}