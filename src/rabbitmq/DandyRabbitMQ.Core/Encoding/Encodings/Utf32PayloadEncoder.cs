namespace DandyRabbitMQ.Core.Encoding.Encodings;

/// <summary>
/// Encodes payloads using UTF-32.
/// </summary>
public class Utf32PayloadEncoder : IPayloadEncoder
{
    /// <summary>
    /// Encodes a string as UTF-32 bytes.
    /// </summary>
    /// <param name="payload">The payload to encode.</param>
    /// <returns>The encoded <paramref name="payload"/>.</returns>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.UTF32.GetBytes(payload);
    }

    /// <summary>
    /// Decodes UTF-32 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The bytes to decode.</param>
    /// <returns>The decoded payload.</returns>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.UTF32.GetString(bytes);
    }
}
