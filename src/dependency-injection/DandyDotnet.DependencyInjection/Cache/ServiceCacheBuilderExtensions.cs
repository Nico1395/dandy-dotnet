namespace DandyDotnet.DependencyInjection.Cache;

public static class ServiceCacheBuilderExtensions
{
    public static ServiceCacheBuilder Inject(this ServiceCacheBuilder builder, Type serviceType, object? serviceKey)
    {
        return builder.Inject(new ServiceCacheKey(serviceType, serviceKey));
    }

    public static ServiceCacheBuilder Inject(this ServiceCacheBuilder builder, Type serviceType)
    {
        return builder.Inject(new ServiceCacheKey(serviceType, null));
    }
    
    public static ServiceCacheBuilder Inject<TService>(this ServiceCacheBuilder builder, object? serviceKey)
    {
        return builder.Inject(new ServiceCacheKey(typeof(TService), serviceKey));
    }

    public static ServiceCacheBuilder Inject<TService>(this ServiceCacheBuilder builder)
    {
        return builder.Inject(new ServiceCacheKey(typeof(TService), null));
    }
}