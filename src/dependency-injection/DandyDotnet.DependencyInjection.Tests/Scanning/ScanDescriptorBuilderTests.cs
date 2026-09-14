using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Scanning;

public sealed class ScanDescriptorBuilderTests
{
    [Fact]
    public void Build_UsesDefaults()
    {
        var scanner = new ServiceScannerBuilder()
            .ScanFor<IHandler<Request, string>>(_ => { })
            .Build();

        var descriptor = Assert.Single(scanner.Descriptors).Value;

        Assert.Equal(typeof(IHandler<Request, string>), descriptor.AbstractType);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
        Assert.Null(descriptor.Predicate);
        Assert.Null(descriptor.ServiceKey);
        Assert.Null(descriptor.KeyedFactory);
        Assert.Null(descriptor.Factory);
        Assert.False(descriptor.IsOpenGeneric);
    }

    [Fact]
    public void Build_UsesAllConfiguredValues()
    {
        var predicate = (Type type) => type == typeof(RequestHandler);
        var factory = (IServiceProvider provider) => new RequestHandler();
        var keyedFactory = (IServiceProvider provider, object? key) => new RequestHandler();

        var scanner = new ServiceScannerBuilder()
            .ScanFor<IHandler<Request, string>>(builder => builder
                .When(predicate)
                .WithLifetime(ServiceLifetime.Singleton)
                .WithKey("handler")
                .WithFactory(factory)
                .WithKeyedFactory(keyedFactory)
                .AsOpenGeneric())
            .Build();

        var descriptor = Assert.Single(scanner.Descriptors).Value;

        Assert.Same(predicate, descriptor.Predicate);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
        Assert.Equal("handler", descriptor.ServiceKey);
        Assert.Same(factory, descriptor.Factory);
        Assert.Same(keyedFactory, descriptor.KeyedFactory);
        Assert.True(descriptor.IsOpenGeneric);
    }

    [Fact]
    public void ConfigurationMethods_ReturnTheSameBuilder()
    {
        var builder = new ServiceScannerBuilder();
        ScanDescriptorBuilder? descriptorBuilder = null;

        builder.ScanFor<IHandler<Request, string>>(configured =>
        {
            descriptorBuilder = configured;
            Assert.Same(configured, configured.When(_ => true));
            Assert.Same(configured, configured.WithLifetime(ServiceLifetime.Scoped));
            Assert.Same(configured, configured.WithKey("key"));
            Assert.Same(configured, configured.WithFactory(_ => new RequestHandler()));
            Assert.Same(configured, configured.WithKeyedFactory((_, _) => new RequestHandler()));
            Assert.Same(configured, configured.AsOpenGeneric());
        });

        Assert.NotNull(descriptorBuilder);
    }
}
