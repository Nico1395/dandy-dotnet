namespace DandyDotnet.DependencyInjection.Cache;

/// <summary>
///     Identifies a cached service by service type and optional service key.
/// </summary>
/// <remarks>
///     <para>
///         A <see langword="null" /> <see cref="ServiceKey" /> represents a non-keyed service registration. Any other
///         value represents a keyed service registration using the same key value that was used when the service was
///         registered with the dependency injection container.
///     </para>
/// </remarks>
/// <param name="ServiceType">The type of service to cache.</param>
/// <param name="ServiceKey">The optional key associated with the service registration.</param>
public sealed record ServiceCacheKey(Type ServiceType, object? ServiceKey);
