namespace DandyRabbitMQ.Core.Encoding.Encodings;

/// <summary>
/// Encodes payloads using ASCII.
/// </summary>
public class AsciiPayloadEncoder : IPayloadEncoder
{
    /// <summary>
    /// Encodes a string as ASCII bytes.
    /// </summary>
    /// <param name="payload">The payload to encode.</param>
    /// <returns>The encoded <paramref name="payload"/>.</returns>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.ASCII.GetBytes(payload);
    }

    /// <summary>
    /// Decodes ASCII <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The bytes to decode.</param>
    /// <returns>The decoded payload.</returns>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.ASCII.GetString(bytes);
    }
}
