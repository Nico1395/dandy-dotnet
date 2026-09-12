using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests;

public class Utf32EncoderTests
{
    private readonly Utf32Encoder encoder = new();

    [Fact]
    public void Encode_ReturnsUtf32LittleEndianBytes()
    {
        var encoded = encoder.Encode("AΩ");

        Assert.Equal([65, 0, 0, 0, 169, 3, 0, 0], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsUtf32LittleEndianString()
    {
        var decoded = encoder.Decode([65, 0, 0, 0, 169, 3, 0, 0]);

        Assert.Equal("AΩ", decoded);
    }

    [Fact]
    public void EncodeAndDecode_ReturnsOriginalString()
    {
        const string payload = "AΩ";

        var result = encoder.Decode(encoder.Encode(payload).Span);

        Assert.Equal(payload, result);
    }
}
