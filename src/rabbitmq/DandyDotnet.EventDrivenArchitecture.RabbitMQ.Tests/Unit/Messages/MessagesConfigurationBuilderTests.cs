using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Messages;

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

    [Fact]
    public void CopyConstructor_CopiesRuntimeTypeEntriesAndAssemblies()
    {
        var original = new MessagesConfigurationBuilder()
            .AddMessage(typeof(TestMessage), x => x.SetKey("original"))
            .ScanInAssemblies(typeof(MessagesConfigurationBuilderTests).Assembly)
            .Build();

        var copy = new MessagesConfigurationBuilder(original).Build();

        Assert.Same(original.MessagesByRuntimeType[typeof(TestMessage)], copy.MessagesByRuntimeType[typeof(TestMessage)]);
        Assert.Equal(original.Assemblies, copy.Assemblies);
    }

    [Fact]
    public void AddMessage_WithDifferentRuntimeTypes_KeepsBothConfigurations()
    {
        var result = new MessagesConfigurationBuilder()
            .AddMessage(typeof(TestMessage), x => x.SetKey("one"))
            .AddMessage(typeof(OtherMessage), x => x.SetKey("two"))
            .Build();

        Assert.Equal(2, result.MessagesByRuntimeType.Count);
        Assert.Contains("one", result.MessagesByKey.Keys);
        Assert.Contains("two", result.MessagesByKey.Keys);
    }

    [Fact]
    public void AddMessage_WithDuplicateKey_IndexesLastMessageByKey()
    {
        var result = new MessagesConfigurationBuilder()
            .AddMessage(typeof(TestMessage), x => x.SetKey("duplicate"))
            .AddMessage(typeof(OtherMessage), x => x.SetKey("duplicate"))
            .Build();
        Assert.Equal(2, result.MessagesByRuntimeType.Count);
        Assert.Single(result.MessagesByKey);
        Assert.Same(result.MessagesByRuntimeType[typeof(OtherMessage)], result.MessagesByKey["duplicate"]);
    }

    private sealed class TestMessage
    {
    }

    private sealed class OtherMessage
    {
    }

}