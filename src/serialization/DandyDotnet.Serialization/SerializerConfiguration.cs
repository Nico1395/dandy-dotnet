using System.Diagnostics.CodeAnalysis;

namespace DandyDotnet.Serialization;

internal sealed class SerializerConfiguration
{
    public Type? SerializerType { get; set; }
    public object? Configuration { get; set; }

    [MemberNotNullWhen(true, nameof(SerializerType))]
    [MemberNotNullWhen(true, nameof(Configuration))]
    public bool IsValid()
    {
        return SerializerType != null && Configuration != null;
    }
}