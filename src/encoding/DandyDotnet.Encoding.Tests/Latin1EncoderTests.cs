using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests;

public class Latin1EncoderTests
{
    private readonly Latin1Encoder _encoder = new();

    [Fact]
    public void Encode_ReturnsLatin1Bytes()
    {
        var encoded = _encoder.Encode("Héllo, £!");

        Assert.Equal([72, 233, 108, 108, 111, 44, 32, 163, 33], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsLatin1String()
    {
        var decoded = _encoder.Decode([72, 233, 108, 108, 111, 44, 32, 163, 33]);

        Assert.Equal("Héllo, £!", decoded);
    }

    [Fact]
    public void EncodeAndDecode_ReturnsOriginalString()
    {
        const string payload = "Héllo, £!";

        var result = _encoder.Decode(_encoder.Encode(payload).Span);

        Assert.Equal(payload, result);
    }
}
