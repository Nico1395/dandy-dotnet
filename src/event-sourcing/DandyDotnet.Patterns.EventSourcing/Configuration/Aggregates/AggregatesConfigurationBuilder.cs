namespace DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;

/// <summary>
///     Provides a builder for configuring aggregates registered within the Event Sourcing subsystem.
/// </summary>
public sealed class AggregatesConfigurationBuilder
{
    private readonly AggregatesConfiguration _configuration = new();

    /// <summary>
    ///     Registers and configures an aggregate root type.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate root type to register.</typeparam>
    /// <param name="builderAction">An action to configure aggregate options such as key, snapshots, or custom factories.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public AggregatesConfigurationBuilder AddAggregate<TAggregate>(Action<AggregateConfigurationBuilder<TAggregate>> builderAction)
        where TAggregate : class
    {
        var builder = new AggregateConfigurationBuilder<TAggregate>();
        builderAction(builder);
        var configuration = builder.Build();

        _configuration.AggregateConfigsByType[configuration.RuntimeType] = configuration;
        _configuration.AggregateConfigsByKey[configuration.Key] = configuration;

        return this;
    }

    internal AggregatesConfiguration Build()
    {
        return _configuration;
    }
}