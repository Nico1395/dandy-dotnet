namespace DandyDotnet.Patterns.Strategies.Abstractions;

/// <summary>
///     Represents an asynchronous definition for a strategy execution, containing the registration key and parameters.
/// </summary>
public interface IAsyncStrategyDefinition
{
    /// <summary>
    ///     Gets the key that identifies the strategy implementation to execute.
    /// </summary>
    object Key { get; }
}

/// <summary>
///     Represents an asynchronous definition for a strategy execution that returns a result of type <typeparamref name="TReturn" />,
///     containing the registration key and parameters.
/// </summary>
/// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
public interface IAsyncStrategyDefinition<TReturn>
{
    /// <summary>
    ///     Gets the key that identifies the strategy implementation to execute.
    /// </summary>
    object Key { get; }
}
