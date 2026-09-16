using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Producer;

public sealed class DispatchInfoTests
{
    [Theory]
    [InlineData(null, null, "configured-exchange", "configured-key")]
    [InlineData("explicit-exchange", null, "explicit-exchange", "configured-key")]
    [InlineData(null, "explicit-key", "configured-exchange", "explicit-key")]
    [InlineData("explicit-exchange", "explicit-key", "explicit-exchange", "explicit-key")]
    public void Create_WithConfiguredMessage_ExplicitArgumentsOverrideDefaults(
        string? exchange, string? routingKey, string expectedExchange, string expectedRoutingKey)
    {
        var configuration = ConfigureMessage();

        var result = DispatchInfo.Create(configuration, exchange,
            routingKey is null ? null : [routingKey], new TestMessage(), null);

        Assert.Equal(expectedExchange, result.Exchange);
        Assert.Equal([expectedRoutingKey], Assert.IsType<string[]>(result.RoutingKeys));
        Assert.Equal(typeof(TestMessage), result.RuntimeType);
        Assert.Equal("configured-type", result.Properties.Type);
    }

    [Fact]
    public void Create_WithUnconfiguredMessage_UsesExplicitRoutingAndRuntimeTypeName()
    {
        var configuration = new MessagesConfigurationBuilder().Build();

        var result = DispatchInfo.Create(configuration, "exchange", ["key"], new TestMessage(), null);

        Assert.Equal("exchange", result.Exchange);
        Assert.Equal(["key"], Assert.IsType<string[]>(result.RoutingKeys));
        Assert.Equal(typeof(TestMessage), result.RuntimeType);
        Assert.Equal(nameof(TestMessage), result.Properties.Type);
    }

    [Theory]
    [InlineData(null, "key")]
    [InlineData("exchange", null)]
    [InlineData(null, null)]
    public void Create_WithUnconfiguredMessageAndMissingRouting_ThrowsInvalidOperationException(string? exchange, string? routingKey)
    {
        var configuration = new MessagesConfigurationBuilder().Build();

        Assert.Throws<InvalidOperationException>(() => DispatchInfo.Create(configuration, exchange,
            routingKey is null ? null : [routingKey], new TestMessage(), null));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Create_WithBlankExplicitExchange_ThrowsInsteadOfUsingConfiguredExchange(string exchange)
    {
        Assert.Throws<InvalidOperationException>(() =>
            DispatchInfo.Create(ConfigureMessage(), exchange, ["key"], new TestMessage(), null));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Create_WithOnlyEmptyRoutingKeys_ThrowsInsteadOfUsingConfiguredKeys(bool whitespace)
    {
        string[] keys = whitespace ? [" ", "\t", ""] : [];

        Assert.Throws<InvalidOperationException>(() =>
            DispatchInfo.Create(ConfigureMessage(), "exchange", keys, new TestMessage(), null));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Create_NormalizesExplicitAndConfiguredRoutingKeys(bool explicitKeys)
    {
        string[] keys = [" one ", "", "one", "\t", "two", " two "];
        var configuration = new MessagesConfigurationBuilder()
            .AddMessage(typeof(TestMessage), message => message.SetExchange("exchange").SetRoutingKeys(keys))
            .Build();

        var result = DispatchInfo.Create(configuration, null, explicitKeys ? keys : null, new TestMessage(), null);

        Assert.Equal(["one", "two"], Assert.IsType<string[]>(result.RoutingKeys));
    }

    [Theory]
    [InlineData(true, null, "configured-type")]
    [InlineData(false, null, nameof(TestMessage))]
    [InlineData(true, "custom-type", "custom-type")]
    [InlineData(false, "custom-type", "custom-type")]
    [InlineData(true, "", "")]
    public void Create_WithExistingProperties_PreservesInstanceAndExplicitType(bool configured, string? type, string expectedType)
    {
        var configuration = configured ? ConfigureMessage() : new MessagesConfigurationBuilder().Build();
        var properties = new BasicProperties
        {
            Type = type,
            CorrelationId = "correlation",
            Headers = new Dictionary<string, object?> { ["source"] = "test" }
        };

        var result = DispatchInfo.Create(configuration, "exchange", ["key"], new TestMessage(), properties);

        Assert.Same(properties, result.Properties);
        Assert.Equal(expectedType, result.Properties.Type);
        Assert.Equal("correlation", result.Properties.CorrelationId);
        Assert.Equal("test", result.Properties.Headers!["source"]);
    }

    [Fact]
    public void Create_WithNullMessage_CurrentlyThrowsNullReferenceException()
    {
        // Characterizes the current implementation; the documented InvalidOperationException is not thrown.
        Assert.Throws<NullReferenceException>(() =>
            DispatchInfo.Create(ConfigureMessage(), "exchange", ["key"], null!, null));
    }

    [Fact]
    public void Create_WithEmptyConfiguredTypeKey_PreservesEmptyTypeProperty()
    {
        var configuration = new MessagesConfigurationBuilder()
            .AddMessage(typeof(TestMessage), message => message.SetKey(""))
            .Build();

        var result = DispatchInfo.Create(configuration, "exchange", ["key"], new TestMessage(), null);

        Assert.Equal("", result.Properties.Type);
    }

    [Theory]
    [InlineData(null, "key")]
    [InlineData("exchange", null)]
    public void Create_WithIncompleteConfiguration_ThrowsInvalidOperationException(string? exchange, string? routingKey)
    {
        var configuration = new MessagesConfigurationBuilder().AddMessage(typeof(TestMessage), _ => { }).Build();

        Assert.Throws<InvalidOperationException>(() => DispatchInfo.Create(configuration, exchange,
            routingKey is null ? null : [routingKey], new TestMessage(), null));
    }

    private static MessagesConfiguration ConfigureMessage() => new MessagesConfigurationBuilder()
        .AddMessage(typeof(TestMessage), message => message
            .SetKey("configured-type")
            .SetExchange("configured-exchange")
            .SetRoutingKeys("configured-key"))
        .Build();

    private sealed class TestMessage;
}