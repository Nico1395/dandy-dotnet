namespace DandyDotnet.Patterns.Strategies.Abstractions;

/// <summary>
///     Represents a synchronous strategy that executes an operation based on a strategy definition.
/// </summary>
/// <typeparam name="TDefinition">The type of the strategy definition containing execution parameters.</typeparam>
public interface IStrategy<in TDefinition>
    where TDefinition : IStrategyDefinition
{
    /// <summary>
    ///     Executes the strategy logic using the specified definition.
    /// </summary>
    /// <param name="definition">The strategy definition containing the required parameters for execution.</param>
    void Execute(TDefinition definition);
}

/// <summary>
///     Represents a synchronous strategy that executes an operation based on a strategy definition and returns a result.
/// </summary>
/// <typeparam name="TDefinition">The type of the strategy definition containing execution parameters.</typeparam>
/// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
public interface IStrategy<in TDefinition, out TReturn>
    where TDefinition : IStrategyDefinition<TReturn>
{
    /// <summary>
    ///     Executes the strategy logic using the specified definition and returns a result.
    /// </summary>
    /// <param name="definition">The strategy definition containing the required parameters for execution.</param>
    /// <returns>The result of the strategy execution.</returns>
    TReturn Execute(TDefinition definition);
}