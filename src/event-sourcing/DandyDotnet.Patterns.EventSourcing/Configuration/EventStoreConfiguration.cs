using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;
using DandyDotnet.Patterns.EventSourcing.Configuration.Events;
using DandyDotnet.Patterns.EventSourcing.Configuration.Outbox;
using DandyDotnet.Patterns.EventSourcing.Configuration.Subscribers;
using DandyDotnet.Patterns.EventSourcing.Subscribers;

namespace DandyDotnet.Patterns.EventSourcing.Configuration;

/// <summary>
///     Represents the complete runtime configuration for the Event Sourcing subsystem.
/// </summary>
/// <remarks>
///     <para>
///         This class holds all aggregated configurations including aggregates, domain events, subscribers,
///         outbox processing, registered plugins, and global exception handlers.
///     </para>
/// </remarks>
public sealed class EventStoreConfiguration
{
    internal EventStoreConfiguration()
    {
    }

    /// <summary>
    ///     Gets the configuration for aggregate types, their keys, factories, and snapshot policies.
    /// </summary>
    public required AggregatesConfiguration Aggregates { get; init; }

    /// <summary>
    ///     Gets the configuration for domain event types, their keys, and lifetime policies.
    /// </summary>
    public required EventsConfiguration Events { get; init; }

    /// <summary>
    ///     Gets the configuration for event subscribers, execution modes, and retry settings.
    /// </summary>
    public required SubscribersConfiguration Subscribers { get; init; }

    /// <summary>
    ///     Gets the configuration for the transactional outbox and background daemon.
    /// </summary>
    public required OutboxConfiguration Outbox { get; init; }

    /// <summary>
    ///     Gets the assemblies configured for scanning and auto-discovery of event sourcing components.
    /// </summary>
    public Assembly[]? Assemblies { get; init; }

    /// <summary>
    ///     Gets or sets the custom callback invoked when an exception occurs while publishing an outbox event to a subscriber.
    /// </summary>
    public Action<IServiceProvider, SubscriberConfiguration, SubscriberContext, Exception>? OnOutboxPublishException { get; set; }

    /// <summary>
    ///     Gets or sets the custom callback invoked when an exception occurs inside a subscriber handler.
    /// </summary>
    public Action<IServiceProvider, Exception>? OnSubscriberException { get; set; }

    /// <summary>
    ///     Gets or sets the custom callback invoked when an exception occurs during an outbox daemon execution cycle.
    /// </summary>
    public Action<IServiceProvider, Exception>? OnDaemonIterationException { get; set; }

    internal IReadOnlyDictionary<string, PluginConfiguration> Plugins { get; init; } = new Dictionary<string, PluginConfiguration>();
}