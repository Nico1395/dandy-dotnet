using System.Diagnostics;
using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Aggregates;

/// <summary>
///     Provides a fluent builder for configuring a specific aggregate root type.
/// </summary>
/// <typeparam name="TAggregate">The aggregate root type being configured.</typeparam>
public sealed class AggregateConfigurationBuilder<TAggregate>
    where TAggregate : class
{
    private readonly AggregateConfiguration _configuration = new()
    {
        Key = typeof(TAggregate).Name,
        RuntimeType = typeof(TAggregate),
    };

    /// <summary>
    ///     Sets the custom key used to identify the aggregate in snapshots and event streams.
    /// </summary>
    /// <param name="key">The aggregate key name.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public AggregateConfigurationBuilder<TAggregate> WithKey(string key)
    {
        _configuration.Key = key;
        return this;
    }

    /// <summary>
    ///     Enables automatic snapshotting for the aggregate with the specified version interval.
    /// </summary>
    /// <param name="interval">The number of events between snapshot creations.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public AggregateConfigurationBuilder<TAggregate> UseSnapshots(int interval)
    {
        _configuration.SnapshotInterval = interval;
        return this;
    }

    /// <summary>
    ///     Configures an inline factory delegate to instantiate or reconstruct the aggregate from its snapshot and event stream.
    /// </summary>
    /// <param name="factoryFunc">
    ///     A delegate that accepts the optional previous snapshot state and the list of stream envelopes,
    ///     and returns the reconstructed aggregate instance.
    /// </param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public AggregateConfigurationBuilder<TAggregate> UseFactory(Func<TAggregate?, IReadOnlyEnvelope[], TAggregate> factoryFunc)
    {
        _configuration.FactoryFunc = (aggregate, envelopes) =>
        {
            return aggregate?.GetType() == _configuration.RuntimeType
                ? factoryFunc((TAggregate?)aggregate, envelopes)
                : throw new UnreachableException($"Aggregate {aggregate?.GetType().Name} is not of type {typeof(TAggregate).Name}.");
        };

        return this;
    }

    internal AggregateConfiguration Build()
    {
        return _configuration;
    }
}