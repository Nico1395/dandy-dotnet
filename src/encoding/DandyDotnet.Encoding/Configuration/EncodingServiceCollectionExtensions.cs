using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Encoding.Configuration;

public static class EncodingServiceCollectionExtensions
{
    public static IServiceCollection AddDandyEncoder(this IServiceCollection services, Action<EncodingConfigurationBuilder>? action)
    {
        var builder = new EncodingConfigurationBuilder();
        action?.Invoke(builder);
        var configuration = builder.Build();
        return services.AddDandyEncoder(configuration);
    }

    public static IServiceCollection AddDandyEncoder(this IServiceCollection services)
    {
        return services.AddDandyEncoder(action: null);
    }

    public static IServiceCollection AddDandyEncoder(this IServiceCollection services, EncodingConfiguration configuration)
    {
        if (services.BuildServiceProvider().GetService(typeof(IEncoder)) != null)
            return services;

        if (configuration.EncoderType.IsAbstract)
            throw new InvalidOperationException("Encoder implementation type is abstract.");

        var payloadEncoderInterface = typeof(IEncoder);
        if (!configuration.EncoderType.IsAssignableTo(payloadEncoderInterface))
            throw new InvalidOperationException($"Encoder implementation type does not implement {payloadEncoderInterface}.");

        return services.AddSingleton(payloadEncoderInterface, configuration.EncoderType);
    }
}
