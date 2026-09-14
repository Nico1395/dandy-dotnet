using System.Diagnostics.CodeAnalysis;

namespace DandyDotnet.Serialization;

public sealed class SerializerConfiguration
{
    public object? ServiceKey { get; set; }
    public Type? SerializerType { get; set; }
    public object? Configuration { get; set; }

    [MemberNotNullWhen(true, nameof(SerializerType))]
    [MemberNotNullWhen(true, nameof(Configuration))]
    public bool IsValid()
    {
        return SerializerType != null && Configuration != null;
    }
}