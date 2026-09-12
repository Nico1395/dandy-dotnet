using System.Reflection;

namespace DandyMediator.Configuration;

/// <summary>
/// Configuration used to set up DandyMediator.
/// </summary>
public sealed class DandyMediatorConfiguration
{
    private readonly Dictionary<string, DandyMediatorPlugin> _plugins = [];
    private readonly Dictionary<string, DandyMediatorPluginConfiguration> _pluginConfigurations = [];
    private List<Assembly> _assemblies = [];

    /// <summary>
    /// Plugins registered with the mediator.
    /// </summary>
    public IReadOnlyDictionary<string, DandyMediatorPlugin> Plugins => _plugins;

    /// <summary>
    /// Assemblies scanned for request and notification handlers.
    /// </summary>
    public IReadOnlyList<Assembly> Assemblies => _assemblies;

    internal void AddPlugin(DandyMediatorPlugin plugin)
    {
        _plugins[plugin.Slot] = plugin;
    }

    internal void AddPluginConfiguration(string slot, DandyMediatorPluginConfiguration pluginConfiguration)
    {
        _pluginConfigurations[slot] = pluginConfiguration;
    }

    internal void SetAssemblies(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies.ToList();
    }
}
