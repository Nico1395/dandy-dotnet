using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Subscribers;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Subscribers;

/// <summary>
///     Represents the configuration options for a specific event subscriber.
/// </summary>
public sealed class SubscriberConfiguration
{
    /// <summary>
    ///     Gets the unique string identifier used to represent this subscriber.
    /// </summary>
    public string Key { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the closed generic subscriber interface type (e.g., <c>ISubscriber&lt;TEvent&gt;</c>).
    /// </summary>
    public required Type AbstractionType { get; init; }

    /// <summary>
    ///     Gets the runtime CLR type implementing the subscriber interface.
    /// </summary>
    public required Type RuntimeType { get; init; }

    /// <summary>
    ///     Gets the runtime CLR type of the domain event handled by this subscriber.
    /// </summary>
    public required Type EventType { get; init; }

    /// <summary>
    ///     Gets the execution mode specifying whether the subscriber runs inline synchronously or asynchronously via outbox.
    /// </summary>
    public SubscriberMode Mode { get; internal set; } = SubscriberMode.Async;

    /// <summary>
    ///     Gets the maximum retry count for outbox execution, or <see langword="null" /> to use the outbox default.
    /// </summary>
    public int? Retries { get; internal set; }
}