using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Sample.Shared;

[MessageKey("InputMessageV1")]
[MessageExchange("messages")]
[MessageRoutes("all")]
public sealed class InputMessageV1 : Message
{
    public required string Text { get; init; }
}