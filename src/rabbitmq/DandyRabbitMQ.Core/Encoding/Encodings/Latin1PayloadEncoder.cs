namespace DandyRabbitMQ.Core.Encoding.Encodings;

/// <summary>
/// Encodes payloads using ISO-8859-1.
/// </summary>
public class Latin1PayloadEncoder : IPayloadEncoder
{
    /// <summary>
    /// Encodes a string as ISO-8859-1 bytes.
    /// </summary>
    /// <param name="payload">The payload to encode.</param>
    /// <returns>The encoded <paramref name="payload"/>.</returns>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.Latin1.GetBytes(payload);
    }

    /// <summary>
    /// Decodes ISO-8859-1 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The bytes to decode.</param>
    /// <returns>The decoded payload.</returns>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.Latin1.GetString(bytes);
    }
}
