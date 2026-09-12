using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests.Codes;

public class Utf32EncoderTests
{
    private readonly Utf32Encoder _encoder = new();

    [Fact]
    public void Encode_ReturnsUtf32LittleEndianBytes()
    {
        var encoded = _encoder.Encode("AΩ");

        Assert.Equal([65, 0, 0, 0, 169, 3, 0, 0], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsUtf32LittleEndianString()
    {
        var decoded = _encoder.Decode([65, 0, 0, 0, 169, 3, 0, 0]);

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
