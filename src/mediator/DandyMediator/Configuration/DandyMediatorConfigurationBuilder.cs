using System.Reflection;

namespace DandyMediator.Configuration;

/// <summary>
/// Builder for <see cref="DandyMediatorConfiguration"/>.
/// </summary>
public sealed class DandyMediatorConfigurationBuilder
{
    private readonly DandyMediatorConfiguration _configuration = new();

    /// <summary>
    /// Sets the assemblies scanned for request and notification handlers.
    /// </summary>
    /// <param name="assemblies">Assemblies to scan.</param>
    /// <returns>The builder.</returns>
    public DandyMediatorConfigurationBuilder ScanInAssemblies(params IEnumerable<Assembly> assemblies)
    {
        _configuration.SetAssemblies(assemblies);
        return this;
    }

    /// <summary>
    /// Registers a mediator plugin.
    /// </summary>
    /// <param name="plugin">Plugin to register.</param>
    /// <returns>The builder.</returns>
    public DandyMediatorConfigurationBuilder UsePlugin(DandyMediatorPlugin plugin)
    {
        _configuration.AddPlugin(plugin);
        return this;
    }

    internal DandyMediatorConfiguration Build()
    {
        return _configuration;
    }
}
