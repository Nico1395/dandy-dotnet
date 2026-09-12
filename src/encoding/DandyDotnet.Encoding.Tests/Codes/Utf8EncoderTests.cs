using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests.Codes;

public class Utf8EncoderTests
{
    private readonly Utf8Encoder _encoder = new();

    [Fact]
    public void Encode_ReturnsUtf8Bytes()
    {
        var encoded = _encoder.Encode("Hello, 世界!");

        Assert.Equal([72, 101, 108, 108, 111, 44, 32, 228, 184, 150, 231, 149, 140, 33], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsUtf8String()
    {
        var decoded = _encoder.Decode([72, 101, 108, 108, 111, 44, 32, 228, 184, 150, 231, 149, 140, 33]);

        Assert.Equal("Hello, 世界!", decoded);
    }

    [Fact]
    public void EncodeAndDecode_ReturnsOriginalString()
    {
        const string payload = "Hello, 世界!";

        var result = _encoder.Decode(_encoder.Encode(payload).Span);

        Assert.Equal(payload, result);
    }
}
