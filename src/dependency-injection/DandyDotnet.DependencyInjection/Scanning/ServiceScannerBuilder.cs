using System.Reflection;

namespace DandyDotnet.DependencyInjection.Scanning;

/// <summary>
///     Builds a <see cref="ServiceScanner" /> by configuring service types and assemblies to scan.
/// </summary>
/// <remarks>
///     <para>
///         Each service type added with <see cref="ScanFor(Type, Action{ScanDescriptorBuilder}?)" /> becomes one
///         <see cref="ScanDescriptor" />. Assemblies added with <see cref="ScanIn(IEnumerable{Assembly})" /> provide the
///         implementation types inspected by the scanner.
///     </para>
/// </remarks>
public sealed class ServiceScannerBuilder
{
    private readonly Dictionary<Type, ScanDescriptor> _descriptors = [];
    private Assembly[] _assemblies = [];

    /// <summary>
    ///     Adds a service type to scan for.
    /// </summary>
    /// <param name="abstractType">The abstract service type that implementation types must implement or derive from.</param>
    /// <param name="builderAction">An optional action used to configure the scan descriptor for the service type.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ServiceScannerBuilder ScanFor(Type abstractType, Action<ScanDescriptorBuilder>? builderAction)
    {
        var builder = new ScanDescriptorBuilder(abstractType);
        builderAction?.Invoke(builder);
        var scanDescriptor = builder.Build();

        _descriptors[scanDescriptor.AbstractType] = scanDescriptor;
        return this;
    }

    /// <summary>
    ///     Sets the assemblies that should be scanned for implementation types.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ServiceScannerBuilder ScanIn(params IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies.ToArray();
        return this;
    }

    /// <summary>
    ///     Builds the configured <see cref="ServiceScanner" />.
    /// </summary>
    /// <returns>A scanner containing the configured descriptors and assemblies.</returns>
    public ServiceScanner Build()
    {
        return new ServiceScanner(
            _descriptors,
            _assemblies);
    }
}
