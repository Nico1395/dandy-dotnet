using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

/// <summary>
///     Extension methods for setting common service lifetimes on a <see cref="ScanDescriptorBuilder" />.
/// </summary>
public static class ScanDescriptorBuilderExtensions
{
    /// <summary>
    ///     Configures generated service descriptors to use <see cref="ServiceLifetime.Transient" />.
    /// </summary>
    /// <param name="builder">The scan descriptor builder to configure.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ScanDescriptorBuilder AsTransient(this ScanDescriptorBuilder builder)
    {
        return builder.WithLifetime(ServiceLifetime.Transient);
    }
    
    /// <summary>
    ///     Configures generated service descriptors to use <see cref="ServiceLifetime.Scoped" />.
    /// </summary>
    /// <param name="builder">The scan descriptor builder to configure.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ScanDescriptorBuilder AsScoped(this ScanDescriptorBuilder builder)
    {
        return builder.WithLifetime(ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Configures generated service descriptors to use <see cref="ServiceLifetime.Singleton" />.
    /// </summary>
    /// <param name="builder">The scan descriptor builder to configure.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ScanDescriptorBuilder AsSingleton(this ScanDescriptorBuilder builder)
    {
        return builder.WithLifetime(ServiceLifetime.Singleton);
    }
}
