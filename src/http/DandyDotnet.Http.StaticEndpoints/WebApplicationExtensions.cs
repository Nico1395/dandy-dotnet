using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Http.StaticEndpoints;

public static class WebApplicationExtensions
{
    public static void MapStaticEndpoints(this WebApplication app)
    {
        var mapper = app.Services.GetService<IStaticEndpointMapper>();
        if (mapper == null)
            throw new InvalidOperationException("Static endpoints were not configured.");

        mapper.MapStaticEndpoints(app);
    }
}