using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Abstractions;

public static class RangedRegistrationServiceCollectionExtensions
{
    public static IServiceCollection AddRange(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        foreach (var implementationType in implementationTypes)
        {
            var descriptor = ServiceDescriptor.Describe(serviceType, implementationType, lifetime);
            services.Add(descriptor);
        }

        return services;
    }

    public static IServiceCollection AddRange<TService>(this IServiceCollection services, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        return services.AddRange(typeof(TService), implementationTypes, lifetime);
    }

    public static IServiceCollection AddKeyedRange(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        foreach (var implementationType in implementationTypes)
        {
            var descriptor = ServiceDescriptor.DescribeKeyed(serviceType, serviceKey, implementationType, lifetime);
            services.Add(descriptor);
        }

        return services;
    }

    public static IServiceCollection AddKeyedRange<T>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        return services.AddKeyedRange(typeof(T), serviceKey, implementationTypes, lifetime);
    }
    
    public static IServiceCollection AddTransientRange(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes)
    {
        return services.AddRange(serviceType, implementationTypes, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddTransientRange<TService>(this IServiceCollection services, IEnumerable<Type> implementationTypes)
    {
        return services.AddTransientRange(typeof(TService), implementationTypes);
    }

    public static IServiceCollection AddKeyedTransientRange(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedRange(serviceType, serviceKey, implementationTypes, ServiceLifetime.Transient);
    }

    public static IServiceCollection AddKeyedTransientRange<T>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedTransientRange(typeof(T), serviceKey, implementationTypes);
    }
    
    public static IServiceCollection AddScopedRange(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes)
    {
        return services.AddRange(serviceType, implementationTypes, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddScopedRange<TService>(this IServiceCollection services, IEnumerable<Type> implementationTypes)
    {
        return services.AddScopedRange(typeof(TService), implementationTypes);
    }

    public static IServiceCollection AddKeyedScopedRange(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedRange(serviceType, serviceKey, implementationTypes, ServiceLifetime.Scoped);
    }

    public static IServiceCollection AddKeyedScoped<T>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedScopedRange(typeof(T), serviceKey, implementationTypes);
    }
    
    public static IServiceCollection AddSingletonRange(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes)
    {
        return services.AddRange(serviceType, implementationTypes, ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddSingletonRange<TService>(this IServiceCollection services, IEnumerable<Type> implementationTypes)
    {
        return services.AddSingletonRange(typeof(TService), implementationTypes);
    }

    public static IServiceCollection AddKeyedSingletonRange(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedRange(serviceType, serviceKey, implementationTypes, ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddKeyedSingletonRange<T>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedSingletonRange(typeof(T), serviceKey, implementationTypes);
    }
}