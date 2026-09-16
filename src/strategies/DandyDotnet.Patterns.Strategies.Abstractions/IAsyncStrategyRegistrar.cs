namespace DandyDotnet.Patterns.Strategies.Abstractions;

/// <summary>
///     Defines a registrar for registering asynchronous strategies associated with an asynchronous strategy definition type.
/// </summary>
/// <typeparam name="TDefinition">The type of the asynchronous strategy definition.</typeparam>
public interface IAsyncStrategyRegistrar<out TDefinition>
    where TDefinition : IAsyncStrategyDefinition
{
    /// <summary>
    ///     Registers an asynchronous strategy implementation type with the specified key.
    /// </summary>
    /// <param name="key">The key identifying the strategy implementation.</param>
    /// <param name="strategyType">The concrete strategy type implementing <see cref="IAsyncStrategy{TDefinition}"/>.</param>
    /// <returns>The same registrar instance so that additional calls can be chained.</returns>
    IAsyncStrategyRegistrar<TDefinition> AddStrategy(object key, Type strategyType);

    /// <summary>
    ///     Registers an asynchronous strategy implementation of type <typeparamref name="TStrategy" /> with the specified key.
    /// </summary>
    /// <typeparam name="TStrategy">The concrete strategy type implementing <see cref="IAsyncStrategy{TDefinition}"/>.</typeparam>
    /// <param name="key">The key identifying the strategy implementation.</param>
    /// <returns>The same registrar instance so that additional calls can be chained.</returns>
    IAsyncStrategyRegistrar<TDefinition> AddStrategy<TStrategy>(object key) where TStrategy : class, IAsyncStrategy<TDefinition>;
}

/// <summary>
///     Defines a registrar for registering asynchronous strategies that return a result associated with an asynchronous strategy definition type.
/// </summary>
/// <typeparam name="TDefinition">The type of the asynchronous strategy definition.</typeparam>
/// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
public interface IAsyncStrategyRegistrar<out TDefinition, TReturn>
    where TDefinition : IAsyncStrategyDefinition<TReturn>
{
    /// <summary>
    ///     Registers an asynchronous strategy implementation type with the specified key.
    /// </summary>
    /// <param name="key">The key identifying the strategy implementation.</param>
    /// <param name="strategyType">The concrete strategy type implementing <see cref="IAsyncStrategy{TDefinition, TReturn}"/>.</param>
    /// <returns>The same registrar instance so that additional calls can be chained.</returns>
    IAsyncStrategyRegistrar<TDefinition, TReturn> AddStrategy(object key, Type strategyType);

    /// <summary>
    ///     Registers an asynchronous strategy implementation of type <typeparamref name="TStrategy" /> with the specified key.
    /// </summary>
    /// <typeparam name="TStrategy">The concrete strategy type implementing &lt;see cref="IAsyncStrategy{TDefinition, TReturn}"/&gt;.</typeparam>
    /// <param name="key">The key identifying the strategy implementation.</param>
    /// <returns>The same registrar instance so that additional calls can be chained.</returns>
    IAsyncStrategyRegistrar<TDefinition, TReturn> AddStrategy<TStrategy>(object key) where TStrategy : class, IAsyncStrategy<TDefinition, TReturn>;
}
