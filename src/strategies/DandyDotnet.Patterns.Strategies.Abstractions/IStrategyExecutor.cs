namespace DandyDotnet.Patterns.Strategies.Abstractions;

/// <summary>
///     Defines an executor service responsible for resolving and executing synchronous and asynchronous strategies
///     based on provided strategy definitions.
/// </summary>
public interface IStrategyExecutor
{
    /// <summary>
    ///     Executes a synchronous strategy using the specified definition.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the strategy definition.</typeparam>
    /// <param name="definition">The strategy definition containing the key and parameters for execution.</param>
    void Execute<TDefinition>(TDefinition definition) where TDefinition : IStrategyDefinition;

    /// <summary>
    ///     Executes a synchronous strategy that returns a result using the specified definition.
    /// </summary>
    /// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
    /// <param name="definition">The strategy definition containing the key and parameters for execution.</param>
    /// <returns>The result of the strategy execution.</returns>
    TReturn Execute<TReturn>(IStrategyDefinition<TReturn> definition);

    /// <summary>
    ///     Executes an asynchronous strategy asynchronously using the specified definition.
    /// </summary>
    /// <typeparam name="TDefinition">The type of the asynchronous strategy definition.</typeparam>
    /// <param name="definition">The strategy definition containing the key and parameters for execution.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous execution operation.</returns>
    Task ExecuteAsync<TDefinition>(TDefinition definition, CancellationToken cancellationToken = default) where TDefinition : IAsyncStrategyDefinition;

    /// <summary>
    ///     Executes an asynchronous strategy that returns a result asynchronously using the specified definition.
    /// </summary>
    /// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
    /// <param name="definition">The strategy definition containing the key and parameters for execution.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous execution operation, containing the result of the strategy execution.</returns>
    Task<TReturn> ExecuteAsync<TReturn>(IAsyncStrategyDefinition<TReturn> definition, CancellationToken cancellationToken = default);
}
