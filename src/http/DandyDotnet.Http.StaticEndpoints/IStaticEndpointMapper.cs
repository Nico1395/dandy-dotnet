using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Routing;

namespace DandyDotnet.Http.StaticEndpoints;

/// <summary>
///     Defines a service that maps static endpoints to an <see cref="WebApplication" />.
/// </summary>
/// <remarks>
///     <para>
///         Implementations of this interface scan for static methods marked with <see cref="HttpMethodAttribute" />
///         and register them as HTTP endpoints with the application.
///     </para>
/// </remarks>
public interface IStaticEndpointMapper
{
    /// <summary>
    ///     Maps discovered static endpoints to the specified <see cref="WebApplication" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method discovers static endpoint methods (either through assembly scanning or explicitly configured)
    ///         and registers them as routes in the <see cref="WebApplication" /> based on their
    ///         <see cref="HttpMethodAttribute" /> metadata.
    ///     </para>
    /// </remarks>
    /// <param name="app">The <see cref="WebApplication" /> to map endpoints to.</param>
    void MapStaticEndpoints(WebApplication app);
}