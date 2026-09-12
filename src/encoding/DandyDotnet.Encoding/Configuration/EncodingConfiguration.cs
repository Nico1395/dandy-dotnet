using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Configuration;

public sealed class EncodingConfiguration
{
    public Type EncoderType { get; set; } = typeof(Utf8Encoder);
}
