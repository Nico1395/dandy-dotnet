namespace DandyDotnet.Serialization.Abstractions;

/// <summary>
///     Defines the contract for a serializer that can convert objects to and from string representations.
/// </summary>
/// <remarks>
///     <para>
///         This interface provides a generic way to serialize objects to string format and deserialize strings back to objects.
///         It is designed to be implementation-agnostic, allowing different serialization libraries (such as Newtonsoft.Json or System.Text.Json)
///         to be used interchangeably.
///     </para>
///     <para>
///         Implementations should handle all necessary type information and manage their own configuration.
///     </para>
/// </remarks>
public interface ISerializer
{
    /// <summary>
    ///     Serializes the specified object to a string representation.
    /// </summary>
    /// <param name="item">The object to serialize. Can be <see langword="null" />.</param>
    /// <param name="type">
    ///     The type to use for serialization. If <see langword="null" />, the runtime type of the object will be used.
    ///     This allows serialization of derived types using the base type information.
    /// </param>
    /// <returns>A string representation of the serialized object.</returns>
    /// <remarks>
    ///     <para>
    ///         The returned string format depends on the underlying serializer implementation.
    ///         Most implementations will return JSON-formatted strings.
    ///     </para>
    ///     <para>
    ///         If <paramref name="item" /> is <see langword="null" />, implementations may return an empty string, a null representation,
    ///         or throw an exception, depending on their specific behavior and configuration.
    ///     </para>
    /// </remarks>
    string Serialize(object item, Type? type);

    /// <summary>
    ///     Deserializes the specified string payload into an object of the specified type.
    /// </summary>
    /// <param name="payload">The string payload to deserialize. Can be <see langword="null" /> or empty.</param>
    /// <param name="type">The type to deserialize the payload into. Must not be <see langword="null" />.</param>
    /// <returns>
    ///     An instance of <paramref name="type" /> populated with the data from the payload, or <see langword="null" />
    ///     if the payload represents a null value.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="payload" /> is <see langword="null" /> or empty and cannot be deserialized.
    /// </exception>
    /// <exception cref="JsonException">
    ///     Thrown when the payload is not valid for the specified type (specific to JSON-based implementations).
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         The exact exceptions thrown depend on the underlying serializer implementation.
    ///         JSON-based implementations typically throw <see cref="JsonException" /> for invalid JSON or type mismatches.
    ///     </para>
    ///     <para>
    ///         The method does not guarantee that the returned object will be of the exact type specified.
    ///         Callers should validate the result if strict type checking is required.
    ///     </para>
    /// </remarks>
    object? Deserialize(string payload, Type type);
}