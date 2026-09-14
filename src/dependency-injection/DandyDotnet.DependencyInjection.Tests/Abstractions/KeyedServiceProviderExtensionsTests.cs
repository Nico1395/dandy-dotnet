using DandyDotnet.DependencyInjection.Abstractions;
using DandyDotnet.DependencyInjection.Tests.Abstractions.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Abstractions;

public sealed class KeyedServiceProviderExtensionsTests
{
    [Fact]
    public void GetKeyedOrDefaultService_Type_ResolvesUnkeyedServiceWhenKeyIsNull()
    {
        using var provider = CreateProvider();

        var service = provider.GetKeyedOrDefaultService(typeof(IRangedService), null);

        Assert.IsType<FirstRangedService>(service);
    }

    [Fact]
    public void GetKeyedOrDefaultService_Type_ResolvesKeyedService()
    {
        using var provider = CreateProvider();

        var service = provider.GetKeyedOrDefaultService(typeof(IRangedService), "key");

        Assert.IsType<SecondRangedService>(service);
    }

    [Fact]
    public void GetKeyedOrDefaultService_Generic_ResolvesUnkeyedServiceWhenKeyIsNull()
    {
        using var provider = CreateProvider();

        var service = provider.GetKeyedOrDefaultService<IRangedService>(null);

        Assert.IsType<FirstRangedService>(service);
    }

    [Fact]
    public void GetKeyedOrDefaultService_Generic_ResolvesKeyedService()
    {
        using var provider = CreateProvider();

        var service = provider.GetKeyedOrDefaultService<IRangedService>("key");

        Assert.IsType<SecondRangedService>(service);
    }

    [Fact]
    public void GetKeyedOrDefaultService_ReturnsNullWhenServiceIsMissing()
    {
        using var provider = new ServiceCollection().BuildServiceProvider();

        Assert.Null(provider.GetKeyedOrDefaultService(typeof(IRangedService), null));
        Assert.Null(provider.GetKeyedOrDefaultService<IRangedService>("key"));
    }

    [Fact]
    public void GetRequiredKeyedOrDefaultService_Type_ResolvesUnkeyedServiceWhenKeyIsNull()
    {
        using var provider = CreateProvider();

        var service = provider.GetRequiredKeyedOrDefaultService(typeof(IRangedService), null);

        Assert.IsType<FirstRangedService>(service);
    }

    [Fact]
    public void GetRequiredKeyedOrDefaultService_Type_ResolvesKeyedService()
    {
        using var provider = CreateProvider();

        var service = provider.GetRequiredKeyedOrDefaultService(typeof(IRangedService), "key");

        Assert.IsType<SecondRangedService>(service);
    }

    [Fact]
    public void GetRequiredKeyedOrDefaultService_Generic_ResolvesUnkeyedServiceWhenKeyIsNull()
    {
        using var provider = CreateProvider();

        var service = provider.GetRequiredKeyedOrDefaultService<IRangedService>(null);

        Assert.IsType<FirstRangedService>(service);
    }

    [Fact]
    public void GetRequiredKeyedOrDefaultService_Generic_ResolvesKeyedService()
    {
        using var provider = CreateProvider();

        var service = provider.GetRequiredKeyedOrDefaultService<IRangedService>("key");

        Assert.IsType<SecondRangedService>(service);
    }

    [Fact]
    public void GetRequiredKeyedOrDefaultService_ThrowsWhenServiceIsMissing()
    {
        using var provider = new ServiceCollection().BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
            provider.GetRequiredKeyedOrDefaultService(typeof(IRangedService), null));
        Assert.Throws<InvalidOperationException>(() =>
            provider.GetRequiredKeyedOrDefaultService<IRangedService>("key"));
    }

    [Fact]
    public void GetKeyedServices_Type_ReturnsUnkeyedServicesWhenKeyIsNull()
    {
        using var provider = CreateProviderWithMultipleServices();

        var services = KeyedServiceProviderExtensions
            .GetKeyedServices(provider, typeof(IRangedService), null)
            .ToArray();

        Assert.Equal(
            [typeof(FirstRangedService), typeof(SecondRangedService)],
            services.Select(service => service!.GetType()));
    }

    [Fact]
    public void GetKeyedServices_Type_ReturnsKeyedServices()
    {
        using var provider = CreateProviderWithMultipleServices();

        var services = KeyedServiceProviderExtensions
            .GetKeyedServices(provider, typeof(IRangedService), "key")
            .ToArray();

        Assert.Equal(
            [typeof(SecondRangedService), typeof(FirstRangedService)],
            services.Select(service => service!.GetType()));
    }

    [Fact]
    public void GetKeyedServices_Generic_ReturnsUnkeyedServicesWhenKeyIsNull()
    {
        using var provider = CreateProviderWithMultipleServices();

        var services = KeyedServiceProviderExtensions
            .GetKeyedServices<IRangedService>(provider, null)
            .ToArray();

        Assert.Equal(
            [typeof(FirstRangedService), typeof(SecondRangedService)],
            services.Select(service => service!.GetType()));
    }

    [Fact]
    public void GetKeyedServices_Generic_ReturnsKeyedServices()
    {
        using var provider = CreateProviderWithMultipleServices();

        var services = KeyedServiceProviderExtensions
            .GetKeyedServices<IRangedService>(provider, "key")
            .ToArray();

        Assert.Equal(
            [typeof(SecondRangedService), typeof(FirstRangedService)],
            services.Select(service => service!.GetType()));
    }

    [Fact]
    public void GetKeyedServices_ReturnsEmptyWhenServicesAreMissing()
    {
        using var provider = new ServiceCollection().BuildServiceProvider();

        Assert.Empty(KeyedServiceProviderExtensions
            .GetKeyedServices(provider, typeof(IRangedService), null));
        Assert.Empty(KeyedServiceProviderExtensions
            .GetKeyedServices<IRangedService>(provider, "key"));
    }

    private static ServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddSingleton<IRangedService, FirstRangedService>()
            .AddKeyedSingleton<IRangedService, SecondRangedService>("key")
            .BuildServiceProvider();
    }

    private static ServiceProvider CreateProviderWithMultipleServices()
    {
        return new ServiceCollection()
            .AddSingleton<IRangedService, FirstRangedService>()
            .AddSingleton<IRangedService, SecondRangedService>()
            .AddKeyedSingleton<IRangedService, SecondRangedService>("key")
            .AddKeyedSingleton<IRangedService, FirstRangedService>("key")
            .BuildServiceProvider();
    }
}
