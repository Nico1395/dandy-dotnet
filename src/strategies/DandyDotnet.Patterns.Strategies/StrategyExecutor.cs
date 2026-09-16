using DandyDotnet.Patterns.Strategies.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Strategies;

/// <summary>
///     Provides the standard implementation of <see cref="IStrategyExecutor" />, resolving strategies from the service provider
///     and executing them synchronously or asynchronously.
/// </summary>
/// <param name="serviceProvider">The service provider used to resolve strategy implementations.</param>
internal sealed class StrategyExecutor(IServiceProvider serviceProvider) : IStrategyExecutor
{
    /// <inheritdoc />
    public void Execute<TDefinition>(TDefinition definition)
        where TDefinition : IStrategyDefinition
    {
        var strategy = serviceProvider.GetRequiredKeyedService<IStrategy<TDefinition>>(definition.Key);
        strategy.Execute(definition);
    }

    /// <inheritdoc />
    public TReturn Execute<TReturn>(IStrategyDefinition<TReturn> definition)
    {
        var strategyType = typeof(IStrategy<,>).MakeGenericType(definition.GetType(), typeof(TReturn));
        var strategy = serviceProvider.GetRequiredKeyedService(strategyType, definition.Key);
        
        var executeMethod = strategyType.GetMethod(nameof(IStrategy<,>.Execute)) ?? throw new InvalidOperationException($"Strategy '{strategyType}' does not contain method with name '{nameof(IStrategy<,>.Execute)}'. This in an internal error, please report this error on GitHub");
        var result = executeMethod.Invoke(strategy, [definition]);

        if (result is not TReturn castedResult)
            throw new InvalidCastException($"Result of strategy '{strategyType}' is of type '{result?.GetType()}' but a result of type '{typeof(TReturn)}' was expected.");

        return castedResult;
    }

    /// <inheritdoc />
    public Task ExecuteAsync<TDefinition>(TDefinition definition, CancellationToken cancellationToken)
        where TDefinition : IAsyncStrategyDefinition
    {
        var strategy = serviceProvider.GetRequiredKeyedService<IAsyncStrategy<TDefinition>>(definition.Key);
        return strategy.ExecuteAsync(definition, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TReturn> ExecuteAsync<TReturn>(IAsyncStrategyDefinition<TReturn> definition, CancellationToken cancellationToken = default)
    {
        var strategyType = typeof(IAsyncStrategy<,>).MakeGenericType(definition.GetType(), typeof(TReturn));
        var strategy = serviceProvider.GetRequiredKeyedService(strategyType, definition.Key);
        
        var executeMethod = strategyType.GetMethod(nameof(IAsyncStrategy<,>.ExecuteAsync)) ?? throw new InvalidOperationException($"Strategy '{strategyType}' does not contain method with name '{nameof(IAsyncStrategy<,>.ExecuteAsync)}'. This in an internal error, please report this error on GitHub");
        var task = executeMethod.Invoke(strategy, [definition, cancellationToken]) as Task<TReturn> ?? throw new InvalidCastException($"Invoking strategy of type '{strategyType}' should have returned a {typeof(Task<TReturn>)}.");;
        var result = await task;

        if (result is not TReturn castedResult)
            throw new InvalidCastException($"Result of strategy '{strategyType}' is of type '{result?.GetType()}' but a result of type '{typeof(TReturn)}' was expected.");

        return castedResult;
    }
}
