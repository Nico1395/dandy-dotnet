using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.TestDoubles;

public sealed class IntegrationMessage : Message
{
    public required string Content { get; init; }
}