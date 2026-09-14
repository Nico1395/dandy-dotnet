using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

public sealed class ScanDescriptor
{
    public required Type AbstractType { get; init; }
    public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Transient;
    public Func<Type, bool>? Predicate { get; init; }
    public object? Key  { get; init; }
    public Func<IServiceProvider, object?, object>? KeyedFactory { get; init; }
    public Func<IServiceProvider, object>? Factory { get; init; }
    public bool IsOpenGeneric { get; init; }
}