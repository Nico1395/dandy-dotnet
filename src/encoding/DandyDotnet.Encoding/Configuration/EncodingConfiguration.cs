using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Configuration;

public sealed class EncodingConfiguration
{
    public object? ServiceKey { get; set; }
    public Type EncoderType { get; set; } = typeof(Utf8Encoder);
}
