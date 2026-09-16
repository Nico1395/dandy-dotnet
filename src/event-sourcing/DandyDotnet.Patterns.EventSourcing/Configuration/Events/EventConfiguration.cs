namespace DandyDotnet.Patterns.EventSourcing.Configuration.Events;

/// <summary>
///     Represents the configuration options for a specific domain event type.
/// </summary>
public sealed class EventConfiguration
{
    /// <summary>
    ///     Gets the unique string identifier used to represent this event type.
    /// </summary>
    public string Key { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the runtime CLR type of the domain event.
    /// </summary>
    public required Type RuntimeType { get; init; }

    /// <summary>
    ///     Gets the outbox lifetime duration for this event type, or <see langword="null" /> to use the default lifetime.
    /// </summary>
    public TimeSpan? Lifetime { get; internal set; }
}