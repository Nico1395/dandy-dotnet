namespace DandyDotnet.DependencyInjection.Cache;

public sealed record ServiceCacheKey(Type ServiceType, object? ServiceKey);