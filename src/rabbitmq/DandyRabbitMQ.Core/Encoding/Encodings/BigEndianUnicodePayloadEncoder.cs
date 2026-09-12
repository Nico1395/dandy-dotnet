namespace DandyRabbitMQ.Core.Encoding.Encodings;

/// <summary>
/// Encodes payloads using big-endian UTF-16.
/// </summary>
public class BigEndianUnicodePayloadEncoder : IPayloadEncoder
{
    /// <summary>
    /// Encodes a string as big-endian UTF-16 bytes.
    /// </summary>
    /// <param name="payload">The payload to encode.</param>
    /// <returns>The encoded <paramref name="payload"/>.</returns>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.BigEndianUnicode.GetBytes(payload);
    }

    /// <summary>
    /// Decodes big-endian UTF-16 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The bytes to decode.</param>
    /// <returns>The decoded payload.</returns>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.BigEndianUnicode.GetString(bytes);
    }
}
