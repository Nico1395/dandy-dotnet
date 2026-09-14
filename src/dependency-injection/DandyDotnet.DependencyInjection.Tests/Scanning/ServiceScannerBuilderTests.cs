using System.Reflection;
using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Scanning;

public sealed class ServiceScannerBuilderTests
{
    [Fact]
    public void Build_UsesConfiguredAssembliesAndDescriptors()
    {
        var assembly = typeof(RequestHandler).Assembly;
        var predicate = (Type type) => type == typeof(RequestHandler);

        var scanner = new ServiceScannerBuilder()
            .ScanIn(assembly)
            .ScanFor<IHandler<Request, string>>(builder => builder.When(predicate).WithLifetime(ServiceLifetime.Singleton))
            .Build();

        Assert.Same(assembly, Assert.Single(scanner.Assemblies));
        var descriptor = Assert.Single(scanner.Descriptors);
        Assert.Equal(typeof(IHandler<Request, string>), descriptor.Key);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Value.Lifetime);
        Assert.Same(predicate, descriptor.Value.Predicate);
    }

    [Fact]
    public void ScanIn_ReplacesPreviouslyConfiguredAssemblies()
    {
        var firstAssembly = typeof(RequestHandler).Assembly;
        var secondAssembly = typeof(ServiceScannerBuilder).Assembly;

        var scanner = new ServiceScannerBuilder()
            .ScanIn(firstAssembly)
            .ScanIn(secondAssembly)
            .Build();

        Assert.Equal([secondAssembly], scanner.Assemblies);
    }

    [Fact]
    public void ScanFor_ReplacesDescriptorWithTheSameAbstractType()
    {
        var scanner = new ServiceScannerBuilder()
            .ScanFor<IHandler<Request, string>>(builder => builder.WithLifetime(ServiceLifetime.Singleton))
            .ScanFor<IHandler<Request, string>>(builder => builder.WithLifetime(ServiceLifetime.Scoped))
            .Build();

        var descriptor = Assert.Single(scanner.Descriptors);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Value.Lifetime);
    }

    [Fact]
    public void Build_WithoutConfiguration_UsesEmptyCollections()
    {
        var scanner = new ServiceScannerBuilder().Build();

        Assert.Empty(scanner.Assemblies);
        Assert.Empty(scanner.Descriptors);
    }
}
