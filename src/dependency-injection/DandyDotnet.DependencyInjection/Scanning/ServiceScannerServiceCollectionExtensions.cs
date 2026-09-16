using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DandyDotnet.DependencyInjection.Scanning;

/// <summary>
///     Extension methods for scanning assemblies and adding discovered services to an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceScannerServiceCollectionExtensions
{
    /// <summary>
    ///     Configures a service scanner and adds the discovered service descriptors to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add discovered descriptors to.</param>
    /// <param name="scannerAction">An action used to configure the service scanner.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection ScanAndAdd(this IServiceCollection services, Action<ServiceScannerBuilder> scannerAction)
    {
        var builder = new ServiceScannerBuilder();
        scannerAction(builder);
        var scanner = builder.Build();

        return services.ScanAndAdd(scanner);
    }

    /// <summary>
    ///     Adds the service descriptors produced by the specified scanner to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add discovered descriptors to.</param>
    /// <param name="scanner">The configured service scanner.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection ScanAndAdd(this IServiceCollection services, ServiceScanner scanner)
    {
        var serviceDescriptors = scanner.GetServiceDescriptors();
        return services.Add(serviceDescriptors);
    }
}
