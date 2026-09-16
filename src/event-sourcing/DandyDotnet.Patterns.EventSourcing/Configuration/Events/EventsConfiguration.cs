using System.Collections.Concurrent;
using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Events;

/// <summary>
///     Represents the registry and configuration collection for all domain event types in the event store.
/// </summary>
public sealed class EventsConfiguration
{
    internal ConcurrentDictionary<Type, EventConfiguration> EventConfigsByType { get; } = new();
    internal ConcurrentDictionary<string, EventConfiguration> EventConfigsByKey { get; } = new();

    /// <summary>
    ///     Gets the read-only dictionary of event configurations indexed by CLR runtime type.
    /// </summary>
    public IReadOnlyDictionary<Type, EventConfiguration> EventsByType => EventConfigsByType;

    /// <summary>
    ///     Gets the read-only dictionary of event configurations indexed by string key.
    /// </summary>
    public IReadOnlyDictionary<string, EventConfiguration> EventsByKey => EventConfigsByKey;

    /// <summary>
    ///     Gets or sets the default lifetime applied to events that do not have an explicit lifetime configured.
    /// </summary>
    public TimeSpan DefaultLifetime { get; set; } = TimeSpan.FromMinutes(15);

    internal EventConfiguration GetOrAddEventConfiguration(Type eventType)
    {
        return EventConfigsByType.GetOrAdd(eventType, type =>
        {
            var attribute = type.GetCustomAttribute<EventAttribute>();
            var configuration = new EventConfiguration
            {
                Key = attribute?.Key ?? eventType.Name,
                RuntimeType = eventType,
                Lifetime = attribute is { LifetimeMinutes: > 0 } ? TimeSpan.FromMinutes(attribute.LifetimeMinutes) : null,
            };

            return EventConfigsByKey[configuration.Key] = configuration;
        });
    }
}