namespace DandyDotnet.Patterns.Strategies.Abstractions;

/// <summary>
///     Represents an asynchronous strategy that executes an operation based on an asynchronous strategy definition.
/// </summary>
/// <typeparam name="TDefinition">The type of the strategy definition containing execution parameters.</typeparam>
public interface IAsyncStrategy<in TDefinition>
    where TDefinition : IAsyncStrategyDefinition
{
    /// <summary>
    ///     Executes the strategy logic asynchronously using the specified definition.
    /// </summary>
    /// <param name="definition">The strategy definition containing the required parameters for execution.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ExecuteAsync(TDefinition definition, CancellationToken cancellationToken);
}

/// <summary>
///     Represents an asynchronous strategy that executes an operation based on an asynchronous strategy definition and returns a result.
/// </summary>
/// <typeparam name="TDefinition">The type of the strategy definition containing execution parameters.</typeparam>
/// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
public interface IAsyncStrategy<in TDefinition, TReturn>
    where TDefinition : IAsyncStrategyDefinition<TReturn>
{
    /// <summary>
    ///     Executes the strategy logic asynchronously using the specified definition and returns a result.
    /// </summary>
    /// <param name="definition">The strategy definition containing the required parameters for execution.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation, containing the result of the strategy execution.</returns>
    Task<TReturn> ExecuteAsync(TDefinition definition, CancellationToken cancellationToken);
}
