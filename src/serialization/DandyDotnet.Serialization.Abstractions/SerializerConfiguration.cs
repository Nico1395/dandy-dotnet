using System.Diagnostics.CodeAnalysis;

namespace DandyDotnet.Serialization.Abstractions;

internal sealed class SerializerConfiguration
{
    public Type? Type { get; set; }
    public object? Configuration { get; set; }

    [MemberNotNullWhen(true, nameof(Type))]
    [MemberNotNullWhen(true, nameof(Configuration))]
    public bool IsValid()
    {
        return Type != null && Configuration != null;
    }
}