using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests;

public class AsciiEncoderTests
{
    private readonly AsciiEncoder _encoder = new();

    [Fact]
    public void Encode_ReturnsAsciiBytes()
    {
        var encoded = _encoder.Encode("Hello, ASCII!");

        Assert.Equal([72, 101, 108, 108, 111, 44, 32, 65, 83, 67, 73, 73, 33], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsAsciiString()
    {
        var decoded = _encoder.Decode([72, 101, 108, 108, 111, 44, 32, 65, 83, 67, 73, 73, 33]);

        Assert.Equal("Hello, ASCII!", decoded);
    }

    [Fact]
    public void EncodeAndDecode_ReturnsOriginalString()
    {
        const string payload = "Hello, ASCII!";

        var result = _encoder.Decode(_encoder.Encode(payload).Span);

        Assert.Equal(payload, result);
    }
}
