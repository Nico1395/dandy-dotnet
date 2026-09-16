namespace DandyDotnet.Serialization.Abstractions;

/// <summary>
///     Extension methods for <see cref="ISerializer" /> that provide strongly-typed serialization and deserialization.
/// </summary>
/// <remarks>
///     <para>
///         These extensions simplify the usage of <see cref="ISerializer" /> by providing generic methods that infer type information
///         from the generic type parameters, reducing the need for manual type specification.
///     </para>
///     <para>
///         The <see cref="Serialize(ISerializer, object)" /> extension automatically uses the runtime type of the object,
///         while <see cref="Deserialize{T}(ISerializer, string)" /> uses the generic type parameter.
///     </para>
/// </remarks>
public static class SerializerExtensions
{
    /// <summary>
    ///     Serializes the specified object to a string representation using its runtime type.
    /// </summary>
    /// <typeparam name="T">The type of the object being serialized. This is inferred from the <paramref name="item" /> parameter.</typeparam>
    /// <param name="serializer">The serializer instance to use for serialization.</param>
    /// <param name="item">The object to serialize. Can be <see langword="null" />.</param>
    /// <returns>A string representation of the serialized object.</returns>
    /// <remarks>
    ///     <para>
    ///         This method is a convenience wrapper around <see cref="ISerializer.Serialize(object, Type?)" /> that automatically passes
    ///         the runtime type of the <paramref name="item" /> as the type parameter.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serializer" /> is <see langword="null" />.</exception>
    public static string Serialize(this ISerializer serializer, object item)
    {
        return serializer.Serialize(item, item.GetType());
    }

    /// <summary>
    ///     Deserializes the specified string payload into an object of the specified generic type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the payload into.</typeparam>
    /// <param name="serializer">The serializer instance to use for deserialization.</param>
    /// <param name="payload">The string payload to deserialize. Can be <see langword="null" /> or empty.</param>
    /// <returns>
    ///     An instance of type <typeparamref name="T" /> populated with the data from the payload, or <see langword="null" />
    ///     if the payload represents a null value or if the deserialized object cannot be cast to type <typeparamref name="T" />.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         This method is a convenience wrapper around <see cref="ISerializer.Deserialize(string, Type)" /> that automatically passes
    ///         <c>typeof(T)</c> as the type parameter and attempts to cast the result to type <typeparamref name="T" />.
    ///     </para>
    ///     <para>
    ///         If the deserialization succeeds but the result cannot be cast to type <typeparamref name="T" />, this method returns
    ///         <see langword="default" /> for the type (which is <see langword="null" /> for reference types).
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="serializer" /> is <see langword="null" />.</exception>
    public static T? Deserialize<T>(this ISerializer serializer, string payload)
    {
        return serializer.Deserialize(payload, typeof(T)) is T casted ? casted : default;
    }
}