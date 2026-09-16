using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests;

public sealed class RemainingConfigurationCoverageTests
{
    [Fact]
    public void ProduceAsync_WithWhitespaceRoutingKeys_Throws()
    {
        var configuration = Configuration(typeof(ConfiguredMessage));
        Assert.Throws<InvalidOperationException>(() => DispatchInfo.Create(configuration, "exchange", [" ", "\t"], new ConfiguredMessage(), null));
    }

    [Fact]
    public void ProduceAsync_WithNullMessage_ThrowsExpectedException()
    {
        var configuration = Configuration(typeof(ConfiguredMessage));
        Assert.ThrowsAny<Exception>(() => DispatchInfo.Create(configuration, "exchange", ["key"], null!, null));
    }

    [Fact]
    public void ProduceAsync_WithNullProperties_CreatesBasicProperties()
    {
        var dispatch = DispatchInfo.Create(Configuration(typeof(ConfiguredMessage)), "exchange", ["key"], new ConfiguredMessage(), null);
        Assert.NotNull(dispatch.Properties);
    }

    [Fact]
    public void ProduceAsync_WithExistingProperties_PreservesHeaders()
    {
        var properties = new BasicProperties { Headers = new Dictionary<string, object?> { ["x-test"] = "value" } };
        var dispatch = DispatchInfo.Create(Configuration(typeof(ConfiguredMessage)), "exchange", ["key"], new ConfiguredMessage(), properties);
        Assert.Same(properties, dispatch.Properties);
        Assert.Equal("value", dispatch.Properties.Headers!["x-test"]);
    }

    [Fact]
    public void ProduceAsync_WithMultipleRoutingKeys_PublishesOncePerDistinctKey()
    {
        var dispatch = DispatchInfo.Create(Configuration(typeof(ConfiguredMessage)), "exchange", ["a", "a", " b ", "b"], new ConfiguredMessage(), null);
        Assert.Equal(["a", "b"], dispatch.RoutingKeys);
    }

    [Fact]
    public void ProduceAsync_WithEmptyMessageKey_UsesExpectedFallbackOrThrows()
    {
        var configuration = Configuration(typeof(ConfiguredMessage));
        configuration.MessagesByRuntimeType[typeof(ConfiguredMessage)].Key = "";
        var dispatch = DispatchInfo.Create(configuration, "exchange", ["key"], new ConfiguredMessage(), null);
        Assert.Equal("", dispatch.Properties.Type);
    }

    [Fact]
    public void MessagesConfiguration_MergesExistingAndNewConfiguration()
    {
        var first = new MessagesConfigurationBuilder().AddMessage(typeof(ConfiguredMessage), x => x.SetKey("first")).Build();
        var second = new MessagesConfigurationBuilder().AddMessage(typeof(SecondMessage), x => x.SetKey("second")).Build();
        var merged = MessagesConfiguration.Merge(first, second);
        Assert.Contains(typeof(ConfiguredMessage), merged.MessagesByRuntimeType.Keys);
        Assert.Contains(typeof(SecondMessage), merged.MessagesByRuntimeType.Keys);
    }

    [Fact]
    public void DuplicateMessageKey_LastConfigurationWinsOrThrows()
    {
        var result = new MessagesConfigurationBuilder()
            .AddMessage(typeof(ConfiguredMessage), x => x.SetKey("duplicate"))
            .AddMessage(typeof(SecondMessage), x => x.SetKey("duplicate"))
            .Build();
        Assert.Equal(typeof(SecondMessage), result.MessagesByKey["duplicate"].RuntimeType);
    }

    [Fact]
    public void CustomMessageKey_RemovesOldRuntimeNameLookup()
    {
        var configuration = new MessagesConfigurationBuilder().ScanInAssemblies(typeof(AttributedMessage).Assembly).Build();
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        services.AddRabbitMQMessages(configuration);
        var result = services.BuildServiceProvider().GetRequiredService<MessagesConfiguration>();
        Assert.DoesNotContain(nameof(AttributedMessage), result.MessagesByKey.Keys);
        Assert.Contains("custom-message", result.MessagesByKey.Keys);
    }

    [Fact]
    public void MessageAttributeScanning_RegistersMessageByRuntimeType()
    {
        var result = ScanMessages();
        Assert.Same(result.MessagesByRuntimeType[typeof(AttributedMessage)], result.MessagesByKey["custom-message"]);
    }

    [Fact]
    public void MessageAttributeScanning_RegistersMessageByCustomKey()
    {
        Assert.Equal(typeof(AttributedMessage), ScanMessages().MessagesByKey["custom-message"].RuntimeType);
    }

    [Fact]
    public void MessageAttributeScanning_RegistersExchange()
    {
        Assert.Equal("scanned-exchange", ScanMessages().MessagesByRuntimeType[typeof(AttributedMessage)].Exchange);
    }

    [Fact]
    public void MessageAttributeScanning_RegistersRoutingKeys()
    {
        Assert.Equal(["scanned.one", "scanned.two"], ScanMessages().MessagesByRuntimeType[typeof(AttributedMessage)].RoutingKeys);
    }

    [Fact]
    public void MessageAttributeScanning_RegistersMetadata()
    {
        Assert.Equal("metadata", ScanMessages().MessagesByRuntimeType[typeof(AttributedMessage)].Metadata["source"]);
    }

    [Fact]
    public void AssemblyScanning_WithNoAssemblies_DoesNotRegisterConsumers()
    {
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        services.AddRabbitMQConsumer(config => { });
        using var provider = services.BuildServiceProvider();
        Assert.Null(provider.GetService<DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions.IConsumer<ConfiguredMessage>>());
    }

    [Fact]
    public void ConnectToCluster_UsesDefaultRecoveryInterval()
    {
        var config = new ConnectivityConfigurationBuilder().ConnectToCluster("u", "p", [], null).Build();
        Assert.Equal(TimeSpan.FromSeconds(5), ((ConnectionFactory)config.ConnectionFactory).NetworkRecoveryInterval);
    }

    [Fact]
    public void SetConnectionFactory_UsesSuppliedFactory()
    {
        var factory = new ConnectionFactory { ClientProvidedName = "supplied" };
        var config = new ConnectivityConfigurationBuilder().SetConnectionFactory(factory).Build();
        Assert.Same(factory, config.ConnectionFactory);
    }

    [Fact]
    public void ConnectivityRegistration_IsIdempotent()
    {
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        var config = new ConnectivityConfigurationBuilder().Build();
        services.AddRabbitMQConnectivity(config).AddRabbitMQConnectivity(config);
        Assert.Single(services.Where(x => x.ServiceType == typeof(DandyDotnet.EventDrivenArchitecture.RabbitMQ.Abstractions.Connectivity.IConnectionProvider)));
    }

    [Fact]
    public void SubscribeChannel_WithMultipleRoutingKeys_CreatesAllBindings()
    {
        var result = new DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations.DeclarationsConfigurationBuilder()
            .SubscribeChannel("exchange", "queue", channel => channel.Queue.RoutingKeys = ["one", "two"])
            .Build();
        Assert.Equal(["one", "two"], result.ChannelsByQueueName["queue"].Queue.RoutingKeys);
    }

    [Fact]
    public void QueueConfiguration_IsIdempotentAcrossFixtureStartup()
    {
        var builder = new DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations.DeclarationsConfigurationBuilder();
        builder.SubscribeChannel("exchange", "queue", _ => { });
        builder.SubscribeChannel("exchange", "queue", _ => { });
        Assert.Single(builder.Build().ChannelsByQueueName);
    }

    [Fact]
    public void MessageConfigurationWithMetadata_IsAvailableToConfiguration()
    {
        var configuration = new MessageConfigurationBuilder(typeof(ConfiguredMessage)).AddMetadata("key", "value").Build();
        Assert.Equal("value", configuration.Metadata["key"]);
    }

    [Fact]
    public void ManualMessageConfiguration_OverridesScannedConfiguration()
    {
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        services.AddRabbitMQMessages(new MessagesConfigurationBuilder().ScanInAssemblies(typeof(AttributedMessage).Assembly).Build());
        services.AddRabbitMQMessages(config => config.AddMessage(typeof(AttributedMessage), x => x.SetKey("manual")));
        var result = services.BuildServiceProvider().GetRequiredService<MessagesConfiguration>();
        Assert.Contains("manual", result.MessagesByKey.Keys);
    }

    private static MessagesConfiguration Configuration(Type type)
    {
        return new MessagesConfigurationBuilder().AddMessage(type, x => x.SetExchange("exchange").SetRoutingKeys("key")).Build();
    }

    private static MessagesConfiguration ScanMessages()
    {
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        services.AddRabbitMQMessages(new MessagesConfigurationBuilder().ScanInAssemblies(typeof(AttributedMessage).Assembly).Build());
        return services.BuildServiceProvider().GetRequiredService<MessagesConfiguration>();
    }

    private sealed class ConfiguredMessage : Message
    {
    }

    private sealed class SecondMessage : Message
    {
    }

    [MessageKey("custom-message")]
    [MessageExchange("scanned-exchange")]
    [MessageRoutes("scanned.one", "scanned.two")]
    [MessageMetadata("source", "metadata")]
    private sealed class AttributedMessage : Message
    {
    }
}
