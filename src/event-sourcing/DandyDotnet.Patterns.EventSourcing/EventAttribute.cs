namespace DandyDotnet.Patterns.EventSourcing;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EventAttribute : Attribute
{
    public string? Key { get; init; }
}