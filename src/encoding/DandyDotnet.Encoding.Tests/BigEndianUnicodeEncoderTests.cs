using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests;

public class BigEndianUnicodeEncoderTests
{
    private readonly BigEndianUnicodeEncoder _encoder = new();

    [Fact]
    public void Encode_ReturnsUtf16BigEndianBytes()
    {
        var encoded = _encoder.Encode("AΩ");

        Assert.Equal([0, 65, 3, 169], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsUtf16BigEndianString()
    {
        var decoded = _encoder.Decode([0, 65, 3, 169]);

        Assert.Equal("AΩ", decoded);
    }

    [Fact]
    public void EncodeAndDecode_ReturnsOriginalString()
    {
        const string payload = "AΩ";

        var result = _encoder.Decode(_encoder.Encode(payload).Span);

        Assert.Equal(payload, result);
    }
}
