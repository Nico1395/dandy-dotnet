using System.Text.Json;

namespace DandyDotnet.Serialization.SystemTextJson;

/// <summary>
///     Configuration for the System.Text.Json serializer.
/// </summary>
/// <remarks>
///     <para>
///         This class provides configuration options specific to the System.Text.Json serializer implementation.
///         The main configuration is through the <see cref="JsonSerializerOptions" /> property, which allows full control
///         over how JSON serialization and deserialization is performed.
///     </para>
///     <para>
///         Use the extension methods in <see cref="SerializerConfigurationBuilderExtensions" /> to easily configure
///         and register the System.Text.Json serializer with the dependency injection container.
///     </para>
/// </remarks>
/// <example>
///     <code>
///         services.AddSerializer(builder => builder
///             .UseSystemTextJson(config =>
///             {
///                 config.JsonSerializerOptions = new JsonSerializerOptions
///                 {
///                     PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
///                     WriteIndented = true,
///                     DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
///                 };
///             }));
///     </code>
/// </example>
public sealed class SystemTextJsonConfiguration
{
    /// <summary>
    ///     Gets or sets the JSON serializer options for the System.Text.Json serializer.
    /// </summary>
    /// <value>
    ///     A <see cref="JsonSerializerOptions" /> instance that controls the behavior of JSON serialization and deserialization.
    ///     If <see langword="null" />, default options will be used.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         This property allows full customization of the System.Text.Json behavior. Common options include:
    ///     </para>
    ///     <list type="bullet">
    ///         <item><description><see cref="JsonSerializerOptions.PropertyNamingPolicy" />: Controls property naming (e.g., CamelCase, SnakeCase).</description></item>
    ///         <item><description><see cref="JsonSerializerOptions.WriteIndented" />: Controls whether the output is indented.</description></item>
    ///         <item><description><see cref="JsonSerializerOptions.DefaultIgnoreCondition" />: Controls how null values are handled.</description></item>
    ///         <item><description><see cref="JsonSerializerOptions.ReferenceHandler" />: Controls how object references are handled.</description></item>
    ///         <item><description><see cref="JsonSerializerOptions.Converters" />: Custom JSON converters for specific types.</description></item>
    ///         <item><description><see cref="JsonSerializerOptions.PropertyNameCaseInsensitive" />: Controls case sensitivity during deserialization.</description></item>
    ///     </list>
    ///     <para>
    ///         If not set, the serializer will use the default <see cref="JsonSerializerOptions" />.
    ///     </para>
    ///     <para>
    ///         Note that System.Text.Json has some differences in behavior compared to Newtonsoft.Json:
    ///         <list type="bullet">
    ///             <item><description>By default, it does not support polymorphic serialization without custom converters.</description></item>
    ///             <item><description>By default, it throws on circular references without a ReferenceHandler.</description></item>
    ///             <item><description>Property names are case-sensitive by default.</description></item>
    ///         </list>
    ///     </para>
    /// </remarks>
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
}