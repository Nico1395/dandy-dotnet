using System.Collections.Concurrent;
using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Events;

public sealed class EventsConfiguration
{
    internal ConcurrentDictionary<Type, EventConfiguration> EventConfigsByType { get; } = new();
    internal ConcurrentDictionary<string, EventConfiguration> EventConfigsByKey { get; } = new();

    public IReadOnlyDictionary<Type, EventConfiguration> EventsByType => EventConfigsByType;
    public IReadOnlyDictionary<string, EventConfiguration> EventsByKey => EventConfigsByKey;

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
                Lifetime = attribute != null ? TimeSpan.FromMinutes(attribute.LifetimeMinutes) : null,
            };

            return EventConfigsByKey[configuration.Key] = configuration;
        });
    }
}