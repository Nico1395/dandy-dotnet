using DandyDotnet.Patterns.Mediator.Abstractions.Requests.Mapping;

namespace DandyDotnet.Patterns.Mediator.Requests.Mapping;

/// <inheritdoc/>
public class RequestResponseMap(Type genericAbstractType, Type genericImplementationType) : IRequestResponseMap
{
    /// <inheritdoc/>
    public Type GenericAbstractType => genericAbstractType;
    
    /// <inheritdoc/>
    public Type GenericImplementationType => genericImplementationType;
}