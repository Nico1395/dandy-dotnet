using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;

public sealed class AggregateConfiguration
{
    public string Key { get; internal set; } = string.Empty;
    public required Type RuntimeType { get; init; }
    public Type? FactoryType { get; internal set; }
    public Func<object?, IReadOnlyEnvelope[], object>? FactoryFunc { get; internal set; }
    public int SnapshotInterval { get; internal set; } = -1;

    public bool UseSnapshots()
    {
        return SnapshotInterval > 0;
    }

    public bool ShouldCreateSnapshot(long currentVersion, long versionAfterAppend)
    {
        if (!UseSnapshots() || currentVersion == versionAfterAppend)
            return false;

        var diff = versionAfterAppend - currentVersion;
        return diff >= SnapshotInterval;
    }
}