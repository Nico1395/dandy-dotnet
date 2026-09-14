using DandyDotnet.DependencyInjection.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Tests.Cache;

public sealed class ServiceCacheTests
{
    [Fact]
    public void Get_CachesUnkeyedService()
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
        var cache = new ServiceCache(provider, null);

        var first = cache.Get(new ServiceCacheKey(typeof(object), null));
        var second = cache.Get<object>();

        Assert.Same(service, first);
        Assert.Same(first, second);
        Assert.Equal(1, factoryCalls);
    }

    [Fact]
    public void Get_CachesKeyedService()
    {
        var factoryCalls = 0;
        var service = new object();
        using var provider = new ServiceCollection()
            .AddKeyedTransient<object>("key", (_, _) =>
            {
                factoryCalls++;
                return service;
            })
            .BuildServiceProvider();
        var cache = new ServiceCache(provider, null);

        var first = cache.Get<object>("key");
        var second = cache.Get(new ServiceCacheKey(typeof(object), "key"));

        Assert.Same(service, first);
        Assert.Same(first, second);
        Assert.Equal(1, factoryCalls);
    }

    [Fact]
    public void Constructor_InjectsServicesImmediately()
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

        var cache = new ServiceCache(provider, [new ServiceCacheKey(typeof(object), null)]);

        Assert.Equal(1, factoryCalls);
        Assert.Same(service, cache.Get<object>());
    }

    [Fact]
    public void Constructor_ThrowsWhenInjectedServiceIsMissing()
    {
        using var provider = new ServiceCollection().BuildServiceProvider();

        Assert.Throws<InvalidOperationException>(() =>
            new ServiceCache(provider, [new ServiceCacheKey(typeof(object), null)]));
    }

    [Fact]
    public void Dispose_DisposesCachedDisposableServices()
    {
        var service = new DisposableService();
        using var provider = new ServiceCollection()
            .AddSingleton(service)
            .BuildServiceProvider();
        var cache = new ServiceCache(provider, null);

        Assert.Same(service, cache.Get<DisposableService>());
        cache.Dispose();

        Assert.True(service.IsDisposed);
    }

    [Fact]
    public async Task DisposeAsync_DisposesCachedAsyncDisposableServices()
    {
        var service = new AsyncDisposableService();
        await using var provider = new ServiceCollection()
            .AddSingleton(service)
            .BuildServiceProvider();
        var cache = new ServiceCache(provider, null);

        Assert.Same(service, cache.Get<AsyncDisposableService>());
        await cache.DisposeAsync();

        Assert.True(service.IsDisposed);
    }

    private sealed class DisposableService : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }

    private sealed class AsyncDisposableService : IAsyncDisposable
    {
        public bool IsDisposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            IsDisposed = true;
            return ValueTask.CompletedTask;
        }
    }
}
