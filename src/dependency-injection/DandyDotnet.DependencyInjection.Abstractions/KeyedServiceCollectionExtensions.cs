using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Abstractions;

public static class KeyedServiceCollectionExtensions
{
    public static IServiceCollection AddKeyedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Type implementationType, ServiceLifetime lifetime)
    {
        var descriptor = serviceKey == null
            ? ServiceDescriptor.Describe(serviceType, implementationType, lifetime)
            : ServiceDescriptor.DescribeKeyed(serviceType, serviceKey, implementationType, lifetime);

        services.Add(descriptor);
        return services;
    }
    
    public static IServiceCollection AddKeyedOrDefault<TService>(this IServiceCollection services, object? serviceKey, Type implementationType, ServiceLifetime lifetime)
    {
        return services.AddKeyedOrDefault(typeof(TService), serviceKey, implementationType, lifetime);
    }

    public static IServiceCollection AddKeyedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        foreach (var implementationType in implementationTypes)
            services.AddKeyedOrDefault(serviceType, serviceKey, implementationType, lifetime);

        return services;
    }

    public static IServiceCollection AddKeyedOrDefault<TService>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        return services.AddKeyedOrDefault(typeof(TService), serviceKey, implementationTypes, lifetime);
    }

    public static IServiceCollection AddKeyedTransientOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationType, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddKeyedTransientOrDefault<TService>(this IServiceCollection services, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedTransientOrDefault(typeof(TService), serviceKey, implementationType);
    }

    public static IServiceCollection AddKeyedTransientOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationTypes, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddKeyedTransientOrDefault<TService>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedTransientOrDefault(typeof(TService), serviceKey, implementationTypes);
    }

    public static IServiceCollection AddKeyedScopedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationType, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddKeyedScopedOrDefault<TService>(this IServiceCollection services, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedScopedOrDefault(typeof(TService), serviceKey, implementationType);
    }

    public static IServiceCollection AddKeyedScopedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationTypes, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddKeyedScopedOrDefault<TService>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedScopedOrDefault(typeof(TService), serviceKey, implementationTypes);
    }

    public static IServiceCollection AddKeyedSingletonOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationType, ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddKeyedSingletonOrDefault<TService>(this IServiceCollection services, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedSingletonOrDefault(typeof(TService), serviceKey, implementationType);
    }

    public static IServiceCollection AddKeyedSingletonOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationTypes, ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddKeyedSingletonOrDefault<TService>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedSingletonOrDefault(typeof(TService), serviceKey, implementationTypes);
    }
}