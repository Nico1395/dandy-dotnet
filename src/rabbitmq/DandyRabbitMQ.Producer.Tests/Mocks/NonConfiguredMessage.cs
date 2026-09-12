using DandyRabbitMQ.Core.Messages;

namespace DandyRabbitMQ.Producer.Tests.Mocks;

internal sealed class NonConfiguredMessage : Message
{
    public required string Content { get; init; }

    public static NonConfiguredMessage Create()
    {
        return new NonConfiguredMessage { Content = "Hello world!" };
    }
}