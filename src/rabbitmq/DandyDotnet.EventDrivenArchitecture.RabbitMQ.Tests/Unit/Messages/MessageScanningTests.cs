using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Messages;

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
        using var provider = services.BuildServiceProvider();
        var result = provider.GetRequiredService<MessagesConfiguration>();

        var message = result.MessagesByRuntimeType[typeof(AttributedMessage)];
        Assert.Same(message, result.MessagesByKey["attributed"]);
        Assert.Equal("exchange", message.Exchange);
        Assert.Equal(["one", "two"], Assert.IsType<string[]>(message.RoutingKeys));
        Assert.Equal("value", message.Metadata["metadata"]);
        Assert.DoesNotContain(nameof(AttributedMessage), result.MessagesByKey.Keys);
    }

    [Fact]
    public void AddRabbitMQMessages_WithManualConfigurationAndScanning_AppliesAttributedRouting()
    {
        var services = new ServiceCollection();
        services.AddRabbitMQMessages(builder => builder
            .AddMessage(typeof(AttributedMessage), message => message.SetExchange("manual-exchange").SetRoutingKeys("manual-key"))
            .ScanInAssemblies(typeof(MessageScanningTests).Assembly));
        using var provider = services.BuildServiceProvider();

        var configuration = provider.GetRequiredService<MessagesConfiguration>();

        var message = configuration.MessagesByRuntimeType[typeof(AttributedMessage)];
        Assert.Equal("exchange", message.Exchange);
        Assert.Equal(["one", "two"], Assert.IsType<string[]>(message.RoutingKeys));
        Assert.Equal("attributed", message.Key);
        Assert.Same(message, configuration.MessagesByKey["attributed"]);
        Assert.DoesNotContain(nameof(AttributedMessage), configuration.MessagesByKey.Keys);
    }

    [MessageKey("attributed")]
    [MessageExchange("exchange")]
    [MessageRoutes("one", "two")]
    [MessageMetadata("metadata", "value")]
    private sealed class AttributedMessage
    {
    }
}