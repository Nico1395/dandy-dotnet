using DandyDotnet.DependencyInjection.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Cache;

public sealed class ServiceCacheBuilderTests
{
    [Fact]
    public void Create_ReturnsBuilderUsingProvidedServiceProvider()
    {
        var service = new object();
        using var provider = new ServiceCollection()
            .AddSingleton(service)
            .BuildServiceProvider();

        var cache = ServiceCacheBuilder.Create(provider)
            .Inject(new ServiceCacheKey(typeof(object), null))
            .Build();

        Assert.Same(service, cache.Get<object>());
        Assert.Same(provider, cache.ServiceProvider);
    }

    [Fact]
    public void Inject_ReturnsSameBuilder()
    {
        using var provider = new ServiceCollection().BuildServiceProvider();
        var builder = ServiceCacheBuilder.Create(provider);

        Assert.Same(builder, builder.Inject(new ServiceCacheKey(typeof(object), null)));
        Assert.Same(builder, builder.Inject([new ServiceCacheKey(typeof(object), null)]));
    }

    [Fact]
    public void Build_DeduplicatesInjectedKeys()
    {
        var factoryCalls = 0;
        var service = new object();
        using var provider = new ServiceCollection()
            .AddTransient(_ =>
            {
                factoryCalls++;
                return service;
            })
            .BuildServiceProvider();
        var key = new ServiceCacheKey(typeof(object), null);

        var cache = ServiceCacheBuilder.Create(provider)
            .Inject(key)
            .Inject(key)
            .Build();

        Assert.Equal(1, factoryCalls);
        Assert.Same(service, cache.Get<object>());
    }
}
