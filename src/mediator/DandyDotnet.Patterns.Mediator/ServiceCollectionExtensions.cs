using DandyDotnet.DependencyInjection.Scanning;
using Microsoft.Extensions.DependencyInjection;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests.Factories;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests.Mapping;
using DandyDotnet.Patterns.Mediator.Configuration;
using DandyDotnet.Patterns.Mediator.Requests;
using DandyDotnet.Patterns.Mediator.Requests.Factories;
using DandyDotnet.Patterns.Mediator.Requests.Mapping;

namespace DandyDotnet.Patterns.Mediator;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection"/> to add DandyDotnet.Patterns.Mediator to the DI container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds DandyDotnet.Patterns.Mediator to the <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The service collection DandyDotnet.Patterns.Mediator is added to.</param>
    /// <param name="action">Configuration action to configure DandyDotnet.Patterns.Mediator.</param>
    /// <returns>The <paramref name="services"/>.</returns>
    public static IServiceCollection AddMediator(this IServiceCollection services, Action<MediatorConfigurationBuilder>? action = null)
    {
        var builder = new MediatorConfigurationBuilder();
        action?.Invoke(builder);
        var configuration = builder.Build();

        services.AddSingleton(configuration);

        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<IRequestPipeline, RequestPipeline>();

        services.AddSingleton<IRequestResponseFactory, RequestResponseFactory>();
        services.AddSingleton<IRequestResponseMapper, RequestResponseMapper>();

        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(IRequestResponse), typeof(RequestResponse)));
        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(IRequestResponse<>), typeof(RequestResponse<>)));

        var scanner = new ServiceScannerBuilder()
            .ScanIn(configuration.Assemblies)
            .ScanFor(configuration.ServiceTypes)
            .Build();
        services.ScanAndAdd(scanner);

        AddPlugins(services, configuration);   // Runs through plugins after the base services have been registered, so a plugin could theoretically overwrite base registrations.

        return services;
    }

    private static void AddServiceTypes(IServiceCollection services, MeditatorConfiguration configuration)
    {
        var handlerTypes = configuration.Assemblies.SelectMany(a => a.DefinedTypes).Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false });
        foreach (var implementationType in handlerTypes)
        {
            var interfaces = implementationType.ImplementedInterfaces;
            foreach (var @interface in interfaces)
            {
                if (!@interface.IsGenericType)
                    continue;

                var genericDefinition = @interface.GetGenericTypeDefinition();
                if (configuration.ServiceTypes.Contains(genericDefinition))
                    services.AddTransient(@interface, implementationType);
            }
        }
    }

    private static void AddPlugins(IServiceCollection services, MeditatorConfiguration configuration)
    {
        foreach (var plugin in configuration.Plugins.Values)
            plugin.ConfigureServices(services);
    }
}
