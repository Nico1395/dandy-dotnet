using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Mocks;

internal sealed class ConfiguredMessage : Message
{
    public required string Content { get; init; }

    public static ConfiguredMessage Create()
    {
        return new ConfiguredMessage { Content = "Hello world!" };
    }
}