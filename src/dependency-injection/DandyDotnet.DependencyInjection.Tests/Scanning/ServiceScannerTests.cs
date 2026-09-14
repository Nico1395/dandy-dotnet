using System.Reflection;
using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.AccessModifiers;
using DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.OpenGeneric;
using DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Scanning;

public sealed class ServiceScannerTests
{
    private static readonly Assembly TestAssembly = typeof(RequestHandler).Assembly;
    private static readonly Func<Type, bool> PipelineTypes = type =>
        type.Namespace == typeof(RequestHandler).Namespace;

    [Fact]
    public void GetServiceDescriptors_FindsAssignableConcreteImplementations()
    {
        var descriptors = CreateScanner<IHandler<Request, string>>().GetServiceDescriptors().ToArray();

        var descriptor = Assert.Single(descriptors);
        Assert.Equal(typeof(IHandler<Request, string>), descriptor.ServiceType);
        Assert.Equal(typeof(RequestHandler), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }

    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void GetServiceDescriptors_PreservesLifetime(ServiceLifetime lifetime)
    {
        var descriptors = CreateScanner<IHandler<Request, string>>(builder => builder.WithLifetime(lifetime))
            .GetServiceDescriptors()
            .ToArray();

        Assert.Equal(lifetime, Assert.Single(descriptors).Lifetime);
    }

    [Fact]
    public void GetServiceDescriptors_AppliesPredicate()
    {
        var descriptors = CreateScanner<IHandler<Request, string>>(
                builder => builder.When(type => type == typeof(RequestExceptionHandler)))
            .GetServiceDescriptors();

        Assert.Empty(descriptors);
    }

    [Fact]
    public void GetServiceDescriptors_CreatesNonKeyedFactoryDescriptor()
    {
        Func<IServiceProvider, object> factory = _ => new RequestHandler();
        var descriptor = Assert.Single(CreateScanner<IHandler<Request, string>>(
                builder => builder.WithFactory(factory))
            .GetServiceDescriptors());

        Assert.False(descriptor.IsKeyedService);
        Assert.Null(descriptor.ImplementationType);
        Assert.Same(factory, descriptor.ImplementationFactory);
    }

    [Fact]
    public void GetServiceDescriptors_CreatesKeyedImplementationDescriptor()
    {
        var descriptor = Assert.Single(CreateScanner<IHandler<Request, string>>(
                builder => builder.WithKey("handler"))
            .GetServiceDescriptors());

        Assert.True(descriptor.IsKeyedService);
        Assert.Equal("handler", descriptor.ServiceKey);
        Assert.Equal(typeof(RequestHandler), descriptor.KeyedImplementationType);
        Assert.Null(descriptor.KeyedImplementationFactory);
    }

    [Fact]
    public void GetServiceDescriptors_CreatesKeyedFactoryDescriptor()
    {
        Func<IServiceProvider, object?, object> factory = (_, key) => new RequestHandler();
        var descriptor = Assert.Single(CreateScanner<IHandler<Request, string>>(
                builder => builder.WithKey("handler").WithKeyedFactory(factory))
            .GetServiceDescriptors());

        Assert.True(descriptor.IsKeyedService);
        Assert.Equal("handler", descriptor.ServiceKey);
        Assert.Same(factory, descriptor.KeyedImplementationFactory);
    }

    [Fact]
    public void GetServiceDescriptors_CreatesKeyedDescriptorFromNonKeyedFactory()
    {
        Func<IServiceProvider, object> factory = _ => new RequestHandler();
        var descriptor = Assert.Single(CreateScanner<IHandler<Request, string>>(
                builder => builder.WithKey("handler").WithFactory(factory))
            .GetServiceDescriptors());

        Assert.True(descriptor.IsKeyedService);
        Assert.Equal("handler", descriptor.ServiceKey);
        Assert.NotNull(descriptor.KeyedImplementationFactory);
        Assert.Equal(typeof(RequestHandler), descriptor.KeyedImplementationFactory!(new ServiceCollection().BuildServiceProvider(), "ignored").GetType());
    }

    [Fact]
    public void GetServiceDescriptors_KeyedFactoryTakesPrecedenceOverFactory()
    {
        Func<IServiceProvider, object> factory = _ => throw new InvalidOperationException();
        Func<IServiceProvider, object?, object> keyedFactory = (_, _) => new RequestHandler();

        var descriptor = Assert.Single(CreateScanner<IHandler<Request, string>>(
                builder => builder
                    .WithKey("handler")
                    .WithFactory(factory)
                    .WithKeyedFactory(keyedFactory))
            .GetServiceDescriptors());

        Assert.Same(keyedFactory, descriptor.KeyedImplementationFactory);
    }

    [Fact]
    public void GetServiceDescriptors_FindsClosedGenericServiceType()
    {
        var descriptors = CreateScanner(typeof(IHandler<,>)).GetServiceDescriptors().ToArray();

        var descriptor = Assert.Single(descriptors);
        Assert.Equal(typeof(IHandler<,>).MakeGenericType(typeof(Request), typeof(string)), descriptor.ServiceType);
        Assert.Equal(typeof(RequestHandler), descriptor.ImplementationType);
    }

    [Fact]
    public void GetServiceDescriptors_FindsOpenGenericImplementation()
    {
        var descriptor = Assert.Single(CreateScanner(typeof(IOpenGenericHandler<>), builder => builder.AsOpenGeneric())
            .GetServiceDescriptors());

        Assert.Equal(typeof(IOpenGenericHandler<>), descriptor.ServiceType);
        Assert.Equal(typeof(OpenGenericHandler<>), descriptor.ImplementationType);
    }

    [Fact]
    public void GetServiceDescriptors_OpenGenericScanIgnoresClosedImplementations()
    {
        var descriptors = CreateScanner(typeof(IOpenGenericHandler<>), builder => builder
                .AsOpenGeneric()
                .When(type => type == typeof(ClosedGenericHandler)))
            .GetServiceDescriptors();

        Assert.Empty(descriptors);
    }

    [Fact]
    public void GetServiceDescriptors_ClosedScanFindsClosedImplementationsOnly()
    {
        var descriptors = CreateScanner<IOpenGenericHandler<Request>>()
            .GetServiceDescriptors();

        var descriptor = Assert.Single(descriptors);
        Assert.Equal(typeof(ClosedGenericHandler), descriptor.ImplementationType);
    }

    [Fact]
    public void GetServiceDescriptors_IncludesInternalClassesButExcludesStaticClasses()
    {
        var descriptors = new ServiceScannerBuilder()
            .ScanIn(TestAssembly)
            .ScanFor<IAccessModifierInterface>(builder => builder.When(type =>
                type.Namespace == typeof(PublicClass).Namespace))
            .Build()
            .GetServiceDescriptors()
            .ToArray();

        Assert.Equal(
            [typeof(InternalClass), typeof(StaticClass).GetNestedType("PrivateClass", BindingFlags.NonPublic)!,
                typeof(PublicClass)],
            descriptors.Select(descriptor => descriptor.ImplementationType).OrderBy(type => type?.Name));
    }

    [Fact]
    public void GetServiceDescriptors_ExcludesNonMatchingTypes()
    {
        var descriptors = CreateScanner<IDisposable>().GetServiceDescriptors();

        Assert.Empty(descriptors);
    }

    [Fact]
    public void GetServiceDescriptors_ExcludesTypesFromOtherAssemblies()
    {
        var descriptors = new ServiceScannerBuilder()
            .ScanIn(typeof(ServiceScanner).Assembly)
            .ScanFor<IHandler<Request, string>>(builder => builder.When(PipelineTypes))
            .Build()
            .GetServiceDescriptors();

        Assert.Empty(descriptors);
    }

    [Fact]
    public void GetServiceDescriptors_SupportsMultipleScanDescriptors()
    {
        var descriptors = new ServiceScannerBuilder()
            .ScanIn(TestAssembly)
            .ScanFor<IHandler<Request, string>>(builder => builder.When(PipelineTypes))
            .ScanFor<IHandlerMiddleware<Request, string>>(builder => builder.When(PipelineTypes))
            .Build()
            .GetServiceDescriptors()
            .ToArray();

        Assert.Equal(
            [typeof(IHandler<Request, string>), typeof(IHandlerMiddleware<Request, string>)],
            descriptors.Select(descriptor => descriptor.ServiceType).OrderBy(type => type.FullName));
    }

    private static ServiceScanner CreateScanner<T>(Action<ScanDescriptorBuilder>? configure = null)
    {
        return new ServiceScannerBuilder()
            .ScanIn(TestAssembly)
            .ScanFor<T>(builder =>
            {
                builder.When(PipelineTypes);
                configure?.Invoke(builder);
            })
            .Build();
    }

    private static ServiceScanner CreateScanner(Type abstractType, Action<ScanDescriptorBuilder>? configure = null)
    {
        return new ServiceScannerBuilder()
            .ScanIn(TestAssembly)
            .ScanFor(abstractType, builder =>
            {
                builder.When(PipelineTypes);
                configure?.Invoke(builder);
            })
            .Build();
    }
}
