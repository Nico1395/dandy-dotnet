using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;
using DandyDotnet.Patterns.EventSourcing.Configuration.Events;
using DandyDotnet.Patterns.EventSourcing.Configuration.Outbox;
using DandyDotnet.Patterns.EventSourcing.Configuration.Subscribers;

namespace DandyDotnet.Patterns.EventSourcing.Configuration;

/// <summary>
///     Provides a fluent builder for configuring the Event Sourcing subsystem.
/// </summary>
/// <remarks>
///     <para>
///         Use this builder to configure aggregates, domain events, subscribers, outbox processing,
///         plugins (e.g. database persistence), assembly scanning, and error handling delegates.
///     </para>
/// </remarks>
public sealed class EventStoreConfigurationBuilder
{
    private readonly Dictionary<string, PluginConfiguration> _plugins = [];

    /// <summary>
    ///     Gets the configuration builder for aggregate roots, factories, and snapshot policies.
    /// </summary>
    public AggregatesConfigurationBuilder Aggregates { get; } = new();

    /// <summary>
    ///     Gets the configuration builder for domain events, type aliases, and lifetimes.
    /// </summary>
    public EventsConfigurationBuilder Events { get; } = new();

    /// <summary>
    ///     Gets the configuration builder for event subscribers, execution modes, and retry counts.
    /// </summary>
    public SubscribersConfigurationBuilder Subscribers { get; } = new();

    /// <summary>
    ///     Gets the configuration builder for transactional outbox dispatching and background daemon settings.
    /// </summary>
    public OutboxConfigurationBuilder Outbox { get; } = new();

    /// <summary>
    ///     Gets or sets the list of assemblies to scan for decorated aggregates, events, and subscribers.
    /// </summary>
    public Assembly[]? Assemblies { get; set; }

    /// <summary>
    ///     Gets or sets a custom callback invoked when an error occurs while publishing an outbox envelope to a subscriber.
    /// </summary>
    public Action<IServiceProvider, SubscriberConfiguration, SubscriberContext, Exception>? OnOutboxPublishException { get; set; }

    /// <summary>
    ///     Gets or sets a custom callback invoked when an unhandled exception occurs inside a subscriber handler.
    /// </summary>
    public Action<IServiceProvider, Exception>? OnSubscriberException { get; set; }

    /// <summary>
    ///     Registers an Event Sourcing plugin (e.g., a SQL persistence provider).
    /// </summary>
    /// <param name="plugin">The plugin configuration instance to register.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public EventStoreConfigurationBuilder UsePlugin(PluginConfiguration plugin)
    {
        _plugins[plugin.Slot] = plugin;
        return this;
    }

    /// <summary>
    ///     Specifies assemblies to scan for decorated aggregates, events, and subscriber implementations.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
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