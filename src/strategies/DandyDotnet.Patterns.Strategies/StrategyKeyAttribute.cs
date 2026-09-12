namespace DandyDotnet.Patterns.Strategies;

[AttributeUsage(AttributeTargets.Class)]
public sealed class StrategyKeyAttribute(object key) : Attribute
{
    public object Key { get; } = key;
}
