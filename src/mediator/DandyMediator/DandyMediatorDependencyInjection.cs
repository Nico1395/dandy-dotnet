using DandyMediator.Commands;
using DandyMediator.Configuration;
using DandyMediator.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DandyMediator.Responses;

namespace DandyMediator;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection"/> to add DandyMediator to the DI container.
/// </summary>
public static class DandyMediatorDependencyInjection
{
    private static readonly IReadOnlyList<Type> _requestHandlerInterfaceTypes =
    [
        typeof(IRequestHandler<>),
        typeof(IRequestExceptionHandler<>),
        typeof(IRequestMiddleware<>),
        typeof(IRequestHandler<,>),
        typeof(IRequestExceptionHandler<,>),
        typeof(IRequestMiddleware<,>),
        typeof(IQueryHandler<,>),
        typeof(ICommandHandler<>),
        typeof(ICommandHandler<,>),
        typeof(INotificationHandler<>),
        typeof(INotificationExceptionHandler<>),
    ];

    /// <summary>
    /// Adds DandyMediator to the <paramref name="services"/>.
    /// </summary>
    /// <param name="services">The service collection DandyMediator is added to.</param>
    /// <param name="configuration">Configuration action to configure DandyMediator.</param>
    /// <returns>The <paramref name="services"/>.</returns>
    public static IServiceCollection AddDandyMediator(this IServiceCollection services, Action<DandyMediatorConfigurationBuilder>? configuration = null)
    {
        var builder = new DandyMediatorConfigurationBuilder();
        configuration?.Invoke(builder);
        var config = builder.Build();

        services.AddSingleton(config);

        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<IRequestPipeline, RequestPipeline>();

        services.AddSingleton<IRequestResponseFactory, RequestResponseFactory>();
        services.AddSingleton<IRequestResponseMapper, RequestResponseMapper>();

        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(IRequestResponse), typeof(RequestResponse)));
        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(IRequestResponse<>), typeof(RequestResponse<>)));
        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(IQueryResponse<>), typeof(QueryResponse<>)));
        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(ICommandResponse), typeof(CommandResponse)));
        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(ICommandResponse<>), typeof(CommandResponse<>)));

        AddRequestHandlersFromAssemblies(services, config.Assemblies);
        InstallPlugins(services, config);   // Runs through plugins after the base services have been registered, so a plugin could theoretically overwrite base registrations.

        return services;
    }

    private static void AddRequestHandlersFromAssemblies(IServiceCollection services, IReadOnlyList<Assembly> assemblies)
    {
        var handlerTypes = assemblies.SelectMany(a => a.DefinedTypes).Where(t => t is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false });
        foreach (var implementationType in handlerTypes)
        {
            var interfaces = implementationType.ImplementedInterfaces;
            foreach (var @interface in interfaces)
            {
                if (!@interface.IsGenericType)
                    continue;

                var genericDefinition = @interface.GetGenericTypeDefinition();
                if (_requestHandlerInterfaceTypes.Contains(genericDefinition))
                    services.AddTransient(@interface, implementationType);
            }
        }
    }

    private static void InstallPlugins(IServiceCollection services, DandyMediatorConfiguration configuration)
    {
        foreach (var plugin in configuration.Plugins.Values)
        {
            var pluginConfig = plugin.ConfigurationFactory(services, configuration);
            configuration.AddPluginConfiguration(plugin.Slot, pluginConfig);
        }
    }
}
