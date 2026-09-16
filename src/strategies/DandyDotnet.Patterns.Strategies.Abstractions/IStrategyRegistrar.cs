namespace DandyDotnet.Patterns.Strategies.Abstractions;

/// <summary>
///     Defines a registrar for registering synchronous strategies associated with a strategy definition type.
/// </summary>
/// <typeparam name="TDefinition">The type of the strategy definition.</typeparam>
public interface IStrategyRegistrar<out TDefinition>
    where TDefinition : IStrategyDefinition
{
    /// <summary>
    ///     Registers a strategy implementation type with the specified key.
    /// </summary>
    /// <param name="key">The key identifying the strategy implementation.</param>
    /// <param name="strategyType">The concrete strategy type implementing <see cref="IStrategy{TDefinition}"/>.</param>
    /// <returns>The same registrar instance so that additional calls can be chained.</returns>
    IStrategyRegistrar<TDefinition> AddStrategy(object key, Type strategyType);

    /// <summary>
    ///     Registers a strategy implementation of type <typeparamref name="TStrategy" /> with the specified key.
    /// </summary>
    /// <typeparam name="TStrategy">The concrete strategy type implementing <see cref="IStrategy{TDefinition}"/>.</typeparam>
    /// <param name="key">The key identifying the strategy implementation.</param>
    /// <returns>The same registrar instance so that additional calls can be chained.</returns>
    IStrategyRegistrar<TDefinition> AddStrategy<TStrategy>(object key) where TStrategy : class, IStrategy<TDefinition>;
}

/// <summary>
///     Defines a registrar for registering synchronous strategies that return a result, associated with a strategy definition type.
/// </summary>
/// <typeparam name="TDefinition">The type of the strategy definition.</typeparam>
/// <typeparam name="TReturn">The type of the result returned by the strategy execution.</typeparam>
public interface IStrategyRegistrar<out TDefinition, in TReturn>
    where TDefinition : IStrategyDefinition<TReturn>
{
    /// <summary>
    ///     Registers a strategy implementation type with the specified key.
    /// </summary>
    /// <param name="key">The key identifying the strategy implementation.</param>
    /// <param name="strategyType">The concrete strategy type implementing <see cref="IStrategy{TDefinition, TReturn}"/>.</param>
    /// <returns>The same registrar instance so that additional calls can be chained.</returns>
    IStrategyRegistrar<TDefinition, TReturn> AddStrategy(object key, Type strategyType);

    /// <summary>
    ///     Registers a strategy implementation of type <typeparamref name="TStrategy" /> with the specified key.
    /// </summary>
    /// <typeparam name="TStrategy">The concrete strategy type implementing <see cref="IStrategy{TDefinition, TReturn}"/>.</typeparam>
    /// <param name="key">The key identifying the strategy implementation.</param>
    /// <returns>The same registrar instance so that additional calls can be chained.</returns>
    IStrategyRegistrar<TDefinition, TReturn> AddStrategy<TStrategy>(object key) where TStrategy : class, IStrategy<TDefinition, TReturn>;
}