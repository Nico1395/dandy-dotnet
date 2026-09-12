using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Serialization.Abstractions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDandySerializer(this IServiceCollection services, Action<SerializerConfigurationBuilder> configure)
    {
        var builder = new SerializerConfigurationBuilder();
        configure(builder);
        var configuration = builder.Build();

        if (!configuration.IsValid())
            throw new InvalidOperationException("Serializer type is not specified.");

        if (services.BuildServiceProvider().GetService(configuration.Type) != null)
            return services;

        services.AddSingleton(typeof(ISerializer), configuration.Type);
        services.AddSingleton(configuration.Configuration.GetType(), configuration.Configuration);

        return services;
    }
}