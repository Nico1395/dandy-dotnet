using System;

namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Attribute that marks a class as an event subscriber in the Event Sourcing pattern.
/// </summary>
/// <remarks>
///     <para>
///         This attribute allows the event sourcing system to automatically discover and register subscribers
///         during assembly scanning. When applied to a class that implements <see cref="ISubscriber{TEvent}" />,
///         it configures the subscriber's behavior.
///     </para>
///     <para>
///         The <see cref="Mode" /> property determines whether the subscriber runs inline or asynchronously.
///         The <see cref="Key" /> property provides a unique identifier for the subscriber, and <see cref="Retries" />
///         controls the retry behavior for this subscriber.
///     </para>
/// </remarks>
/// <example>
///     <code language="csharp">
///         [Subscriber(Key = "UserActions", Mode = SubscriberMode.Async, Retries = 3)]
///         public class UserCreatedSubscriber : ISubscriber{UserCreatedEvent}
///         {
///             public Task HandleAsync(UserCreatedEvent @event, SubscriberContext context, CancellationToken cancellationToken)
///             {
///                 // Handle event
///                 return Task.CompletedTask;
///             }
///         }
///     </code>
/// </example>
/// <seealso cref="SubscriberMode" />
/// <seealso cref="ISubscriber{TEvent}" />
[AttributeUsage(AttributeTargets.Class)]
public sealed class SubscriberAttribute : Attribute
{
    /// <summary>
    ///     Gets the processing mode for this subscriber.
    /// </summary>
    /// <value>
    ///     The <see cref="SubscriberMode" /> that determines when the subscriber is invoked. The default is
    ///     <see cref="SubscriberMode.Async" />.
    /// </value>
    /// <remarks>
    ///     The mode can be either <see cref="SubscriberMode.Inline" /> for synchronous processing during the
    ///     append operation, or <see cref="SubscriberMode.Async" /> for asynchronous processing by the outbox daemon.
    /// </remarks>
    public SubscriberMode Mode { get; init; } = SubscriberMode.Async;

    /// <summary>
    ///     Gets the key that uniquely identifies this subscriber.
    /// </summary>
    /// <value>
    ///     A string key that identifies the subscriber. If <see langword="null" />, the subscriber's type name
    ///     will be used as the default key.
    /// </value>
    /// <remarks>
    ///     This key is used to track the subscriber's consumption progress in the outbox. Each subscriber
    ///     should have a unique key to ensure proper tracking and retry behavior.
    /// </remarks>
    public string? Key { get; init; }

    /// <summary>
    ///     Gets the maximum number of retries for this subscriber when it fails.
    /// </summary>
    /// <value>
    ///     The maximum number of retry attempts. A value of -1 (the default) means retries are disabled.
    ///     A value of 0 means only one attempt (no retries).
    /// </value>
    /// <remarks>
    ///     <para>
    ///         When a subscriber fails to process an event, it will be retried up to this many times.
    ///         The actual retry count is tracked per event envelope and subscriber combination.
    ///     </para>
    ///     <para>
    ///         The <see cref="SubscriberContext" /> passed to the subscriber contains the current retry count
    ///         and the maximum retries, which can be used to implement custom retry logic.
    ///     </para>
    /// </remarks>
    public int Retries { get; init; } = -1;
}
