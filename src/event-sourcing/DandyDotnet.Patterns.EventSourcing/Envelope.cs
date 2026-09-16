using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing;

/// <summary>
///     Represents an immutable envelope wrapping a domain event and its metadata within an event stream.
/// </summary>
internal sealed class Envelope : IReadOnlyEnvelope
{
    /// <inheritdoc />
    public required string StreamId { get; init; }

    /// <inheritdoc />
    public required object Event { get; init; }

    /// <inheritdoc />
    public required long Version { get; init; }

    /// <inheritdoc />
    public required DateTime Timestamp { get; init; }

    /// <inheritdoc />
    public required string EventKey { get; init; }

    /// <inheritdoc />
    public required Type RuntimeType { get; init; }
}