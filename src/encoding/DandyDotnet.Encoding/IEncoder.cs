namespace DandyDotnet.Encoding;

public interface IEncoder
{
    ReadOnlyMemory<byte> Encode(string payload);
    string Decode(ReadOnlySpan<byte> bytes);
}