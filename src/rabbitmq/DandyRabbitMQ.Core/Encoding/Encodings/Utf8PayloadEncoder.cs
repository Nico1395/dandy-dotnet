namespace DandyRabbitMQ.Core.Encoding.Encodings;

/// <summary>
/// Encodes payloads using UTF-8.
/// </summary>
public class Utf8PayloadEncoder : IPayloadEncoder
{
    /// <summary>
    /// Encodes a string as UTF-8 bytes.
    /// </summary>
    /// <param name="payload">The payload to encode.</param>
    /// <returns>The encoded <paramref name="payload"/>.</returns>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.UTF8.GetBytes(payload);
    }

    /// <summary>
    /// Decodes UTF-8 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The bytes to decode.</param>
    /// <returns>The decoded payload.</returns>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
