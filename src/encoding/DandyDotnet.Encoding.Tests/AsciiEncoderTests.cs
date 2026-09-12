using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Tests;

public class AsciiEncoderTests
{
    private readonly AsciiEncoder encoder = new();

    [Fact]
    public void Encode_ReturnsAsciiBytes()
    {
        var encoded = encoder.Encode("Hello, ASCII!");

        Assert.Equal([72, 101, 108, 108, 111, 44, 32, 65, 83, 67, 73, 73, 33], encoded.ToArray());
    }

    [Fact]
    public void Decode_ReturnsAsciiString()
    {
        var decoded = encoder.Decode([72, 101, 108, 108, 111, 44, 32, 65, 83, 67, 73, 73, 33]);

        Assert.Equal("Hello, ASCII!", decoded);
    }

    [Fact]
    public void EncodeAndDecode_ReturnsOriginalString()
    {
        const string payload = "Hello, ASCII!";

        var result = encoder.Decode(encoder.Encode(payload).Span);

        Assert.Equal(payload, result);
    }
}
