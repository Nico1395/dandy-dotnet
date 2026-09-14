using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Abstractions;

public static class KeyedServiceProviderExtensions
{
    public static object? GetKeyedOrDefaultService(this IServiceProvider serviceProvider, Type serviceType, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetService(serviceType)
            : serviceProvider.GetKeyedService(serviceType, serviceKey);
    }
    
    public static TService? GetKeyedOrDefaultService<TService>(this IServiceProvider serviceProvider, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetService<TService>()
            : serviceProvider.GetKeyedService<TService>(serviceKey);
    }

    public static object GetRequiredKeyedOrDefaultService(this IServiceProvider serviceProvider, Type serviceType, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetRequiredService(serviceType)
            : serviceProvider.GetRequiredKeyedService(serviceType, serviceKey);
    }

    public static TService GetRequiredKeyedOrDefaultService<TService>(this IServiceProvider serviceProvider, object? serviceKey)
        where TService : notnull
    {
        return serviceKey == null
            ? serviceProvider.GetRequiredService<TService>()
            : serviceProvider.GetRequiredKeyedService<TService>(serviceKey);
    }

    public static IEnumerable<object?> GetKeyedServices(this IServiceProvider serviceProvider, Type serviceType, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetServices(serviceType)
            : serviceProvider.GetKeyedServices(serviceType, serviceKey);
    }

    public static IEnumerable<TService?> GetKeyedServices<TService>(this IServiceProvider serviceProvider, object? serviceKey)
    {
        return serviceKey == null
            ? serviceProvider.GetServices<TService>()
            : serviceProvider.GetKeyedServices<TService>(serviceKey);
    }
}