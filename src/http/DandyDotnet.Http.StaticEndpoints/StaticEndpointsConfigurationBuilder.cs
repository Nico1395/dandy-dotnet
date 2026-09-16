using System.Reflection;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DandyDotnet.Http.StaticEndpoints;

/// <summary>
///     Builder for creating <see cref="StaticEndpointsConfiguration" /> instances.
/// </summary>
/// <remarks>
///     <para>
///         This builder provides a fluent API for configuring which assemblies to scan for static endpoint
///         methods. Use the <see cref="ScanInAssemblies" /> method to specify assemblies, then call
///         <see cref="Build" /> to create the final configuration.
///     </para>
/// </remarks>
public sealed class StaticEndpointsConfigurationBuilder
{
    private readonly StaticEndpointsConfiguration _configuration = new();

    /// <summary>
    ///     Adds assemblies to scan for static endpoint methods.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The specified assemblies will be scanned for classes containing static methods marked with
    ///         <see cref="HttpMethodAttribute" />. These methods will be registered as HTTP endpoints.
    ///     </para>
    /// </remarks>
    /// <param name="assemblies">The assemblies to scan for static endpoint methods.</param>
    /// <returns>The same <see cref="StaticEndpointsConfigurationBuilder" /> instance for method chaining.</returns>
    public StaticEndpointsConfigurationBuilder ScanInAssemblies(params IEnumerable<Assembly> assemblies)
    {
        _configuration.Assemblies.AddRange(assemblies);
        return this;
    }

    /// <summary>
    ///     Builds the final <see cref="StaticEndpointsConfiguration" /> with the configured settings.
    /// </summary>
    /// <returns>A <see cref="StaticEndpointsConfiguration" /> instance with the configured assemblies and methods.</returns>
    public StaticEndpointsConfiguration Build()
    {
        return _configuration;
    }
}