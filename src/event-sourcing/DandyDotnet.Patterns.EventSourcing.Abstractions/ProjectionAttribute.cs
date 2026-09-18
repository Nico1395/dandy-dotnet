namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ProjectionAttribute : Attribute
{
    public string? Key { get; init; }
    public ProjectionMode Mode { get; init; } = ProjectionMode.Async;
}