using DandyDotnet.DependencyInjection.Scanning;
using DandyDotnet.DependencyInjection.Tests.Scanning.Mocks.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Scanning;

public sealed class ServiceScannerBuilderExtensionsTests
{
    [Fact]
    public void ScanFor_Type_AddsDescriptorAndReturnsBuilder()
    {
        var builder = new ServiceScannerBuilder();

        var result = builder.ScanFor(typeof(IHandler<Request, string>));

        Assert.Same(builder, result);
        var descriptor = Assert.Single(builder.Build().Descriptors).Value;
        Assert.Equal(typeof(IHandler<Request, string>), descriptor.AbstractType);
        Assert.Equal(ServiceLifetime.Transient, descriptor.Lifetime);
    }

    [Fact]
    public void ScanFor_Types_AddsAllDescriptorsAndReturnsBuilder()
    {
        var builder = new ServiceScannerBuilder();

        var result = builder.ScanFor(
            new[] { typeof(IHandler<Request, string>), typeof(IHandlerMiddleware<Request, string>) });

        Assert.Same(builder, result);
        Assert.Equal(
            [typeof(IHandler<Request, string>), typeof(IHandlerMiddleware<Request, string>)],
            builder.Build().Descriptors.Keys.OrderBy(type => type.FullName));
    }

    [Fact]
    public void ScanFor_GenericWithBuilderAction_ForwardsActionAndReturnsBuilder()
    {
        var builder = new ServiceScannerBuilder();
        var predicate = (Type type) => type == typeof(RequestHandler);

        var result = builder.ScanFor<IHandler<Request, string>>(
            descriptorBuilder => descriptorBuilder
                .When(predicate)
                .WithLifetime(ServiceLifetime.Singleton));

        Assert.Same(builder, result);
        var descriptor = Assert.Single(builder.Build().Descriptors).Value;
        Assert.Same(predicate, descriptor.Predicate);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }

    [Fact]
    public void ScanFor_GenericWithoutBuilderAction_AddsDescriptorAndReturnsBuilder()
    {
        var builder = new ServiceScannerBuilder();

        var result = builder.ScanFor<IHandler<Request, string>>();

        Assert.Same(builder, result);
        Assert.Contains(typeof(IHandler<Request, string>), builder.Build().Descriptors.Keys);
    }
}
