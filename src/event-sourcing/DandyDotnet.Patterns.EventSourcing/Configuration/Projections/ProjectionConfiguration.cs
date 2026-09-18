using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Projections;

public sealed class ProjectionConfiguration
{
    internal List<Type> EventTypes { get; } = [];

    public string Key { get; internal set; } = string.Empty;
    public required Type RuntimeType { get; init; }
    public Func<object?, IReadOnlyEnvelope[], object>? FactoryFunc { get; internal set; }
    public Func<object, IEnumerable<(string Name, object Value)>>? KeyFactoryFunc { get; internal set; }
    public ProjectionMode Mode { get; internal set; }
}