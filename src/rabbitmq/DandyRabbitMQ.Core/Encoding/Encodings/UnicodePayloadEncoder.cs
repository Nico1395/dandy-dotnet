namespace DandyRabbitMQ.Core.Encoding.Encodings;

/// <summary>
/// Encodes payloads using UTF-16.
/// </summary>
public class UnicodePayloadEncoder : IPayloadEncoder
{
    /// <summary>
    /// Encodes a string as UTF-16 bytes.
    /// </summary>
    /// <param name="payload">The payload to encode.</param>
    /// <returns>The encoded <paramref name="payload"/>.</returns>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.Unicode.GetBytes(payload);
    }

    /// <summary>
    /// Decodes UTF-16 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The bytes to decode.</param>
    /// <returns>The decoded payload.</returns>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.Unicode.GetString(bytes);
    }
}
