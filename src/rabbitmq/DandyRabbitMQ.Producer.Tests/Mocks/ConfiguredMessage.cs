using DandyRabbitMQ.Core.Messages;

namespace DandyRabbitMQ.Producer.Tests.Mocks;

internal sealed class ConfiguredMessage : Message
{
    public required string Content { get; init; }

    public static ConfiguredMessage Create()
    {
        return new ConfiguredMessage { Content = "Hello world!" };
    }
}