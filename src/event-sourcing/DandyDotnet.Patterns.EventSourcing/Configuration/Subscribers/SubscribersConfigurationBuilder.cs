using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Subscribers;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Subscribers;

/// <summary>
///     Provides a builder for configuring event subscribers in the event store.
/// </summary>
public sealed class SubscribersConfigurationBuilder
{
    private readonly SubscribersConfiguration _configuration = new();

    /// <summary>
    ///     Registers and configures an event subscriber type.
    /// </summary>
    /// <param name="subscriberType">The concrete subscriber type implementing <see cref="ISubscriber{TEvent}" />.</param>
    /// <param name="builderAction">An action to configure subscriber options such as execution mode and retry policy.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="subscriberType" /> does not implement <see cref="ISubscriber{TEvent}" />.</exception>
    public SubscribersConfigurationBuilder AddSubscriber(Type subscriberType, Action<SubscriberConfigurationBuilder> builderAction)
    {
        if (!subscriberType.IsAssignableTo(typeof(ISubscriber<>)))
            throw new ArgumentException($"Subscriber of type '{subscriberType}' does not implement '{typeof(ISubscriber<>)}'.", nameof(subscriberType));

        var eventType = subscriberType.GetGenericArguments()[0];
        var builder = new SubscriberConfigurationBuilder(subscriberType, eventType);
        builderAction(builder);
        var configuration = builder.Build();

        _configuration.ByType[configuration.RuntimeType] = configuration;
        _configuration.ByKey[configuration.Key] = configuration;

        return this;
    }

    internal SubscribersConfiguration Build()
    {
        return _configuration;
    }
}