namespace DandyDotnet.Patterns.EventSourcing.Configuration.Events;

/// <summary>
///     Provides a fluent builder for configuring options for a specific event type.
/// </summary>
/// <typeparam name="TEvent">The domain event type being configured.</typeparam>
public sealed class EventConfigurationBuilder<TEvent>
    where TEvent : class
{
    private readonly EventConfiguration _configuration = new()
    {
        Key = typeof(TEvent).Name,
        RuntimeType = typeof(TEvent),
    };

    /// <summary>
    ///     Sets the custom string key used to identify the event in envelopes and persistence stores.
    /// </summary>
    /// <param name="key">The event key name.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public EventConfigurationBuilder<TEvent> WithKey(string key)
    {
        _configuration.Key = key;
        return this;
    }
    
    /// <summary>
    ///     Sets the lifetime duration for this event type before it expires in the outbox.
    /// </summary>
    /// <param name="lifetime">The lifetime duration.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public EventConfigurationBuilder<TEvent> WithLifetime(TimeSpan lifetime)
    {
        _configuration.Lifetime = lifetime;
        return this;
    }

    internal EventConfiguration Build()
    {
        return _configuration;
    }
}