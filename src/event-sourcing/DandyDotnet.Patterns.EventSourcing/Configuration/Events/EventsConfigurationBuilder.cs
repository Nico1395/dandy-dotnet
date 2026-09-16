namespace DandyDotnet.Patterns.EventSourcing.Configuration.Events;

/// <summary>
///     Provides a builder for configuring domain events and default event options in the event store.
/// </summary>
public sealed class EventsConfigurationBuilder
{
    private readonly EventsConfiguration _configuration = new();
    
    /// <summary>
    ///     Registers and configures an event type.
    /// </summary>
    /// <typeparam name="TEvent">The domain event type.</typeparam>
    /// <param name="builderAction">An action to configure the event options such as its key name or custom lifetime.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public EventsConfigurationBuilder AddEvent<TEvent>(Action<EventConfigurationBuilder<TEvent>> builderAction)
        where TEvent : class
    {
        var builder = new EventConfigurationBuilder<TEvent>();
        builderAction(builder);
        var configuration = builder.Build();

        _configuration.EventConfigsByType[configuration.RuntimeType] = configuration;
        _configuration.EventConfigsByKey[configuration.Key] = configuration;

        return this;
    }

    /// <summary>
    ///     Configures the default event lifetime applied to events that do not specify a custom lifetime.
    /// </summary>
    /// <param name="lifetime">The default event lifetime duration.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public EventsConfigurationBuilder WithDefaultLifetime(TimeSpan lifetime)
    {
        _configuration.DefaultLifetime = lifetime;
        return this;
    }

    internal EventsConfiguration Build()
    {
        return _configuration;
    }
}