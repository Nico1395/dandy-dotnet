using System.Text.Json;
using DandyDotnet.Serialization.Abstractions;

namespace DandyDotnet.Serialization.SystemTextJson;

/// <summary>
///     A serializer implementation that uses System.Text.Json for serialization and deserialization.
/// </summary>
/// <remarks>
///     <para>
///         This class implements <see cref="Abstractions.ISerializer" /> using the System.Text.Json library.
///         It is marked as internal and is primarily intended to be used through the extension methods in
///         <see cref="SerializerConfigurationBuilderExtensions" />.
///     </para>
///     <para>
///         The serializer uses the provided <see cref="SystemTextJsonConfiguration" /> to customize serialization behavior.
///         The configuration includes <see cref="SystemTextJsonConfiguration.JsonSerializerOptions" /> which allows
///         full control over the JSON serialization process.
///     </para>
/// </remarks>
internal sealed class SystemTextJsonSerializer(SystemTextJsonConfiguration configuration) : ISerializer
{
    private readonly SystemTextJsonConfiguration _configuration = configuration;

    /// <summary>
    ///     Serializes the specified object to a JSON string.
    /// </summary>
    /// <param name="item">The object to serialize. Can be <see langword="null" />.</param>
    /// <param name="type">
    ///     The type to use for serialization. If <see langword="null" />, the runtime type of the object will be used.
    /// </param>
    /// <returns>A JSON string representation of the serialized object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when both <paramref name="item" /> and <paramref name="type" /> are <see langword="null" />.</exception>
    /// <exception cref="JsonException">
    ///     Thrown when an error occurs during serialization (e.g., circular reference detected, unsupported type).
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method uses <see cref="JsonSerializer.Serialize(object, Type, JsonSerializerOptions)" /> for serialization.
    ///         The behavior depends on the configured <see cref="SystemTextJsonConfiguration.JsonSerializerOptions" />.
    ///     </para>
    ///     <para>
    ///         For <see langword="null" /> objects, the output depends on the DefaultIgnoreCondition setting in the configuration.
    ///         By default, System.Text.Json will throw an exception when trying to serialize null values.
    ///     </para>
    /// </remarks>
    public string Serialize(object item, Type? type)
    {
        type ??= item.GetType();
        return JsonSerializer.Serialize(item, type, _configuration.JsonSerializerOptions);
    }

    /// <summary>
    ///     Deserializes the specified JSON string into an object of the specified type.
    /// </summary>
    /// <param name="payload">The JSON string to deserialize. Can be <see langword="null" /> or empty.</param>
    /// <param name="type">The type to deserialize the JSON into. Must not be <see langword="null" />.</param>
    /// <returns>
    ///     An instance of <paramref name="type" /> populated with the data from the JSON string, or <see langword="null" />
    ///     if the JSON represents a null value.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="payload" /> is <see langword="null" /> or empty.</exception>
    /// <exception cref="JsonException">
    ///     Thrown when an error occurs during deserialization (e.g., JSON is invalid, type is not compatible).
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     Thrown when the type cannot be deserialized (e.g., abstract class without custom converter).
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method uses <see cref="JsonSerializer.Deserialize(string, Type, JsonSerializerOptions)" /> for deserialization.
    ///         The behavior depends on the configured <see cref="SystemTextJsonConfiguration.JsonSerializerOptions" />.
    ///     </para>
    ///     <para>
    ///         If the JSON string represents a null value, this method returns <see langword="null" />.
    ///         Otherwise, it creates a new instance of the specified type and populates it with data from the JSON.
    ///     </para>
    /// </remarks>
    public object? Deserialize(string payload, Type type)
    {
        return JsonSerializer.Deserialize(payload, type, _configuration.JsonSerializerOptions);
    }
}