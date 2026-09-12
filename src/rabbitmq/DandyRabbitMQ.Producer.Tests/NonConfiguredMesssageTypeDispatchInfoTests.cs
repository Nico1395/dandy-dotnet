using DandyRabbitMQ.Producer.Tests.Fixtures;
using DandyRabbitMQ.Producer.Tests.Mocks;
using RabbitMQ.Client;

namespace DandyRabbitMQ.Producer.Tests;

public class NonConfiguredMesssageTypeDispatchInfoTests(DefaultFixture fixture) : IClassFixture<DefaultFixture>
{
    [Fact]
    public void Create_WithExchange_WithRoutingKeys()
    {
        var message = NonConfiguredMessage.Create();

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            "custom-exchange",
            ["custom-routing-key"],
            message,
            null);

        Assert.Equal(typeof(NonConfiguredMessage), dispatchInfo.RuntimeType);
        Assert.Equal("custom-exchange", dispatchInfo.Exchange);
        Assert.Equal(["custom-routing-key"], dispatchInfo.RoutingKeys);
    }

    [Fact]
    public void Create_WithoutExchange_WithRoutingKeys_Throws()
    {
        var message = NonConfiguredMessage.Create();

        Assert.Throws<InvalidOperationException>(() =>
        {
            return DispatchInfo.Create(
                fixture.GetMessagesConfiguration(),
                null,
                ["custom-routing-key"],
                message,
                null);
        });
    }

    [Fact]
    public void Create_WithExchange_WithoutRoutingKeys_Throws()
    {
        var message = NonConfiguredMessage.Create();

        Assert.Throws<InvalidOperationException>(() =>
        {
            return DispatchInfo.Create(
                fixture.GetMessagesConfiguration(),
                "custom-exchange",
                null,
                message,
                null);
        });
    }

    [Fact]
    public void Create_EmptyRoutingKeys_Throws()
    {
        var message = NonConfiguredMessage.Create();

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
    public void Create_SettingType_BasicPropertiesTypeNotOverwritten()
    {
        var message = NonConfiguredMessage.Create();
        var properties = new BasicProperties { Type = "custom-type" };

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            "custom-exchange",
            ["custom-routing-key"],
            message,
            properties);

        Assert.Equal("custom-type", dispatchInfo.Properties.Type);
    }

    [Fact]
    public void Create_NotSettingType_BasicPropertiesTypeIsSetToTypeName()
    {
        var message = NonConfiguredMessage.Create();

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            "custom-exchange",
            ["custom-routing-key"],
            message,
            null);

        Assert.Equal(nameof(NonConfiguredMessage), dispatchInfo.Properties.Type);
    }

    [Fact]
    public void Create_DeterminesCorrectRuntimeType()
    {
        var message = NonConfiguredMessage.Create();

        var dispatchInfo = DispatchInfo.Create(
            fixture.GetMessagesConfiguration(),
            "custom-exchange",
            ["custom-routing-key"],
            message,
            null);

        Assert.Equal(typeof(NonConfiguredMessage), dispatchInfo.RuntimeType);
    }
}
