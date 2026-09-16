using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class MessageConfigurationBuilderTests
{
    [Fact]
    public void Build_DefaultsKeyToRuntimeTypeName()
    {
        var configuration = new MessageConfigurationBuilder(typeof(TestMessage)).Build();

        Assert.Equal(nameof(TestMessage), configuration.Key);
        Assert.Equal(typeof(TestMessage), configuration.RuntimeType);
    }

    [Fact]
    public void Build_StoresConfiguredMessageValues()
    {
        var configuration = new MessageConfigurationBuilder(typeof(TestMessage))
            .SetKey("test")
            .SetExchange("exchange")
            .SetRoutingKeys("one", "two")
            .AddMetadata("answer", 42)
            .Build();

        Assert.Equal("test", configuration.Key);
        Assert.Equal("exchange", configuration.Exchange);
        Assert.Equal(["one", "two"], configuration.RoutingKeys);
        Assert.Equal(42, configuration.Metadata["answer"]);
    }

    [Fact]
    public void AddMetadata_WithDuplicateKey_ReplacesValue()
    {
        var configuration = new MessageConfigurationBuilder(typeof(TestMessage))
            .AddMetadata("key", "first")
            .AddMetadata("key", "second")
            .Build();

        Assert.Equal("second", configuration.Metadata["key"]);
    }

    private sealed class TestMessage
    {
    }
}
