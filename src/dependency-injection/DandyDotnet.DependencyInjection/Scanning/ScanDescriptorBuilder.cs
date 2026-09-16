using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

/// <summary>
///     Builds a <see cref="ScanDescriptor" /> for a service type discovered by scanning assemblies.
/// </summary>
/// <remarks>
///     <para>
///         The builder configures how matching implementation types are filtered and how service descriptors are created
///         for them. Unless configured otherwise, descriptors use <see cref="ServiceLifetime.Transient" /> and do not
///         include open generic implementation type definitions.
///     </para>
/// </remarks>
/// <param name="abstractType">The abstract type that discovered implementation types must implement or derive from.</param>
public sealed class ScanDescriptorBuilder(Type abstractType)
{
    private ServiceLifetime _lifetime = ServiceLifetime.Transient;
    private Func<Type, bool>? _predicate;
    private object? _serviceKey;
    private Func<Type, object?>? _serviceKeyFactory;
    private Func<IServiceProvider, object?, object>? _keyedFactory;
    private Func<IServiceProvider, object>? _factory;
    private bool _allowOpenGeneric;

    /// <summary>
    ///     Adds a predicate used to filter discovered implementation types.
    /// </summary>
    /// <param name="predicate">A predicate that returns <see langword="true" /> for implementation types to register.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ScanDescriptorBuilder When(Func<Type, bool> predicate)
    {
        _predicate = predicate;
        return this;
    }

    /// <summary>
    ///     Sets the lifetime used for generated service descriptors.
    /// </summary>
    /// <param name="lifetime">The service lifetime.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ScanDescriptorBuilder WithLifetime(ServiceLifetime lifetime)
    {
        _lifetime = lifetime;
        return this;
    }

    /// <summary>
    ///     Sets a fixed service key used for generated keyed service descriptors.
    /// </summary>
    /// <param name="serviceKey">The key associated with generated service registrations.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ScanDescriptorBuilder WithKey(object? serviceKey)
    {
        _serviceKey = serviceKey;
        return this;
    }

    /// <summary>
    ///     Sets a factory used to create a service key for each discovered implementation type.
    /// </summary>
    /// <param name="keyFactory">A factory that receives the discovered implementation type and returns its service key.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ScanDescriptorBuilder WithKey(Func<Type, object?> keyFactory)
    {
        _serviceKeyFactory = keyFactory;
        return this;
    }

    /// <summary>
    ///     Sets a factory used to create keyed service instances.
    /// </summary>
    /// <param name="keyedFactory">A factory that receives the service provider and service key.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ScanDescriptorBuilder WithKeyedFactory(Func<IServiceProvider, object?, object> keyedFactory)
    {
        _keyedFactory = keyedFactory;
        return this;
    }

    /// <summary>
    ///     Sets a factory used to create non-keyed service instances.
    /// </summary>
    /// <param name="factory">A factory that receives the service provider.</param>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ScanDescriptorBuilder WithFactory(Func<IServiceProvider, object> factory)
    {
        _factory = factory;
        return this;
    }

    /// <summary>
    ///     Includes open generic implementation type definitions in scanning results.
    /// </summary>
    /// <returns>The same builder instance so that additional calls can be chained.</returns>
    public ScanDescriptorBuilder AllowOpenGeneric()
    {
        _allowOpenGeneric = true;
        return this;
    }

    internal ScanDescriptor Build()
    {
        return new ScanDescriptor
        {
            AbstractType = abstractType,
            Lifetime = _lifetime,
            Predicate = _predicate,
            ServiceKey = _serviceKey,
            ServiceKeyFactory = _serviceKeyFactory,
            KeyedFactory = _keyedFactory,
            Factory = _factory,
            AllowOpenGeneric = _allowOpenGeneric,
        };
    }
}
