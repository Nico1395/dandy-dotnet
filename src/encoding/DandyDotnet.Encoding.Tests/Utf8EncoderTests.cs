using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests;

public class Utf8EncoderTests
{
    private readonly Utf8Encoder encoder = new();

    [Fact]
    public void Encode_ReturnsUtf8Bytes()
    {
        var encoded = encoder.Encode("Hello, 世界!");

        Assert.Equal([72, 101, 108, 108, 111, 44, 32, 228, 184, 150, 231, 149, 140, 33], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsUtf8String()
    {
        var decoded = encoder.Decode([72, 101, 108, 108, 111, 44, 32, 228, 184, 150, 231, 149, 140, 33]);

        Assert.Equal("Hello, 世界!", decoded);
    }

    [Fact]
    public void EncodeAndDecode_ReturnsOriginalString()
    {
        const string payload = "Hello, 世界!";

        var result = encoder.Decode(encoder.Encode(payload).Span);

        Assert.Equal(payload, result);
    }
}
