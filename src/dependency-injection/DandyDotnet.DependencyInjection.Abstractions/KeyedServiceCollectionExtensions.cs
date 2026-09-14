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

    public static IServiceCollection AddKeyedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, object implementationInstance)
    {
        var descriptor = serviceKey == null
            ? ServiceDescriptor.Singleton(serviceType, implementationInstance)
            : ServiceDescriptor.KeyedSingleton(serviceType, serviceKey, implementationInstance);

        services.Add(descriptor);
        return services;
    }

    public static IServiceCollection AddKeyedOrDefault<TService>(this IServiceCollection services, object? serviceKey, TService implementationInstance)
        where TService : class
    {
        return services.AddKeyedOrDefault(typeof(TService), serviceKey, implementationInstance);
    }

    public static IServiceCollection AddKeyedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory, ServiceLifetime lifetime)
    {
        var descriptor = serviceKey == null
            ? ServiceDescriptor.Describe(serviceType, serviceProvider => implementationFactory(serviceProvider, null), lifetime)
            : ServiceDescriptor.DescribeKeyed(serviceType, serviceKey, implementationFactory, lifetime);

        services.Add(descriptor);
        return services;
    }

    public static IServiceCollection AddKeyedOrDefault<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory, ServiceLifetime lifetime)
    {
        return services.AddKeyedOrDefault(
            typeof(TService),
            serviceKey,
            (serviceProvider, key) => implementationFactory(serviceProvider, key)!,
            lifetime);
    }

    public static IServiceCollection AddKeyedOrDefault<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory, ServiceLifetime lifetime)
    {
        return services.AddKeyedOrDefault(
            typeof(TService),
            serviceKey,
            (serviceProvider, key) => implementationFactory(serviceProvider, key)!,
            lifetime);
    }

    public static IServiceCollection AddKeyedTransientOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationFactory, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddKeyedTransientOrDefault<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceKey, implementationFactory, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddKeyedTransientOrDefault<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory)
    {
        return services.AddKeyedOrDefault<TService, TImplementation>(serviceKey, implementationFactory, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddKeyedScopedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationFactory, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddKeyedScopedOrDefault<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceKey, implementationFactory, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddKeyedScopedOrDefault<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory)
    {
        return services.AddKeyedOrDefault<TService, TImplementation>(serviceKey, implementationFactory, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddKeyedSingletonOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationFactory, ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddKeyedSingletonOrDefault<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceKey, implementationFactory, ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddKeyedSingletonOrDefault<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory)
    {
        return services.AddKeyedOrDefault<TService, TImplementation>(serviceKey, implementationFactory, ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddKeyedSingletonOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, object implementationInstance)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationInstance);
    }

    public static IServiceCollection AddKeyedSingletonOrDefault<TService>(this IServiceCollection services, object? serviceKey, TService implementationInstance)
        where TService : class
    {
        return services.AddKeyedOrDefault(serviceKey, implementationInstance);
    }
}