using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.Serialization.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Serialization;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerializer(this IServiceCollection services, Action<SerializerConfigurationBuilder>? configure)
    {
        var builder = new SerializerConfigurationBuilder();
        configure?.Invoke(builder);
        var configuration = builder.Build();

        return services.AddSerializer(configuration);
    }

    public static IServiceCollection AddSerializer(this IServiceCollection services)
    {
        return services.AddSerializer(configure: null);
    }

    public static IServiceCollection AddSerializer(this IServiceCollection services, SerializerConfiguration configuration)
    {
        if (!configuration.IsValid())
            throw new InvalidOperationException("Serializer type is not specified.");

        if (services.BuildServiceProvider().GetKeyedOrDefaultService(typeof(ISerializer), configuration.ServiceKey) != null)
            return services;

        services.AddKeyedSingletonOrDefault(typeof(ISerializer), configuration.ServiceKey, configuration.SerializerType);
        services.AddKeyedSingletonOrDefault(configuration.Configuration.GetType(), configuration.ServiceKey, configuration.Configuration);

        return services;
    }
}