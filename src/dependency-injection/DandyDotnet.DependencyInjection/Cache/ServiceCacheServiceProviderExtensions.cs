namespace DandyDotnet.DependencyInjection.Cache;

public static class ServiceCacheServiceProviderExtensions
{
    public static ServiceCache CreateServiceCache(this IServiceProvider serviceProvider, Action<ServiceCacheBuilder> builderAction)
    {
        var builder = ServiceCacheBuilder.Create(serviceProvider);
        builderAction(builder);
        return builder.Build();
    }
}