using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests;

public class UnicodeEncoderTests
{
    private readonly UnicodeEncoder encoder = new();

    [Fact]
    public void Encode_ReturnsUtf16LittleEndianBytes()
    {
        var encoded = encoder.Encode("AΩ");

        Assert.Equal([65, 0, 169, 3], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsUtf16LittleEndianString()
    {
        var decoded = encoder.Decode([65, 0, 169, 3]);

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
