using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Abstractions;

/// <summary>
///     Extension methods for adding multiple implementations of the same service type to an <see cref="IServiceCollection" />.
/// </summary>
/// <remarks>
///     <para>
///         Each implementation type is added as its own <see cref="ServiceDescriptor" />. The order of registrations follows
///         the order produced by <c>implementationTypes</c>.
///     </para>
/// </remarks>
public static class RangedRegistrationServiceCollectionExtensions
{
    /// <summary>
    ///     Adds multiple service registrations with the specified lifetime.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddRange(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        foreach (var implementationType in implementationTypes)
        {
            var descriptor = ServiceDescriptor.Describe(serviceType, implementationType, lifetime);
            services.Add(descriptor);
        }

        return services;
    }

    /// <summary>
    ///     Adds multiple service registrations for <typeparamref name="TService" /> with the specified lifetime.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddRange<TService>(this IServiceCollection services, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        return services.AddRange(typeof(TService), implementationTypes, lifetime);
    }

    /// <summary>
    ///     Adds multiple keyed service registrations with the specified lifetime.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">The key associated with the service registrations.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedRange(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        foreach (var implementationType in implementationTypes)
        {
            var descriptor = ServiceDescriptor.DescribeKeyed(serviceType, serviceKey, implementationType, lifetime);
            services.Add(descriptor);
        }

        return services;
    }

    /// <summary>
    ///     Adds multiple keyed service registrations for <typeparamref name="T" /> with the specified lifetime.
    /// </summary>
    /// <typeparam name="T">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceKey">The key associated with the service registrations.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedRange<T>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        return services.AddKeyedRange(typeof(T), serviceKey, implementationTypes, lifetime);
    }
    
    /// <summary>
    ///     Adds multiple transient service registrations.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddTransientRange(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes)
    {
        return services.AddRange(serviceType, implementationTypes, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     Adds multiple transient service registrations for <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddTransientRange<TService>(this IServiceCollection services, IEnumerable<Type> implementationTypes)
    {
        return services.AddTransientRange(typeof(TService), implementationTypes);
    }

    /// <summary>
    ///     Adds multiple keyed transient service registrations.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">The key associated with the service registrations.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedTransientRange(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedRange(serviceType, serviceKey, implementationTypes, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     Adds multiple keyed transient service registrations for <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceKey">The key associated with the service registrations.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedTransientRange<T>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedTransientRange(typeof(T), serviceKey, implementationTypes);
    }
    
    /// <summary>
    ///     Adds multiple scoped service registrations.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddScopedRange(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes)
    {
        return services.AddRange(serviceType, implementationTypes, ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Adds multiple scoped service registrations for <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddScopedRange<TService>(this IServiceCollection services, IEnumerable<Type> implementationTypes)
    {
        return services.AddScopedRange(typeof(TService), implementationTypes);
    }

    /// <summary>
    ///     Adds multiple keyed scoped service registrations.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">The key associated with the service registrations.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedScopedRange(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedRange(serviceType, serviceKey, implementationTypes, ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Adds multiple keyed scoped service registrations for <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceKey">The key associated with the service registrations.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedScoped<T>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedScopedRange(typeof(T), serviceKey, implementationTypes);
    }
    
    /// <summary>
    ///     Adds multiple singleton service registrations.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddSingletonRange(this IServiceCollection services, Type serviceType, IEnumerable<Type> implementationTypes)
    {
        return services.AddRange(serviceType, implementationTypes, ServiceLifetime.Singleton);
    }

    /// <summary>
    ///     Adds multiple singleton service registrations for <typeparamref name="TService" />.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddSingletonRange<TService>(this IServiceCollection services, IEnumerable<Type> implementationTypes)
    {
        return services.AddSingletonRange(typeof(TService), implementationTypes);
    }

    /// <summary>
    ///     Adds multiple keyed singleton service registrations.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">The key associated with the service registrations.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonRange(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedRange(serviceType, serviceKey, implementationTypes, ServiceLifetime.Singleton);
    }

    /// <summary>
    ///     Adds multiple keyed singleton service registrations for <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceKey">The key associated with the service registrations.</param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonRange<T>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedSingletonRange(typeof(T), serviceKey, implementationTypes);
    }
}
