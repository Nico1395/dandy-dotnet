using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using DandyDotnet.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;

/// <summary>
/// Provides dependency injection registration extensions for the producer.
/// </summary>
public static class ProducerServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures DandyRabbitMQ producer <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="builderAction">An action that configures the producer.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRabbitMQProducer(this IServiceCollection services, Action<ProducerConfigurationBuilder> builderAction)
    {
        var builder = new ProducerConfigurationBuilder();
        builderAction.Invoke(builder);
        var configuration = builder.Build();

        services.AddSingleton(configuration);
        services.AddScoped<IProducer, Producer>();

        services.AddRabbitMQConnectivity(configuration.ConnectivityConfigurationBuilder.Build());
        services.AddRabbitMQMessages(configuration.MessagesConfigurationBuilder.Build());
        services.AddRabbitMQDeclarations(configuration.DeclarationsConfiguration.Build());

        if (configuration.SerializerConfiguration != null)
            services.AddSerialization(configuration.SerializerConfiguration);

        if (configuration.EncodingConfiguration != null)
            services.AddEncoder(configuration.EncodingConfiguration);

        return services;
    }
}
