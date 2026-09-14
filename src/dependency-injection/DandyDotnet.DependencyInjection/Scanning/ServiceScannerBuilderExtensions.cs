namespace DandyDotnet.DependencyInjection.Scanning;

public static class ServiceScannerBuilderExtensions
{
    public static ServiceScannerBuilder ScanFor<T>(this ServiceScannerBuilder builder, Action<ScanDescriptorBuilder> builderAction)
    {
        return builder.ScanFor(typeof(T), builderAction);
    }
}