using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

public static class ScanDescriptorBuilderExtensions
{
    public static ScanDescriptorBuilder AsTransient(this ScanDescriptorBuilder builder)
    {
        return builder.WithLifetime(ServiceLifetime.Transient);
    }
    
    public static ScanDescriptorBuilder AsScoped(this ScanDescriptorBuilder builder)
    {
        return builder.WithLifetime(ServiceLifetime.Scoped);
    }

    public static ScanDescriptorBuilder AsSingleton(this ScanDescriptorBuilder builder)
    {
        return builder.WithLifetime(ServiceLifetime.Singleton);
    }
}