using Microsoft.AspNetCore.Builder;

namespace DandyDotnet.Http.StaticEndpoints;

public interface IStaticEndpointMapper
{
    void MapStaticEndpoints(WebApplication app);
}