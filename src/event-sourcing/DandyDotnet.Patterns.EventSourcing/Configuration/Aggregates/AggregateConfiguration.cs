using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;

/// <summary>
///     Represents the configuration options for a specific aggregate root type.
/// </summary>
public sealed class AggregateConfiguration
{
    /// <summary>
    ///     Gets the unique string identifier used to represent this aggregate type.
    /// </summary>
    public string Key { get; internal set; } = string.Empty;

    /// <summary>
    ///     Gets the runtime CLR type of the aggregate root.
    /// </summary>
    public required Type RuntimeType { get; init; }

    /// <summary>
    ///     Gets the factory delegate used to construct or restore the aggregate from a snapshot and event stream.
    /// </summary>
    public Func<object?, IReadOnlyEnvelope[], object>? FactoryFunc { get; internal set; }

    /// <summary>
    ///     Gets the snapshot interval representing how many events trigger a snapshot creation, or <c>-1</c> if snapshots are disabled.
    /// </summary>
    public int SnapshotInterval { get; internal set; } = -1;

    /// <summary>
    ///     Determines whether snapshots are enabled for this aggregate.
    /// </summary>
    /// <returns><see langword="true" /> if snapshots are enabled; otherwise, <see langword="false" />.</returns>
    public bool UseSnapshots()
    {
        return SnapshotInterval > 0;
    }

    /// <summary>
    ///     Determines whether a snapshot should be created given the current and target stream versions.
    /// </summary>
    /// <param name="currentVersion">The stream version before appending new events.</param>
    /// <param name="versionAfterAppend">The stream version after appending new events.</param>
    /// <returns><see langword="true" /> if a snapshot should be created; otherwise, <see langword="false" />.</returns>
    public bool ShouldCreateSnapshot(long currentVersion, long versionAfterAppend)
    {
        if (!UseSnapshots() || currentVersion == versionAfterAppend)
            return false;

        var diff = versionAfterAppend - currentVersion;
        return diff >= SnapshotInterval;
    }
}