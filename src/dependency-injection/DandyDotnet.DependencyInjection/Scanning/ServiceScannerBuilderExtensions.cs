namespace DandyDotnet.DependencyInjection.Scanning;

public static class ServiceScannerBuilderExtensions
{
    public static ServiceScannerBuilder ScanFor(this ServiceScannerBuilder builder, Type abstractType)
    {
        return builder.ScanFor(abstractType, builderAction: null);
    }

    public static ServiceScannerBuilder ScanFor(this ServiceScannerBuilder builder, IEnumerable<Type> abstractTypes)
    {
        foreach (var abstractType in abstractTypes)
            builder.ScanFor(abstractType);
        
        return builder;
    }

    public static ServiceScannerBuilder ScanFor<T>(this ServiceScannerBuilder builder, Action<ScanDescriptorBuilder>? builderAction)
    {
        return builder.ScanFor(typeof(T), builderAction);
    }
    
    public static ServiceScannerBuilder ScanFor<T>(this ServiceScannerBuilder builder)
    {
        return builder.ScanFor(typeof(T), builderAction: null);
    }
}