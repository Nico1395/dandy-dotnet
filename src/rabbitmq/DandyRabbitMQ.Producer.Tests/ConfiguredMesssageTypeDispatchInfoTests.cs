using DandyRabbitMQ.Producer.Tests.Fixtures;
using DandyRabbitMQ.Producer.Tests.Mocks;
using RabbitMQ.Client;

namespace DandyRabbitMQ.Producer.Tests;

public class ConfiguredMesssageTypeDispatchInfoTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void Create_WithExchange_WithRoutingKeys()
    {
        var message = ConfiguredMessage.Create();

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            "custom-exchange",
            ["custom-routing-key"],
            message,
            null);

        Assert.Equal(typeof(ConfiguredMessage), dispatchInfo.RuntimeType);
        Assert.Equal("custom-exchange", dispatchInfo.Exchange);
        Assert.Equal(["custom-routing-key"], dispatchInfo.RoutingKeys);
    }

    [Fact]
    public void Create_WithoutExchange_WithRoutingKeys()
    {
        var message = ConfiguredMessage.Create();

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            null,
            ["custom-routing-key"],
            message,
            null);

        Assert.Equal("exchange", dispatchInfo.Exchange);
        Assert.Equal(["custom-routing-key"], dispatchInfo.RoutingKeys);
    }

    [Fact]
    public void Create_WithExchange_WithoutRoutingKeys()
    {
        var message = ConfiguredMessage.Create();

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            "custom-exchange",
            null,
            message,
            null);

        Assert.Equal("custom-exchange", dispatchInfo.Exchange);
        Assert.Equal(["routing-key"], dispatchInfo.RoutingKeys);
    }

    [Fact]
    public void Create_WithoutExchange_WithoutRoutingKeys()
    {
        var message = ConfiguredMessage.Create();

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            null,
            null,
            message,
            null);

        Assert.Equal("exchange", dispatchInfo.Exchange);
        Assert.Equal(["routing-key"], dispatchInfo.RoutingKeys);
    }

    [Fact]
    public void Create_WithDuplicateRoutingKeys_ReturnsDistinctRoutingKeys()
    {
        var message = ConfiguredMessage.Create();

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            "custom-exchange",
            ["routing-key", "routing-key"],
            message,
            null);

        Assert.Equal(["routing-key"], dispatchInfo.RoutingKeys);
    }

    [Fact]
    public void Create_WithEmptyRoutingKeys_Throws()
    {
        var message = ConfiguredMessage.Create();

        Assert.Throws<InvalidOperationException>(() =>
        {
            return DispatchInfo.Create(
                fixture.GetMessagesConfiguration(),
                "custom-exchange",
                [],
                message,
                null);
        });
    }

    [Fact]
    public void Create_WithWhitespaceExchange_Throws()
    {
        var message = ConfiguredMessage.Create();

        Assert.Throws<InvalidOperationException>(() =>
        {
            return DispatchInfo.Create(
                fixture.GetMessagesConfiguration(),
                " ",
                ["custom-routing-key"],
                message,
                null);
        });
    }

    [Fact]
    public void Create_SettingType_BasicPropertiesTypeNotOverwritten()
    {
        var message = ConfiguredMessage.Create();
        var properties = new BasicProperties { Type = "custom-type" };

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            null,
            null,
            message,
            properties);

        Assert.Equal("custom-type", dispatchInfo.Properties.Type);
    }

    [Fact]
    public void Create_ResolvesCorrectRuntimeType()
    {
        var message = ConfiguredMessage.Create();

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            null,
            null,
            message,
            null);

        Assert.Equal(typeof(ConfiguredMessage), dispatchInfo.RuntimeType);
    }
}
