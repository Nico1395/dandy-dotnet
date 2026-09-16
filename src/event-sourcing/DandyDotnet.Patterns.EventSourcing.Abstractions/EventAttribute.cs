using System;

namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

/// <summary>
///     Attribute that marks a class as an event in the Event Sourcing pattern.
/// </summary>
/// <remarks>
///     <para>
///         Events represent state changes that have occurred in the system. This attribute allows configuration
///         of the event's key and lifetime.
///     </para>
///     <para>
///         When applied to a class, it indicates that the class represents an event that can be stored in
///         the event store and replayed to reconstruct aggregate state.
///     </para>
///     <para>
///         The <see cref="Key" /> property defines how the event type is identified, and <see cref="LifetimeMinutes" />
///         controls how long the event should be retained in the outbox before being considered expired.
///     </para>
/// </remarks>
/// <example>
///     <code language="csharp">
///         [Event(Key = "UserCreated", LifetimeMinutes = 60)]
///         public class UserCreatedEvent
///         {
///             public string UserId { get; set; }
///             public string Username { get; set; }
///         }
///     </code>
/// </example>
[AttributeUsage(AttributeTargets.Class)]
public sealed class EventAttribute : Attribute
{
    /// <summary>
    ///     Gets the key used to identify this event type in the event store.
    /// </summary>
    /// <value>
    ///     A string key that uniquely identifies the event type. If <see langword="null" />, the event's
    ///     type name will be used as the default key.
    /// </value>
    /// <remarks>
    ///     This key is used for routing and identification purposes within the event store. It allows
    ///     the system to look up event configurations and map events to their respective handlers.
    /// </remarks>
    public string? Key { get; init; }

    /// <summary>
    ///     Gets the lifetime of this event type in minutes before it expires in the outbox.
    /// </summary>
    /// <value>
    ///     The number of minutes the event should be retained in the outbox. A value of -1 (the default) means
    ///     the event will never expire. If the value is 0 or positive, expired events will be automatically
    ///     removed from the outbox after this duration.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         This controls the retention period for events in the outbox. Events that exceed their lifetime
    ///         will be removed during the periodic cleanup process performed by the outbox daemon.
    ///     </para>
///     <para>
    ///         This is useful for events that have a natural expiration, such as time-sensitive notifications
    ///         or temporary data that should not be processed after a certain time period.
    ///     </para>
    /// </remarks>
    public int LifetimeMinutes { get; init; } = -1;
}
