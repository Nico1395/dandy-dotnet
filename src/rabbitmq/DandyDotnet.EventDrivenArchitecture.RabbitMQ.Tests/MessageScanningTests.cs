using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class MessageScanningTests
{
    [Fact]
    public void ScanInAssemblies_RegistersAttributedMessageMetadata()
    {
        var configuration = new MessagesConfigurationBuilder()
            .ScanInAssemblies(typeof(MessageScanningTests).Assembly)
            .Build();
        var services = new ServiceCollection();
        services.AddRabbitMQMessages(configuration);
        var result = services.BuildServiceProvider().GetRequiredService<MessagesConfiguration>();

        var message = result.MessagesByRuntimeType[typeof(AttributedMessage)];
        Assert.Same(message, result.MessagesByKey["attributed"]);
        Assert.Equal("exchange", message.Exchange);
        Assert.Equal(["one", "two"], message.RoutingKeys);
        Assert.Equal("value", message.Metadata["metadata"]);
    }

    [MessageKey("attributed")]
    [MessageExchange("exchange")]
    [MessageRoutes("one", "two")]
    [MessageMetadata("metadata", "value")]
    private sealed class AttributedMessage
    {
    }
}
