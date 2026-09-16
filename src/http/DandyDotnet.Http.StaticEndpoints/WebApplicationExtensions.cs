using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Http.StaticEndpoints;

/// <summary>
///     Extension methods for mapping static endpoints in an <see cref="WebApplication" />.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    ///     Maps static endpoints registered in the service collection to the <see cref="WebApplication" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method retrieves the registered <see cref="IStaticEndpointMapper" /> from the service collection
    ///         and invokes it to map all discovered static endpoints to the application.
    ///     </para>
    ///     <para>
    ///         Ensure that <see cref="ServiceCollectionExtensions.AddStaticEndpoints" /> has been called during
    ///         service configuration before invoking this method.
    ///     </para>
    /// </remarks>
    /// <param name="app">The <see cref="WebApplication" /> to map endpoints to.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when static endpoints have not been configured (the <see cref="IStaticEndpointMapper" /> service
    ///     is not registered in the service collection).
    /// </exception>
    public static void MapStaticEndpoints(this WebApplication app)
    {
        var mapper = app.Services.GetService<IStaticEndpointMapper>();
        if (mapper == null)
            throw new InvalidOperationException("Static endpoints were not configured.");

        mapper.MapStaticEndpoints(app);
    }
}