using System.Reflection;

namespace DandyDotnet.DependencyInjection.Scanning;

public sealed class ServiceScannerBuilder
{
    private readonly Dictionary<Type, ScanDescriptor> _descriptors = [];
    private Assembly[] _assemblies = [];

    public ServiceScannerBuilder ScanFor(Type abstractType, Action<ScanDescriptorBuilder>? builderAction)
    {
        var builder = new ScanDescriptorBuilder(abstractType);
        builderAction?.Invoke(builder);
        var scanDescriptor = builder.Build();

        _descriptors[scanDescriptor.AbstractType] = scanDescriptor;
        return this;
    }

    public ServiceScannerBuilder ScanIn(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
        return this;
    }

    public ServiceScanner Build()
    {
        return new ServiceScanner(
            _descriptors,
            _assemblies);
    }
}