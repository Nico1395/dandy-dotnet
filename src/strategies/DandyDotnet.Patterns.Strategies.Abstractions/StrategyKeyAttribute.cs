namespace DandyDotnet.Patterns.Strategies.Abstractions;

/// <summary>
///     Specifies the key associated with a strategy implementation class for automatic discovery and registration.
/// </summary>
/// <param name="key">The key identifying the strategy.</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class StrategyKeyAttribute(object key) : Attribute
{
    /// <summary>
    ///     Gets the key that identifies the strategy implementation.
    /// </summary>
    public object Key { get; } = key;
}
