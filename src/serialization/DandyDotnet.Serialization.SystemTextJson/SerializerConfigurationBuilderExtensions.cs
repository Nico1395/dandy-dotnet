using System.Text.Json;
using DandyDotnet.Serialization.Abstractions;

namespace DandyDotnet.Serialization.SystemTextJson;

/// <summary>
///     Extension methods for <see cref="SerializerConfigurationBuilder" /> to configure System.Text.Json serializer.
/// </summary>
/// <remarks>
///     <para>
///         These extensions provide convenient ways to configure and register the System.Text.Json serializer
///         with the dependency injection container.
///     </para>
///     <para>
///         They simplify the process by handling the creation of the <see cref="SystemTextJsonConfiguration" />
///         and the proper serializer type registration.
///     </para>
/// </remarks>
public static class SerializerConfigurationBuilderExtensions
{
    /// <summary>
    ///     Configures the serializer to use System.Text.Json with the specified configuration.
    /// </summary>
    /// <param name="configurationBuilder">The serializer configuration builder to extend.</param>
    /// <param name="configuration">The System.Text.Json configuration to use.</param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurationBuilder" /> is <see langword="null" />.</exception>
    /// <remarks>
    ///     <para>
    ///         This method sets up the serializer to use <see cref="SystemTextJsonSerializer" /> with the provided
    ///         <see cref="SystemTextJsonConfiguration" />.
    ///     </para>
    /// </remarks>
    public static SerializerConfigurationBuilder UseSystemTextJson(this SerializerConfigurationBuilder configurationBuilder, SystemTextJsonConfiguration configuration)
    {
        return configurationBuilder.UseSerializer(typeof(SystemTextJsonSerializer), configuration);
    }

    /// <summary>
    ///     Configures the serializer to use System.Text.Json with the specified configuration action.
    /// </summary>
    /// <param name="configurationBuilder">The serializer configuration builder to extend.</param>
    /// <param name="action">
    ///     An action that configures the <see cref="SystemTextJsonConfiguration" />.
    ///     If <see langword="null" />, a default configuration is created.
    /// </param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurationBuilder" /> is <see langword="null" />.</exception>
    /// <remarks>
    ///     <para>
    ///         This method creates a new <see cref="SystemTextJsonConfiguration" />, applies the provided action to it,
    ///         and then configures the serializer to use System.Text.Json with that configuration.
    ///     </para>
    ///     <para>
    ///         This is the recommended way to configure the System.Text.Json serializer as it provides a fluent API
    ///         for setting up all the required options.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    ///         services.AddSerializer(builder => builder
    ///             .UseServiceKey("myKey")
    ///             .UseSystemTextJson(config =>
    ///             {
    ///                 config.JsonSerializerOptions = new JsonSerializerOptions
    ///                 {
    ///                     PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    ///                     WriteIndented = true,
    ///                     ReferenceHandler = ReferenceHandler.IgnoreCycles
    ///                 };
    ///             }));
    ///     </code>
    /// </example>
    public static SerializerConfigurationBuilder UseSystemTextJson(this SerializerConfigurationBuilder configurationBuilder, Action<SystemTextJsonConfiguration>? action)
    {
        var configuration = new SystemTextJsonConfiguration();
        action?.Invoke(configuration);
        return configurationBuilder.UseSystemTextJson(configuration);
    }

    /// <summary>
    ///     Configures the serializer to use System.Text.Json with default configuration.
    /// </summary>
    /// <param name="configurationBuilder">The serializer configuration builder to extend.</param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurationBuilder" /> is <see langword="null" />.</exception>
    /// <remarks>
    ///     <para>
    ///         This method configures the serializer to use System.Text.Json with a new, default
    ///         <see cref="SystemTextJsonConfiguration" /> (where <see cref="SystemTextJsonConfiguration.JsonSerializerOptions" /> is <see langword="null" />).
    ///     </para>
    ///     <para>
    ///         This is a convenience method for when the default System.Text.Json options are sufficient.
    ///     </para>
    /// </remarks>
    public static SerializerConfigurationBuilder UseSystemTextJson(this SerializerConfigurationBuilder configurationBuilder)
    {
        return configurationBuilder.UseSystemTextJson(action: null);
    }

    /// <summary>
    ///     Configures the serializer to use System.Text.Json with the specified JSON serializer options.
    /// </summary>
    /// <param name="configurationBuilder">The serializer configuration builder to extend.</param>
    /// <param name="options">The JSON serializer options to use.</param>
    /// <returns>The same <see cref="SerializerConfigurationBuilder" /> instance for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="configurationBuilder" /> or <paramref name="options" /> is <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This is a convenience method for when you already have a prepared <see cref="JsonSerializerOptions" /> instance.
    ///     </para>
    ///     <para>
    ///         It creates a new <see cref="SystemTextJsonConfiguration" /> and sets its
    ///         <see cref="SystemTextJsonConfiguration.JsonSerializerOptions" /> property to the provided value.
    ///     </para>
    /// </remarks>
    public static SerializerConfigurationBuilder UseSystemTextJson(this SerializerConfigurationBuilder configurationBuilder, JsonSerializerOptions options)
    {
        return configurationBuilder.UseSystemTextJson(cfg => cfg.JsonSerializerOptions = options);
    }
}