using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

/// <summary>
///     Describes how implementation types discovered by a <see cref="ServiceScanner" /> should be registered.
/// </summary>
/// <remarks>
///     <para>
///         A descriptor identifies the abstract service type to scan for, the lifetime to use for generated service
///         descriptors, optional filtering rules, optional keyed-service configuration, and optional factories.
///     </para>
/// </remarks>
public sealed class ScanDescriptor
{
    /// <summary>
    ///     Gets the abstract type that discovered implementation types must implement or derive from.
    /// </summary>
    public required Type AbstractType { get; init; }

    /// <summary>
    ///     Gets the lifetime used for generated service descriptors.
    /// </summary>
    public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Transient;

    /// <summary>
    ///     Gets the predicate used to filter discovered implementation types.
    /// </summary>
    public Func<Type, bool>? Predicate { get; init; }

    /// <summary>
    ///     Gets the fixed service key used for generated keyed service descriptors.
    /// </summary>
    public object? ServiceKey  { get; init; }

    /// <summary>
    ///     Gets the factory used to create a service key for each discovered implementation type.
    /// </summary>
    public Func<Type, object?>? ServiceKeyFactory { get; init; }

    /// <summary>
    ///     Gets the factory used to create keyed service instances.
    /// </summary>
    public Func<IServiceProvider, object?, object>? KeyedFactory { get; init; }

    /// <summary>
    ///     Gets the factory used to create non-keyed service instances.
    /// </summary>
    public Func<IServiceProvider, object>? Factory { get; init; }

    /// <summary>
    ///     Gets a value indicating whether open generic implementation types are included in scanning results.
    /// </summary>
    public bool AllowOpenGeneric { get; init; }

    /// <summary>
    ///     Determines whether this descriptor produces keyed service descriptors.
    /// </summary>
    /// <returns>
    ///     <see langword="true" /> if a fixed service key or service key factory is configured; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool IsKeyed()
    {
        return ServiceKey != null || ServiceKeyFactory != null;
    }
}
