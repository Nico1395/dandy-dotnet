using System.Collections.Concurrent;
using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Subscribers;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Subscribers;

/// <summary>
///     Represents the registry and configuration collection for all event subscribers in the event store.
/// </summary>
public sealed class SubscribersConfiguration
{
    internal ConcurrentDictionary<Type, SubscriberConfiguration> ByType { get; } = [];
    internal ConcurrentDictionary<string, SubscriberConfiguration> ByKey { get; } = [];
    internal ConcurrentDictionary<Type, List<SubscriberConfiguration>> ByEventType { get; } = [];

    /// <summary>
    ///     Gets the read-only dictionary of subscriber configurations indexed by their concrete runtime type.
    /// </summary>
    public IReadOnlyDictionary<Type, SubscriberConfiguration> SubscribersByType => ByType;

    /// <summary>
    ///     Gets the read-only dictionary of subscriber configurations indexed by their unique string key.
    /// </summary>
    public IReadOnlyDictionary<string, SubscriberConfiguration> SubscribersByKey => ByKey;

    /// <summary>
    ///     Gets the read-only dictionary of subscriber configurations indexed by domain event type.
    /// </summary>
    public IReadOnlyDictionary<Type, List<SubscriberConfiguration>> SubscribersByEventType => ByEventType;

    internal SubscriberConfiguration GetOrAddSubscriberConfiguration(Type subscriberType)
    {
        return ByType.GetOrAdd(subscriberType, type =>
        {
            var subscriberInterface = subscriberType
                .GetInterfaces()
                .FirstOrDefault(interfaceType =>
                    interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(ISubscriber<>));

            if (subscriberInterface is null)
                throw new ArgumentException($"Subscriber of type '{subscriberType}' does not implement '{typeof(ISubscriber<>)}'.", nameof(subscriberType));

            var attribute = type.GetCustomAttribute<SubscriberAttribute>();
            var eventType = subscriberInterface.GetGenericArguments()[0];
            var configuration = new SubscriberConfiguration
            {
                Key = attribute?.Key ?? subscriberType.Name,
                AbstractionType = typeof(ISubscriber<>).MakeGenericType(eventType),
                RuntimeType = subscriberType,
                EventType = eventType,
                Mode = attribute?.Mode ?? SubscriberMode.Async,
                Retries = attribute?.Retries > 0 ? attribute.Retries : null,
            };

            ByKey[configuration.Key] = configuration;
            var subscriberTypes = ByEventType.GetOrAdd(eventType, _ => []);
            subscriberTypes.Add(configuration);

            return configuration;
        });
    }
}