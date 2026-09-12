using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests;

public class Latin1EncoderTests
{
    private readonly Latin1Encoder encoder = new();

    [Fact]
    public void Encode_ReturnsLatin1Bytes()
    {
        var encoded = encoder.Encode("Héllo, £!");

        Assert.Equal([72, 233, 108, 108, 111, 44, 32, 163, 33], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsLatin1String()
    {
        var decoded = encoder.Decode([72, 233, 108, 108, 111, 44, 32, 163, 33]);

        Assert.Equal("Héllo, £!", decoded);
    }

    [Fact]
    public void EncodeAndDecode_ReturnsOriginalString()
    {
        const string payload = "Héllo, £!";

        var result = encoder.Decode(encoder.Encode(payload).Span);

        Assert.Equal(payload, result);
    }
}
