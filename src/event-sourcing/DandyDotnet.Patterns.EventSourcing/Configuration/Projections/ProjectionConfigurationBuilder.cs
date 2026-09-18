using System.Diagnostics;
using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Configuration.Projections;

public sealed class ProjectionConfigurationBuilder<TProjection>
    where TProjection : class
{
    private readonly ProjectionConfiguration _configuration = new()
    {
        Key = typeof(TProjection).Name,
        RuntimeType = typeof(TProjection),
    };

    public ProjectionConfigurationBuilder<TProjection> WithKey(string key)
    {
        _configuration.Key = key;
        return this;
    }

    public ProjectionConfigurationBuilder<TProjection> WithMode(ProjectionMode mode)
    {
        _configuration.Mode = mode;
        return this;
    }
    
    public ProjectionConfigurationBuilder<TProjection> UseFactory(Func<TProjection?, IReadOnlyEnvelope[], TProjection> factoryFunc)
    {
        _configuration.FactoryFunc = (projection, envelopes) =>
        {
            return projection?.GetType() == _configuration.RuntimeType
                ? factoryFunc((TProjection?)projection, envelopes)
                : throw new UnreachableException($"Projection {projection?.GetType().Name} is not of type {typeof(TProjection).Name}.");
        };

        return this;
    }

    public ProjectionConfigurationBuilder<TProjection> WithKeyProperties(Func<TProjection, IEnumerable<(string Name, object Value)>> factoryFunc)
    {
        _configuration.KeyFactoryFunc = (projection) =>
        {
            return projection is TProjection casted
                ? factoryFunc(casted)
                : throw new UnreachableException($"Projection {projection?.GetType().Name} is not of type {typeof(TProjection).Name}.");
        };

        return this;
    }

    internal ProjectionConfiguration Build()
    {
        return _configuration;
    }
}