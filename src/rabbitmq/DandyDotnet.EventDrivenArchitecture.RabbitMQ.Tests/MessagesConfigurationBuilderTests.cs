using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class MessagesConfigurationBuilderTests
{
    [Fact]
    public void AddMessage_IndexesConfigurationByRuntimeTypeAndKey()
    {
        var result = new MessagesConfigurationBuilder().AddMessage(typeof(TestMessage), x => x.SetKey("key")).Build();

        Assert.Same(result.MessagesByRuntimeType[typeof(TestMessage)], result.MessagesByKey["key"]);
    }

    [Fact]
    public void AddMessage_WithSameRuntimeType_ReplacesRuntimeTypeConfiguration()
    {
        var result = new MessagesConfigurationBuilder()
            .AddMessage(typeof(TestMessage), x => x.SetKey("first"))
            .AddMessage(typeof(TestMessage), x => x.SetKey("second"))
            .Build();

        Assert.Equal("second", result.MessagesByRuntimeType[typeof(TestMessage)].Key);
    }

    [Fact]
    public void ScanInAssemblies_StoresAssemblies()
    {
        var result = new MessagesConfigurationBuilder().ScanInAssemblies(typeof(MessagesConfigurationBuilderTests).Assembly).Build();

        Assert.Contains(typeof(MessagesConfigurationBuilderTests).Assembly, result.Assemblies);
    }

    private sealed class TestMessage
    {
    }
}
