using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DandyDotnet.DependencyInjection.Scanning;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ScanAndAdd(this IServiceCollection services, Action<ServiceScannerBuilder> scannerAction)
    {
        var builder = new ServiceScannerBuilder();
        scannerAction(builder);
        var scanner = builder.Build();

        return services.ScanAndAdd(scanner);
    }

    public static IServiceCollection ScanAndAdd(this IServiceCollection services, ServiceScanner scanner)
    {
        var serviceDescriptors = scanner.GetServiceDescriptors();
        return services.Add(serviceDescriptors);
    }
}