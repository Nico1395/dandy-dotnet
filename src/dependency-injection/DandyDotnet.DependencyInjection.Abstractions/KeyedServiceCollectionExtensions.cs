using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Abstractions;

/// <summary>
///     Extension methods for <see cref="IServiceCollection" /> that make keyed registrations optional.
/// </summary>
/// <remarks>
///     <para>
///         These helpers add either a keyed <see cref="ServiceDescriptor" /> or a non-keyed <see cref="ServiceDescriptor" />
///         based on whether <c>serviceKey</c> is <see langword="null" />.
///     </para>
///     <para>
///         This is useful when the caller accepts an optional key and wants a single registration path that works for both
///         keyed and non-keyed dependency injection.
///     </para>
/// </remarks>
public static class KeyedServiceCollectionExtensions
{
    /// <summary>
    ///     Adds a service registration using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationType">The concrete implementation type to register.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Type implementationType, ServiceLifetime lifetime)
    {
        var descriptor = serviceKey == null
            ? ServiceDescriptor.Describe(serviceType, implementationType, lifetime)
            : ServiceDescriptor.DescribeKeyed(serviceType, serviceKey, implementationType, lifetime);

        services.Add(descriptor);
        return services;
    }
    
    /// <summary>
    ///     Adds a service registration for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationType">The concrete implementation type to register.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedOrDefault<TService>(this IServiceCollection services, object? serviceKey, Type implementationType, ServiceLifetime lifetime)
    {
        return services.AddKeyedOrDefault(typeof(TService), serviceKey, implementationType, lifetime);
    }

    /// <summary>
    ///     Adds multiple service registrations using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        foreach (var implementationType in implementationTypes)
            services.AddKeyedOrDefault(serviceType, serviceKey, implementationType, lifetime);

        return services;
    }

    /// <summary>
    ///     Adds multiple service registrations for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedOrDefault<TService>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes, ServiceLifetime lifetime)
    {
        return services.AddKeyedOrDefault(typeof(TService), serviceKey, implementationTypes, lifetime);
    }

    /// <summary>
    ///     Adds a transient service registration using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationType">The concrete implementation type to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedTransientOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationType, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     Adds a transient service registration for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationType">The concrete implementation type to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedTransientOrDefault<TService>(this IServiceCollection services, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedTransientOrDefault(typeof(TService), serviceKey, implementationType);
    }

    /// <summary>
    ///     Adds multiple transient service registrations using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedTransientOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationTypes, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     Adds multiple transient service registrations for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedTransientOrDefault<TService>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedTransientOrDefault(typeof(TService), serviceKey, implementationTypes);
    }

    /// <summary>
    ///     Adds a scoped service registration using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationType">The concrete implementation type to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedScopedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationType, ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Adds a scoped service registration for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationType">The concrete implementation type to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedScopedOrDefault<TService>(this IServiceCollection services, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedScopedOrDefault(typeof(TService), serviceKey, implementationType);
    }

    /// <summary>
    ///     Adds multiple scoped service registrations using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedScopedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationTypes, ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Adds multiple scoped service registrations for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedScopedOrDefault<TService>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedScopedOrDefault(typeof(TService), serviceKey, implementationTypes);
    }

    /// <summary>
    ///     Adds a singleton service registration using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationType">The concrete implementation type to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationType, ServiceLifetime.Singleton);
    }

    /// <summary>
    ///     Adds a singleton service registration for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationType">The concrete implementation type to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonOrDefault<TService>(this IServiceCollection services, object? serviceKey, Type implementationType)
    {
        return services.AddKeyedSingletonOrDefault(typeof(TService), serviceKey, implementationType);
    }

    /// <summary>
    ///     Adds multiple singleton service registrations using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationTypes, ServiceLifetime.Singleton);
    }

    /// <summary>
    ///     Adds multiple singleton service registrations for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptors to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationTypes">The concrete implementation types to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonOrDefault<TService>(this IServiceCollection services, object? serviceKey, IEnumerable<Type> implementationTypes)
    {
        return services.AddKeyedSingletonOrDefault(typeof(TService), serviceKey, implementationTypes);
    }

    /// <summary>
    ///     Adds a singleton instance registration using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationInstance">The implementation instance to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, object implementationInstance)
    {
        var descriptor = serviceKey == null
            ? ServiceDescriptor.Singleton(serviceType, implementationInstance)
            : ServiceDescriptor.KeyedSingleton(serviceType, serviceKey, implementationInstance);

        services.Add(descriptor);
        return services;
    }

    /// <summary>
    ///     Adds a singleton instance registration for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationInstance">The implementation instance to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedOrDefault<TService>(this IServiceCollection services, object? serviceKey, TService implementationInstance)
        where TService : class
    {
        return services.AddKeyedOrDefault(typeof(TService), serviceKey, implementationInstance);
    }

    /// <summary>
    ///     Adds a service registration using a factory, using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory, ServiceLifetime lifetime)
    {
        var descriptor = serviceKey == null
            ? ServiceDescriptor.Describe(serviceType, serviceProvider => implementationFactory(serviceProvider, null), lifetime)
            : ServiceDescriptor.DescribeKeyed(serviceType, serviceKey, implementationFactory, lifetime);

        services.Add(descriptor);
        return services;
    }

    /// <summary>
    ///     Adds a service registration for <typeparamref name="TService" /> using a factory, using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedOrDefault<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory, ServiceLifetime lifetime)
    {
        return services.AddKeyedOrDefault(
            typeof(TService),
            serviceKey,
            (serviceProvider, key) => implementationFactory(serviceProvider, key)!,
            lifetime);
    }

    /// <summary>
    ///     Adds a service registration for <typeparamref name="TService" /> using a factory that produces <typeparamref name="TImplementation" />,
    ///     using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type produced by the factory.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedOrDefault<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory, ServiceLifetime lifetime)
    {
        return services.AddKeyedOrDefault(
            typeof(TService),
            serviceKey,
            (serviceProvider, key) => implementationFactory(serviceProvider, key)!,
            lifetime);
    }

    /// <summary>
    ///     Adds a transient service registration using a factory, using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedTransientOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationFactory, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     Adds a transient service registration for <typeparamref name="TService" /> using a factory, using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedTransientOrDefault<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceKey, implementationFactory, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     Adds a transient service registration for <typeparamref name="TService" /> using a factory that produces <typeparamref name="TImplementation" />,
    ///     using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type produced by the factory.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedTransientOrDefault<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory)
    {
        return services.AddKeyedOrDefault<TService, TImplementation>(serviceKey, implementationFactory, ServiceLifetime.Transient);
    }

    /// <summary>
    ///     Adds a scoped service registration using a factory, using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedScopedOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationFactory, ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Adds a scoped service registration for <typeparamref name="TService" /> using a factory, using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedScopedOrDefault<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceKey, implementationFactory, ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Adds a scoped service registration for <typeparamref name="TService" /> using a factory that produces <typeparamref name="TImplementation" />,
    ///     using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type produced by the factory.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedScopedOrDefault<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory)
    {
        return services.AddKeyedOrDefault<TService, TImplementation>(serviceKey, implementationFactory, ServiceLifetime.Scoped);
    }

    /// <summary>
    ///     Adds a singleton service registration using a factory, using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, Func<IServiceProvider, object?, object> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationFactory, ServiceLifetime.Singleton);
    }

    /// <summary>
    ///     Adds a singleton service registration for <typeparamref name="TService" /> using a factory, using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonOrDefault<TService>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TService> implementationFactory)
    {
        return services.AddKeyedOrDefault(serviceKey, implementationFactory, ServiceLifetime.Singleton);
    }

    /// <summary>
    ///     Adds a singleton service registration for <typeparamref name="TService" /> using a factory that produces <typeparamref name="TImplementation" />,
    ///     using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <typeparam name="TImplementation">The implementation type produced by the factory.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationFactory">
    ///     A factory that creates the service instance. When <c>serviceKey</c> is <see langword="null" />, the
    ///     factory is invoked with a <see langword="null" /> key.
    /// </param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonOrDefault<TService, TImplementation>(this IServiceCollection services, object? serviceKey, Func<IServiceProvider, object?, TImplementation> implementationFactory)
    {
        return services.AddKeyedOrDefault<TService, TImplementation>(serviceKey, implementationFactory, ServiceLifetime.Singleton);
    }

    /// <summary>
    ///     Adds a singleton instance registration using keyed registration when a key is provided.
    /// </summary>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceType">The service type to register.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationInstance">The implementation instance to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonOrDefault(this IServiceCollection services, Type serviceType, object? serviceKey, object implementationInstance)
    {
        return services.AddKeyedOrDefault(serviceType, serviceKey, implementationInstance);
    }

    /// <summary>
    ///     Adds a singleton instance registration for <typeparamref name="TService" /> using keyed registration when a key is provided.
    /// </summary>
    /// <typeparam name="TService">The service type to register.</typeparam>
    /// <param name="services">The service collection to add the descriptor to.</param>
    /// <param name="serviceKey">
    ///     The service key to register with, or <see langword="null" /> to register as a non-keyed service.
    /// </param>
    /// <param name="implementationInstance">The implementation instance to register.</param>
    /// <returns>The same <paramref name="services" /> instance so that additional calls can be chained.</returns>
    public static IServiceCollection AddKeyedSingletonOrDefault<TService>(this IServiceCollection services, object? serviceKey, TService implementationInstance)
        where TService : class
    {
        return services.AddKeyedOrDefault(serviceKey, implementationInstance);
    }
}
