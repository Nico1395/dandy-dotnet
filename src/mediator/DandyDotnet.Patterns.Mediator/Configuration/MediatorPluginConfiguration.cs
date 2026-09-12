using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Mediator.Configuration;

/// <summary>
/// Base class for DandyDotnet.Patterns.Mediator plugins.
/// </summary>
public abstract class MediatorPluginConfiguration
{
    /// <summary>
    /// Configuration slot used by the plugin.
    /// </summary>
    public abstract string Slot { get; }

    public abstract void ConfigureServices(IServiceCollection services);
}
