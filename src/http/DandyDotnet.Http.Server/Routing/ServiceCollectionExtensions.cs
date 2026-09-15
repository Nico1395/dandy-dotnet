using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Http.Server.Endpoints;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDandyEndpoints(this IServiceCollection services, Action<EndpointsConfigurationBuilder>? builderAction = null)
    {
        var builder = new EndpointsConfigurationBuilder();
        builderAction?.Invoke(builder);
        var configuration = builder.Build();

        services.AddSingleton(configuration);

        return services;
    }
}