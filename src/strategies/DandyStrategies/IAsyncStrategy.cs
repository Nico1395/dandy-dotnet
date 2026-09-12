namespace DandyStrategies;

public interface IAsyncStrategy<in TDefinition>
    where TDefinition : IAsyncStrategyDefinition
{
    Task ExecuteAsync(TDefinition definition, CancellationToken cancellationToken);
}

public interface IAsyncStrategy<in TDefinition, TReturn>
    where TDefinition : IAsyncStrategyDefinition<TReturn>
{
    Task<TReturn> ExecuteAsync(TDefinition definition, CancellationToken cancellationToken);
}
