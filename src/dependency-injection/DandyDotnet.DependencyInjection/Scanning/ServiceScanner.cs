using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

public sealed class ServiceScanner(IReadOnlyDictionary<Type, ScanDescriptor> descriptors, Assembly[] assemblies)
{
    public IReadOnlyDictionary<Type, ScanDescriptor> Descriptors { get; } = descriptors;
    public Assembly[] Assemblies { get; } = assemblies;

    public IEnumerable<ServiceDescriptor> GetServiceDescriptors()
    {
    }
}