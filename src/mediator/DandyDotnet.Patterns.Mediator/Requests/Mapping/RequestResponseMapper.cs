using System.Collections.Concurrent;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests.Mapping;

namespace DandyDotnet.Patterns.Mediator.Requests.Mapping;

/// <inheritdoc/>
public class RequestResponseMapper(IEnumerable<IRequestResponseMap> maps) : IRequestResponseMapper
{
    private readonly ConcurrentDictionary<Type, Type> _cache = [];

    /// <inheritdoc/>
    public virtual Type GetImplementationTypeFor(Type abstractResponseType)
    {
        return _cache.GetOrAdd(abstractResponseType, type =>
        {
            var genericImplementationType = maps.FirstOrDefault(m => type.IsAssignableTo(m.GenericAbstractType))?.GenericImplementationType;
            if (genericImplementationType == null)
                throw new NotSupportedException($"Failed to resolve an implementation type for abstract response type '{abstractResponseType}'.");

            return abstractResponseType.IsGenericType
                ? genericImplementationType.MakeGenericType(abstractResponseType.GetGenericArguments())
                : genericImplementationType;
        });
    }
}