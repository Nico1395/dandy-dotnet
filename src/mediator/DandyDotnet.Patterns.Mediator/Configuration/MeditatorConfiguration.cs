using System.Reflection;
using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.Patterns.Mediator.Abstractions.Notifications;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;

namespace DandyDotnet.Patterns.Mediator.Configuration;

/// <summary>
/// Configuration used to set up DandyDotnet.Patterns.Mediator.
/// </summary>
public sealed class MeditatorConfiguration
{
    private readonly Dictionary<string, MediatorPluginConfiguration> _plugins = [];
    
    private List<Assembly> _assemblies = [];
    private readonly List<Type> _serviceTypes = 
    [
        typeof(IRequestHandler<>),
        typeof(IRequestExceptionHandler<>),
        typeof(IRequestMiddleware<>),
        typeof(IRequestHandler<,>),
        typeof(IRequestExceptionHandler<,>),
        typeof(IRequestMiddleware<,>),
        typeof(INotificationHandler<>),
        typeof(INotificationExceptionHandler<>),
    ];

    /// <summary>
    /// Plugins registered with the mediator.
    /// </summary>
    public IReadOnlyDictionary<string, MediatorPluginConfiguration> Plugins => _plugins;

    /// <summary>
    /// Assemblies scanned for request and notification handlers.
    /// </summary>
    public IReadOnlyList<Assembly> Assemblies => _assemblies;

    /// <summary>
    /// Types of services that are registered via the <see cref="ServiceScanner"/> in <see cref="ServiceCollectionExtensions.AddMediator"/>.
    /// </summary>
    public IReadOnlyList<Type> ServiceTypes => _serviceTypes;

    internal void AddPlugin(MediatorPluginConfiguration pluginConfiguration)
    {
        _plugins[pluginConfiguration.Slot] = pluginConfiguration;
    }

    internal void SetAssemblies(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies.ToList();
    }

    internal void AddServiceType(Type serviceType)
    {
        _serviceTypes.Add(serviceType);
    }
}
