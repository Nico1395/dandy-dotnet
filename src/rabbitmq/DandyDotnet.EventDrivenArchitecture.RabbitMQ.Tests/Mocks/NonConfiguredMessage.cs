using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Mocks;

internal sealed class NonConfiguredMessage : Message
{
    public required string Content { get; init; }

    public static NonConfiguredMessage Create()
    {
        return new NonConfiguredMessage { Content = "Hello world!" };
    }
}