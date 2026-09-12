using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;
using DandyDotnet.Patterns.EventSourcing.Configuration.Events;
using DandyDotnet.Patterns.EventSourcing.Configuration.Outbox;
using DandyDotnet.Patterns.EventSourcing.Configuration.Subscribers;
using DandyDotnet.Patterns.EventSourcing.Subscribers;

namespace DandyDotnet.Patterns.EventSourcing.Configuration;

public sealed class EventStoreConfigurationBuilder
{
    private readonly Dictionary<string, PluginConfiguration> _plugins = [];

    public AggregatesConfigurationBuilder Aggregates { get; } = new();
    public EventsConfigurationBuilder Events { get; } = new();
    public SubscribersConfigurationBuilder Subscribers { get; } = new();
    public OutboxConfigurationBuilder Outbox { get; } = new();

    public Assembly[]? Assemblies { get; set; }

    public Action<IServiceProvider, SubscriberConfiguration, SubscriberContext, Exception>? OnOutboxPublishException { get; set; }
    public Action<IServiceProvider, Exception>? OnSubscriberException { get; set; }

    public EventStoreConfigurationBuilder UsePlugin(PluginConfiguration plugin)
    {
        _plugins[plugin.Slot] = plugin;
        return this;
    }

    public EventStoreConfigurationBuilder ScanInAssemblies(params Assembly[] assemblies)
    {
        Assemblies = assemblies;
        return this;
    }

    internal EventStoreConfiguration Build()
    {
        return new EventStoreConfiguration
        {
            Aggregates = Aggregates.Build(),
            Events = Events.Build(),
            Subscribers = Subscribers.Build(),
            Outbox = Outbox.Build(),
            Plugins = _plugins,
            Assemblies = Assemblies,
            OnOutboxPublishException = OnOutboxPublishException,
            OnSubscriberException = OnSubscriberException,
        };
    }
}