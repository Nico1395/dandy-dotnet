using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.Serialization.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DandyDotnet.Serialization.NewtonsoftJson;

/// <summary>
///     Configuration for the Newtonsoft.Json serializer.
/// </summary>
/// <remarks>
///     <para>
///         This class provides configuration options specific to the Newtonsoft.Json (Json.NET) serializer implementation.
///         The main configuration is through the <see cref="JsonSerializerSettings" /> property, which allows full control
///         over how JSON serialization and deserialization is performed.
///     </para>
///     <para>
///         Use the extension methods in <see cref="SerializerConfigurationBuilderExtensions" /> to easily configure
///         and register the Newtonsoft.Json serializer with the dependency injection container.
///     </para>
/// </remarks>
/// <example>
///     <code>
///         services.AddSerialization(builder => builder
///             .UseNewtonsoftJson(config =>
///             {
///                 config.JsonSerializerSettings = new JsonSerializerSettings
///                 {
///                     NullValueHandling = NullValueHandling.Ignore,
///                     Formatting = Formatting.Indented
///                 };
///             }));
///     </code>
/// </example>
public sealed class NewtonsoftJsonSerializerConfiguration : SerializerConfiguration
{
    /// <summary>
    ///     Gets or sets the JSON serializer settings for the Newtonsoft.Json serializer.
    /// </summary>
    /// <value>
    ///     A <see cref="JsonSerializerSettings" /> instance that controls the behavior of JSON serialization and deserialization.
    ///     If <see langword="null" />, default settings will be used.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         This property allows full customization of the Newtonsoft.Json behavior. Common settings include:
    ///     </para>
    ///     <list type="bullet">
    ///         <item><description><see cref="JsonSerializerSettings.NullValueHandling" />: Controls whether null values are included in the output.</description></item>
    ///         <item><description><see cref="JsonSerializerSettings.Formatting" />: Controls whether the output is indented.</description></item>
    ///         <item><description><see cref="JsonSerializerSettings.ContractResolver" />: Controls property naming, visibility, etc.</description></item>
    ///         <item><description><see cref="JsonSerializerSettings.Converters" />: Custom JSON converters for specific types.</description></item>
    ///         <item><description><see cref="JsonSerializerSettings.TypeNameHandling" />: Controls how type information is handled for polymorphic serialization.</description></item>
    ///     </list>
    ///     <para>
    ///         If not set, the serializer will use the default <see cref="JsonSerializerSettings" />.
    ///     </para>
    /// </remarks>
    public JsonSerializerSettings? JsonSerializerSettings { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureServices(SerializationConfiguration configuration, IServiceCollection services)
    {
        base.ConfigureServices(configuration, services);
        services.AddKeyedSingletonOrDefault<ISerializer>(configuration.ServiceKey, new NewtonsoftJsonSerializer(this));
    }
}