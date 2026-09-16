using DandyDotnet.Serialization.Abstractions;
using Newtonsoft.Json;

namespace DandyDotnet.Serialization.NewtonsoftJson;

/// <summary>
///     Extension methods for <see cref="SerializerConfigurationBuilder" /> to configure Newtonsoft.Json serializer.
/// </summary>
/// <remarks>
///     <para>
///         These extensions provide convenient ways to configure and register the Newtonsoft.Json serializer
///         with the dependency injection container.
///     </para>
///     <para>
///         They simplify the process by handling the creation of the <see cref="NewtonsoftJsonConfiguration" />
///         and the proper serializer type registration.
///     </para>
/// </remarks>
public static class SerializerConfigurationBuilderExtensions
{
    /// <summary>
    ///     Configures the serializer to use Newtonsoft.Json with the specified configuration.
    /// </summary>
    /// <param name="configurationBuilder">The serializer configuration builder to extend.</param>
    /// <param name="configuration">The Newtonsoft.Json configuration to use.</param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurationBuilder" /> is <see langword="null" />.</exception>
    /// <remarks>
    ///     <para>
    ///         This method sets up the serializer to use <see cref="NewtonsoftJsonSerializer" /> with the provided
    ///         <see cref="NewtonsoftJsonConfiguration" />.
    ///     </para>
    /// </remarks>
    public static SerializerConfigurationBuilder UseNewtonsoftJson(this SerializerConfigurationBuilder configurationBuilder, NewtonsoftJsonConfiguration configuration)
    {
        return configurationBuilder.UseSerializer(typeof(NewtonsoftJsonSerializer), configuration);
    }

    /// <summary>
    ///     Configures the serializer to use Newtonsoft.Json with the specified configuration action.
    /// </summary>
    /// <param name="configurationBuilder">The serializer configuration builder to extend.</param>
    /// <param name="action">
    ///     An action that configures the <see cref="NewtonsoftJsonConfiguration" />.
    ///     If <see langword="null" />, a default configuration is created.
    /// </param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurationBuilder" /> is <see langword="null" />.</exception>
    /// <remarks>
    ///     <para>
    ///         This method creates a new <see cref="NewtonsoftJsonConfiguration" />, applies the provided action to it,
    ///         and then configures the serializer to use Newtonsoft.Json with that configuration.
    ///     </para>
    ///     <para>
    ///         This is the recommended way to configure the Newtonsoft.Json serializer as it provides a fluent API
    ///         for setting up all the required options.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    ///         services.AddSerializer(builder => builder
    ///             .UseServiceKey("myKey")
    ///             .UseNewtonsoftJson(config =>
    ///             {
    ///                 config.JsonSerializerSettings = new JsonSerializerSettings
    ///                 {
    ///                     NullValueHandling = NullValueHandling.Ignore,
    ///                     ContractResolver = new CamelCasePropertyNamesContractResolver()
    ///                 };
    ///             }));
    ///     </code>
    /// </example>
    public static SerializerConfigurationBuilder UseNewtonsoftJson(this SerializerConfigurationBuilder configurationBuilder, Action<NewtonsoftJsonConfiguration>? action)
    {
        var configuration = new NewtonsoftJsonConfiguration();
        action?.Invoke(configuration);
        return configurationBuilder.UseNewtonsoftJson(configuration);
    }

    /// <summary>
    ///     Configures the serializer to use Newtonsoft.Json with default configuration.
    /// </summary>
    /// <param name="configurationBuilder">The serializer configuration builder to extend.</param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurationBuilder" /> is <see langword="null" />.</exception>
    /// <remarks>
    ///     <para>
    ///         This method configures the serializer to use Newtonsoft.Json with a new, default
    ///         <see cref="NewtonsoftJsonConfiguration" /> (where <see cref="NewtonsoftJsonConfiguration.JsonSerializerSettings" /> is <see langword="null" />).
    ///     </para>
    ///     <para>
    ///         This is a convenience method for when the default Newtonsoft.Json settings are sufficient.
    ///     </para>
    /// </remarks>
    public static SerializerConfigurationBuilder UseNewtonsoftJson(this SerializerConfigurationBuilder configurationBuilder)
    {
        return configurationBuilder.UseNewtonsoftJson(action: null);
    }

    /// <summary>
    ///     Configures the serializer to use Newtonsoft.Json with the specified JSON serializer settings.
    /// </summary>
    /// <param name="configurationBuilder">The serializer configuration builder to extend.</param>
    /// <param name="settings">The JSON serializer settings to use.</param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="configurationBuilder" /> or <paramref name="settings" /> is <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This is a convenience method for when you already have a prepared <see cref="JsonSerializerSettings" /> instance.
    ///     </para>
    ///     <para>
    ///         It creates a new <see cref="NewtonsoftJsonConfiguration" /> and sets its
    ///         <see cref="NewtonsoftJsonConfiguration.JsonSerializerSettings" /> property to the provided value.
    ///     </para>
    /// </remarks>
    public static SerializerConfigurationBuilder UseNewtonsoftJson(this SerializerConfigurationBuilder configurationBuilder, JsonSerializerSettings settings)
    {
        return configurationBuilder.UseNewtonsoftJson(cfg => cfg.JsonSerializerSettings = settings);
    }
}