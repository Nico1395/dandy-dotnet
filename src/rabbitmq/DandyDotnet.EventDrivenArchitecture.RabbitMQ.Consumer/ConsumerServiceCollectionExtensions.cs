using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Connectivity;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Declarations;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Messages;
using DandyDotnet.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;

/// <summary>
/// Provides dependency injection registration extensions for consumers.
/// </summary>
public static class ConsumerServiceCollectionExtensions
{
    /// <summary>
    /// Adds and configures DandyRabbitMQ consumer <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="builderAction">An action that configures the consumer.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRabbitMQConsumer(this IServiceCollection services, Action<ConsumerConfigurationBuilder> builderAction)
    {
        var builder = new ConsumerConfigurationBuilder();
        builderAction.Invoke(builder);
        var configuration = builder.Build();

        services.AddSingleton(configuration);
        services.AddHostedService<ConsumerWorker>();
        services.AddSingleton<IConsumerPipeline, ConsumerPipeline>();
        services.AddSingleton<IReceiver, Receiver>();

        if (configuration.Assemblies != null)
        {
            services.ScanAndAdd(scanner =>
            {
                scanner.ScanIn(configuration.Assemblies);
                scanner.ScanFor(typeof(IConsumer<>));
                scanner.ScanFor(typeof(IConsumerMiddleware<>), middleware =>
                {
                    middleware.AllowOpenGeneric();
                });
                scanner.ScanFor(typeof(IConsumerExceptionHandler<>), exceptionHandler =>
                {
                    exceptionHandler.AllowOpenGeneric();
                });
            });
        }

        services.AddRabbitMQConnectivity(configuration.ConnectivityConfigurationBuilder.Build());
        services.AddRabbitMQMessages(configuration.MessagesConfigurationBuilder.Build());
        services.AddRabbitMQDeclarations(configuration.DeclarationsConfigurationBuilder.Build());

        if (configuration.SerializerConfiguration != null)
            services.AddSerialization(configuration.SerializerConfiguration);

        if (configuration.EncodingConfiguration != null)
            services.AddEncoder(configuration.EncodingConfiguration);

        return services;
    }
}
