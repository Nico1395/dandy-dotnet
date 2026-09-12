namespace DandyDotnet.Patterns.EventSourcing;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
public sealed class AggregateFactoryAttribute : Attribute;