using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Http.StaticEndpoints;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStaticEndpoints(this IServiceCollection services, Action<StaticEndpointsConfigurationBuilder>? builderAction = null)
    {
        var builder = new StaticEndpointsConfigurationBuilder();
        builderAction?.Invoke(builder);
        var configuration = builder.Build();

        services.AddSingleton(configuration);
        services.AddSingleton<IStaticEndpointMapper, StaticEndpointMapper>();

        return services;
    }
}