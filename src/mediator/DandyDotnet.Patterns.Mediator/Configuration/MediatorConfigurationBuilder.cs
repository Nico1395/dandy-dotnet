using System.Reflection;
using DandyDotnet.DependencyInjection.Scanning;

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

    /// <summary>
    /// Adds a service type to scan for via the <see cref="ServiceScanner"/> in <see cref="ServiceCollectionExtensions.AddMediator"/>.
    /// </summary>
    /// <param name="serviceType">The type of service to scan for.</param>
    /// <returns>The builder.</returns>
    /// <remarks>
    ///     <para>
    ///         Duplicate service types are ignored.
    ///     </para>
    /// </remarks>
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
