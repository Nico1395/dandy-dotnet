using System.Reflection;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DandyDotnet.Http.StaticEndpoints;

/// <summary>
///     Configuration for static endpoint discovery and mapping.
/// </summary>
/// <remarks>
///     <para>
///         This configuration is used by the <see cref="StaticEndpointMapper" /> to discover and map static endpoint
///         methods to HTTP routes.
///     </para>
///     <para>
///         Static endpoints are methods marked with an <see cref="HttpMethodAttribute" /> that are either:
///         <list type="bullet">
///             <item><description>Discovered through assembly scanning (via <see cref="Assemblies" />)</description></item>
///             <item><description>Explicitly specified (via <see cref="Methods" />)</description></item>
///         </list>
///     </para>
/// </remarks>
public sealed class StaticEndpointsConfiguration
{
    /// <summary>
    ///     Gets or sets the list of assemblies to scan for static endpoint methods.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The scanner looks for static methods marked with an <see cref="HttpMethodAttribute" /> in these assemblies.
    ///     </para>
    /// </remarks>
    /// <value>A list of <see cref="Assembly" /> instances to scan.</value>
    public List<Assembly> Assemblies { get; set; } = new();

    /// <summary>
    ///     Gets or sets the list of explicitly specified static endpoint methods to map.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         These methods are added to the list of discovered endpoints without requiring them to be found
    ///         through assembly scanning.
    ///     </para>
    /// </remarks>
    /// <value>A list of <see cref="MethodInfo" /> instances representing static endpoint methods.</value>
    public List<MethodInfo> Methods { get; set; } = new();
}