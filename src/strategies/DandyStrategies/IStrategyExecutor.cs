namespace DandyStrategies;

public interface IStrategyExecutor
{
    void Execute<TDefinition>(TDefinition definition) where TDefinition : IStrategyDefinition;
    TReturn Execute<TReturn>(IStrategyDefinition<TReturn> definition);
    Task ExecuteAsync<TDefinition>(TDefinition definition, CancellationToken cancellationToken = default) where TDefinition : IAsyncStrategyDefinition;
    Task<TReturn> ExecuteAsync<TReturn>(IAsyncStrategyDefinition<TReturn> definition, CancellationToken cancellationToken = default);
}
