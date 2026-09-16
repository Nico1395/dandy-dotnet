using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Unit.Messages;

public sealed class MessagesConfigurationTests
{
    [Fact]
    public void Merge_WithDistinctMessages_PreservesBothIndexes()
    {
        var first = new MessagesConfigurationBuilder().AddMessage(typeof(ConfiguredMessage), x => x.SetKey("first")).Build();
        var second = new MessagesConfigurationBuilder().AddMessage(typeof(SecondMessage), x => x.SetKey("second")).Build();
        var merged = MessagesConfiguration.Merge(first, second);
        Assert.Equal(2, merged.MessagesByRuntimeType.Count);
        Assert.Equal(2, merged.MessagesByKey.Count);
        Assert.Same(first.MessagesByRuntimeType[typeof(ConfiguredMessage)], merged.MessagesByRuntimeType[typeof(ConfiguredMessage)]);
        Assert.Same(second.MessagesByRuntimeType[typeof(SecondMessage)], merged.MessagesByRuntimeType[typeof(SecondMessage)]);
        Assert.Same(merged.MessagesByRuntimeType[typeof(ConfiguredMessage)], merged.MessagesByKey["first"]);
        Assert.Same(merged.MessagesByRuntimeType[typeof(SecondMessage)], merged.MessagesByKey["second"]);
    }

    [Fact]
    public void AddMessage_WithExistingRuntimeType_PreservesOriginalConfiguration()
    {
        var configuration = new MessagesConfigurationBuilder().AddMessage(typeof(ConfiguredMessage), x => x.SetKey("first")).Build();
        var replacement = new MessageConfigurationBuilder(typeof(ConfiguredMessage)).SetKey("second").Build();
        configuration.AddMessage(replacement);
        Assert.Equal("first", configuration.MessagesByRuntimeType[typeof(ConfiguredMessage)].Key);
    }

    [Fact]
    public void Merge_WithDuplicateTypeAndKey_PreservesFirstConfiguration()
    {
        var first = new MessagesConfigurationBuilder()
            .AddMessage(typeof(ConfiguredMessage), message => message.SetKey("same").SetExchange("first")).Build();
        var second = new MessagesConfigurationBuilder()
            .AddMessage(typeof(ConfiguredMessage), message => message.SetKey("same").SetExchange("second")).Build();

        var merged = MessagesConfiguration.Merge(first, second);

        var original = first.MessagesByRuntimeType[typeof(ConfiguredMessage)];
        Assert.Same(original, Assert.Single(merged.MessagesByRuntimeType).Value);
        Assert.Same(original, Assert.Single(merged.MessagesByKey).Value);
        Assert.Equal("second", second.MessagesByKey["same"].Exchange);
    }

    [Fact]
    public void Merge_WithRepeatedAssemblies_StoresEachAssemblyOnce()
    {
        var assembly = typeof(MessagesConfigurationTests).Assembly;
        var first = new MessagesConfigurationBuilder().ScanInAssemblies(assembly).Build();
        var second = new MessagesConfigurationBuilder().ScanInAssemblies(assembly, typeof(string).Assembly).Build();

        var merged = MessagesConfiguration.Merge(first, second);

        Assert.Equal([assembly, typeof(string).Assembly], merged.Assemblies);
        Assert.Equal([assembly], first.Assemblies);
    }

    private sealed class ConfiguredMessage
    {
    }

    private sealed class SecondMessage
    {
    }
}