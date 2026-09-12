using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;
using DandyDotnet.Patterns.EventSourcing.Configuration.Events;
using DandyDotnet.Patterns.EventSourcing.Configuration.Outbox;
using DandyDotnet.Patterns.EventSourcing.Configuration.Subscribers;
using DandyDotnet.Patterns.EventSourcing.Subscribers;

namespace DandyDotnet.Patterns.EventSourcing.Configuration;

public sealed class EventStoreConfiguration
{
    internal EventStoreConfiguration()
    {
    }

    public required AggregatesConfiguration Aggregates { get; init; }
    public required EventsConfiguration Events { get; init; }
    public required SubscribersConfiguration Subscribers { get; init; }
    public required OutboxConfiguration Outbox { get; init; }

    public Assembly[]? Assemblies { get; init; }

    public Action<IServiceProvider, SubscriberConfiguration, SubscriberContext, Exception>? OnOutboxPublishException { get; set; }
    public Action<IServiceProvider, Exception>? OnSubscriberException { get; set; }
    public Action<IServiceProvider, Exception>? OnDaemonIterationException { get; set; }

    internal IReadOnlyDictionary<string, PluginConfiguration> Plugins { get; init; } = new Dictionary<string, PluginConfiguration>();
}