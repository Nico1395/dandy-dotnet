using System.Reflection;

namespace DandyDotnet.Patterns.Mediator.Configuration;

/// <summary>
/// Builder for <see cref="MeditatorConfiguration"/>.
/// </summary>
public sealed class MediatorConfigurationBuilder
{
    private readonly MeditatorConfiguration _configuration = new();

    /// <summary>
    /// Sets the assemblies scanned for request and notification handlers.
    /// </summary>
    /// <param name="assemblies">Assemblies to scan.</param>
    /// <returns>The builder.</returns>
    public MediatorConfigurationBuilder ScanInAssemblies(params IEnumerable<Assembly> assemblies)
    {
        _configuration.SetAssemblies(assemblies);
        return this;
    }

    public MediatorConfigurationBuilder ScanForServiceType(Type serviceType)
    {
        _configuration.AddServiceType(serviceType);
        return this;
    }

    /// <summary>
    /// Registers a mediator pluginConfiguration.
    /// </summary>
    /// <param name="pluginConfiguration">Plugin to register.</param>
    /// <returns>The builder.</returns>
    public MediatorConfigurationBuilder UsePlugin(MediatorPluginConfiguration pluginConfiguration)
    {
        _configuration.AddPlugin(pluginConfiguration);
        return this;
    }

    internal MeditatorConfiguration Build()
    {
        return _configuration;
    }
}
