using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.DependencyInjection.Scanning;

public sealed class ScanDescriptor
{
    public required Type AbstractType { get; init; }
    public ServiceLifetime Lifetime { get; init; } = ServiceLifetime.Transient;
    public Func<Type, bool>? Predicate { get; init; }
    public object? ServiceKey  { get; init; }
    public Func<IServiceProvider, object?, object>? KeyedFactory { get; init; }
    public Func<IServiceProvider, object>? Factory { get; init; }
    public bool IsOpenGeneric { get; init; }

    [MemberNotNullWhen(true, nameof(ServiceKey))]
    public bool IsKeyed()
    {
        return ServiceKey != null;
    }
}