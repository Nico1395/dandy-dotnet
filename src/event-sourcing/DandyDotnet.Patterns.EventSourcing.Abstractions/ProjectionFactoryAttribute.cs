namespace DandyDotnet.Patterns.EventSourcing.Abstractions;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
public sealed class ProjectionFactoryAttribute : Attribute;