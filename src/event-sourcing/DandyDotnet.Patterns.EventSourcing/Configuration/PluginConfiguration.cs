using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Configuration;

/// <summary>
///     Represents the base configuration contract for Event Sourcing plugins.
/// </summary>
/// <remarks>
///     <para>
///         Plugins extend the Event Sourcing framework by registering specific persistence providers,
///         serialization engines, or outbox dispatch mechanisms.
///     </para>
///     <para>
///         Each plugin occupies a designated <see cref="Slot" /> to prevent conflicting configurations
///         within the same functional area.
///     </para>
/// </remarks>
public abstract class PluginConfiguration
{
    /// <summary>
    ///     Gets the unique slot identifier for this plugin category (e.g., persistence).
    /// </summary>
    public abstract string Slot { get; }

    /// <summary>
    ///     Registers plugin-specific services into the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to register services into.</param>
    public abstract void ConfigureServices(IServiceCollection services);
}