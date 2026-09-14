using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.OpenGeneric;
using DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Scanning;

public sealed class ServiceScannerServiceCollectionExtensionsTests
{
    [Fact]
    public void ScanAndAdd_WithBuilderAction_RegistersImplementationForResolution()
    {
        var services = new ServiceCollection();
        services.ScanAndAdd(builder => builder
            .ScanIn(typeof(RequestHandler).Assembly)
            .ScanFor<IHandler<Request, string>>(descriptor =>
                descriptor.When(type => type == typeof(RequestHandler))));

        using var provider = services.BuildServiceProvider();
        var handler = provider.GetRequiredService<IHandler<Request, string>>();

        Assert.IsType<RequestHandler>(handler);
    }

    [Fact]
    public void ScanAndAdd_WithScanner_RegistersOpenGenericImplementationForResolution()
    {
        var scanner = new ServiceScannerBuilder()
            .ScanIn(typeof(OpenGenericHandler<>).Assembly)
            .ScanFor(typeof(IOpenGenericHandler<>), descriptor => descriptor
                .When(type => type.IsGenericTypeDefinition)
                .AllowOpenGeneric())
            .Build();
        var services = new ServiceCollection().ScanAndAdd(scanner);

        using var provider = services.BuildServiceProvider();
        var handler = provider.GetRequiredService<IOpenGenericHandler<Request>>();

        Assert.IsType<OpenGenericHandler<Request>>(handler);
    }

    [Fact]
    public void ScanAndAdd_RegistersKeyedImplementationForKeyedResolution()
    {
        var services = new ServiceCollection();
        services.ScanAndAdd(builder => builder
            .ScanIn(typeof(RequestHandler).Assembly)
            .ScanFor<IHandler<Request, string>>(descriptor => descriptor
                .When(type => type == typeof(RequestHandler))
                .WithKey("handler")));

        using var provider = services.BuildServiceProvider();
        var handler = provider.GetRequiredKeyedService<IHandler<Request, string>>("handler");

        Assert.IsType<RequestHandler>(handler);
    }

    [Fact]
    public void ScanAndAdd_KeyedFactoryReceivesConfiguredKey()
    {
        object? receivedKey = null;
        var services = new ServiceCollection();
        services.ScanAndAdd(builder => builder
            .ScanIn(typeof(RequestHandler).Assembly)
            .ScanFor<IHandler<Request, string>>(descriptor => descriptor
                .When(type => type == typeof(RequestHandler))
                .WithKey("handler")
                .WithKeyedFactory((_, key) =>
                {
                    receivedKey = key;
                    return new RequestHandler();
                })));

        using var provider = services.BuildServiceProvider();
        var handler = provider.GetRequiredKeyedService<IHandler<Request, string>>("handler");

        Assert.IsType<RequestHandler>(handler);
        Assert.Equal("handler", receivedKey);
    }

    [Fact]
    public void ScanAndAdd_NonKeyedFactoryIsUsedWhenResolvingService()
    {
        var factoryCalls = 0;
        var services = new ServiceCollection();
        services.ScanAndAdd(builder => builder
            .ScanIn(typeof(RequestHandler).Assembly)
            .ScanFor<IHandler<Request, string>>(descriptor => descriptor
                .When(type => type == typeof(RequestHandler))
                .WithFactory(_ =>
                {
                    factoryCalls++;
                    return new RequestHandler();
                })));

        using var provider = services.BuildServiceProvider();
        var first = provider.GetRequiredService<IHandler<Request, string>>();
        var second = provider.GetRequiredService<IHandler<Request, string>>();

        Assert.IsType<RequestHandler>(first);
        Assert.IsType<RequestHandler>(second);
        Assert.Equal(2, factoryCalls);
    }

    [Fact]
    public void ScanAndAdd_FindsConcreteTypesThatInheritTheConfiguredService()
    {
        var descriptors = new ServiceScannerBuilder()
            .ScanIn(typeof(RequestHandler).Assembly)
            .ScanFor<IHandler<Request, string>>(descriptor =>
                descriptor.When(type => type == typeof(RequestHandler) || type == typeof(InheritedRequestHandler)))
            .Build()
            .GetServiceDescriptors()
            .ToArray();

        Assert.Contains(descriptors, descriptor =>
            descriptor.ImplementationType == typeof(InheritedRequestHandler));
    }

    private sealed class InheritedRequestHandler : RequestHandler;
}
