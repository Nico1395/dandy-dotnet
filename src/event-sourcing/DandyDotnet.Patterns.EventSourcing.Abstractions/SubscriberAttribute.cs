namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

[AttributeUsage(AttributeTargets.Class)]
public sealed class SubscriberAttribute : Attribute
{
    public SubscriberMode Mode { get; init; } = SubscriberMode.Async;
    public string? Key { get; init; }
    public int Retries { get; init; } = -1;
}