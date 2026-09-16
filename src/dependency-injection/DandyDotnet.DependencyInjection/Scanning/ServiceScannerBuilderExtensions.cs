namespace DandyDotnet.DependencyInjection.Scanning;

/// <summary>
///     Extension methods for configuring service types on a <see cref="ServiceScannerBuilder" />.
/// </summary>
public static class ServiceScannerBuilderExtensions
{
    /// <summary>
    ///     Adds a service type to scan for using the default scan descriptor configuration.
    /// </summary>
    /// <param name="builder">The service scanner builder to configure.</param>
    /// <param name="abstractType">The abstract service type that implementation types must implement or derive from.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ServiceScannerBuilder ScanFor(this ServiceScannerBuilder builder, Type abstractType)
    {
        return builder.ScanFor(abstractType, builderAction: null);
    }

    /// <summary>
    ///     Adds service types to scan for using the default scan descriptor configuration.
    /// </summary>
    /// <param name="builder">The service scanner builder to configure.</param>
    /// <param name="abstractTypes">The abstract service types that implementation types must implement or derive from.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ServiceScannerBuilder ScanFor(this ServiceScannerBuilder builder, params IEnumerable<Type> abstractTypes)
    {
        foreach (var abstractType in abstractTypes)
            builder.ScanFor(abstractType);
        
        return builder;
    }

    /// <summary>
    ///     Adds <typeparamref name="T" /> as a service type to scan for.
    /// </summary>
    /// <typeparam name="T">The abstract service type that implementation types must implement or derive from.</typeparam>
    /// <param name="builder">The service scanner builder to configure.</param>
    /// <param name="builderAction">An optional action used to configure the scan descriptor for the service type.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ServiceScannerBuilder ScanFor<T>(this ServiceScannerBuilder builder, Action<ScanDescriptorBuilder>? builderAction)
    {
        return builder.ScanFor(typeof(T), builderAction);
    }
    
    /// <summary>
    ///     Adds <typeparamref name="T" /> as a service type to scan for using the default scan descriptor configuration.
    /// </summary>
    /// <typeparam name="T">The abstract service type that implementation types must implement or derive from.</typeparam>
    /// <param name="builder">The service scanner builder to configure.</param>
    /// <returns>The same <paramref name="builder" /> instance so that additional calls can be chained.</returns>
    public static ServiceScannerBuilder ScanFor<T>(this ServiceScannerBuilder builder)
    {
        return builder.ScanFor(typeof(T), builderAction: null);
    }
}
