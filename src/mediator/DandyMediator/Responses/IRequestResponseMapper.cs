namespace DandyMediator.Responses;

/// <summary>
/// Resolves concrete response implementation types.
/// </summary>
public interface IRequestResponseMapper
{
    /// <summary>
    /// Gets the implementation type for an abstract response type.
    /// </summary>
    /// <param name="abstractResponseType">Abstract response type.</param>
    /// <returns>Concrete response implementation type.</returns>
    Type GetImplementationTypeFor(Type abstractResponseType);
}