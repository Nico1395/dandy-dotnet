namespace DandyDotnet.Patterns.Mediator.Responses;

/// <inheritdoc/>
public class RequestResponseMap(Type genericAbstractType, Type genericImplementationType) : IRequestResponseMap
{
    /// <inheritdoc/>
    public Type GenericAbstractType => genericAbstractType;
    
    /// <inheritdoc/>
    public Type GenericImplementationType => genericImplementationType;
}