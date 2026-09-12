using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Mediator.Configuration;

/// <summary>
/// Base class for DandyDotnet.Patterns.Mediator plugins.
/// </summary>
public abstract class DandyMediatorPlugin
{
    /// <summary>
    /// Unique plugin key.
    /// </summary>
    public abstract string Key { get; }
    
    /// <summary>
    /// Configuration slot used by the plugin.
    /// </summary>
    public abstract string Slot { get; }
    
    /// <summary>
    /// Creates the plugin configuration and registers services.
    /// </summary>
    public required Func<IServiceCollection, DandyMediatorConfiguration, DandyMediatorPluginConfiguration> ConfigurationFactory { get; init; }
}
