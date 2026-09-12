namespace DandyMediator.Responses;

/// <inheritdoc/>
public class RequestResponseMap(Type genericAbstractType, Type genericImplementationType) : IRequestResponseMap
{
    /// <inheritdoc/>
    public Type GenericAbstractType => genericAbstractType;
    
    /// <inheritdoc/>
    public Type GenericImplementationType => genericImplementationType;
}