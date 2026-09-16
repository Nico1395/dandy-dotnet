using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Subscribers;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Subscribers;

/// <summary>
///     Provides a fluent builder for configuring options for a specific event subscriber.
/// </summary>
public sealed class SubscriberConfigurationBuilder(Type subscriberType, Type eventType)
{
    private readonly SubscriberConfiguration _configuration = new SubscriberConfiguration
    {
        Key = subscriberType.Name,
        AbstractionType = typeof(ISubscriber<>).MakeGenericType(eventType),
        RuntimeType = subscriberType,
        EventType = eventType,
    };

    /// <summary>
    ///     Sets the unique string key that identifies this subscriber.
    /// </summary>
    /// <param name="key">The unique subscriber key name.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public SubscriberConfigurationBuilder WithKey(string key)
    {
        _configuration.Key = key;
        return this;
    }
    
    /// <summary>
    ///     Sets the execution mode (inline synchronous or outbox asynchronous) for this subscriber.
    /// </summary>
    /// <param name="mode">The <see cref="SubscriberMode" /> to use.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public SubscriberConfigurationBuilder WithMode(SubscriberMode mode)
    {
        _configuration.Mode = mode;
        return this;
    }

    /// <summary>
    ///     Sets the maximum number of retry attempts for failed asynchronous subscriber executions.
    /// </summary>
    /// <param name="retries">The maximum retry count.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public SubscriberConfigurationBuilder WithRetries(int retries)
    {
        _configuration.Retries = retries;
        return this;
    }

    internal SubscriberConfiguration Build()
    {
        return _configuration;
    }
}