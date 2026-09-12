namespace DandyMediator.Responses;

/// <summary>
/// Maps an abstract response type to its concrete implementation type.
/// </summary>
/// <remarks>
///     <para>
///         Both <see cref="GenericAbstractType"/> and <see cref="GenericImplementationType"/> are assumed to be generic types.
///     </para>
/// </remarks>
public interface IRequestResponseMap
{
    /// <summary>
    /// Abstract response type.
    /// </summary>
    Type GenericAbstractType { get; }

    /// <summary>
    /// Concrete response implementation type.
    /// </summary>
    Type GenericImplementationType { get; }
}