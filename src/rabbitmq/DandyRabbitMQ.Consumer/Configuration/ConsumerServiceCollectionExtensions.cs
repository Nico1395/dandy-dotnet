using System.Reflection;
using DandyRabbitMQ.Consumer.Worker;
using DandyRabbitMQ.Core.Connectivity.Configuration;
using DandyRabbitMQ.Core.Declarations.Configuration;
using DandyRabbitMQ.Core.Encoding.Configuration;
using DandyRabbitMQ.Core.Messages.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DandyRabbitMQ.Consumer.Configuration;

/// <summary>
/// Provides dependency injection registration extensions for consumers.
/// </summary>
public static class ConsumerServiceCollectionExtensions
{
    private static readonly IReadOnlyList<Type> _typesToRegister =
    [
        typeof(IConsumer<>),
        typeof(IConsumerMiddleware<>),
        typeof(IConsumerExceptionHandler<>),
    ];

    /// <summary>
    /// Adds and configures DandyRabbitMQ consumer <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The service collection to update.</param>
    /// <param name="builderAction">An action that configures the consumer.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddDandyRabbitMQConsumer(this IServiceCollection services, Action<ConsumerConfigurationBuilder> builderAction)
    {
        var builder = new ConsumerConfigurationBuilder();
        builderAction.Invoke(builder);
        var configuration = builder.Build();

        services.AddSingleton(configuration);
        services.AddHostedService<ConsumerWorker>();
        services.AddSingleton<IConsumerPipeline, ConsumerPipeline>();
        services.AddSingleton<IReceiver, Receiver>();

        if (configuration.Assemblies != null)
            AddServicesFromAssemblies(services, configuration.Assemblies);

        services.AddDandyRabbitMQConnectivity(configuration.ConnectivityConfigurationBuilder.Build());
        services.AddDandyRabbitMQMessages(configuration.MessagesConfigurationBuilder.Build());
        services.AddDandyRabbitMQEncoding(configuration.EncodingConfigurationBuilder.Build());
        services.AddDandyRabbitMQDeclarations(configuration.DeclarationsConfigurationBuilder.Build());

        return services;
    }

    private static void AddServicesFromAssemblies(IServiceCollection services, IReadOnlyList<Assembly> assemblies)
    {
        var implementationTypes = assemblies.SelectMany(a => a.DefinedTypes).Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false });
        foreach (var implementationType in implementationTypes)
        {
            var interfaces = implementationType.ImplementedInterfaces;
            foreach (var @interface in interfaces)
            {
                if (!@interface.IsGenericType)
                    continue;

                var genericDefinition = @interface.GetGenericTypeDefinition();
                if (_typesToRegister.Contains(genericDefinition))
                    services.AddTransient(@interface, implementationType);
            }
        }
    }
}
