using DandyDotnet.Encoding.Abstractions;

namespace DandyDotnet.Encoding.Codes;

/// <summary>
///     An <see cref="IEncoder"/> implementation that encodes and decodes string payloads using UTF-32 encoding.
/// </summary>
/// <remarks>
///     <para>
///         UTF-32 (32-bit Unicode Transformation Format) encodes each Unicode character using exactly 32 bits (4 bytes).
///         This provides a fixed-width encoding where each character always occupies the same number of bytes,
///         simplifying string manipulation and random access to characters.
///     </para>
///     <para>
///         UTF-32 supports all Unicode characters directly without the need for surrogate pairs or variable-length
///         encoding. This makes it efficient for processing text where character position calculations are frequent,
///         but it uses more space than variable-length encodings like UTF-8 or UTF-16.
///     </para>
///     <para>
///         The encoded data includes a Byte Order Mark (BOM) at the beginning to indicate the byte order.
///         The BOM for UTF-32 is 0x0000FEFF for big-endian or 0xFFFE0000 for little-endian.
///     </para>
///     <para>
///         This encoder is suitable for applications that need constant-time character access or work with
///         a wide range of Unicode characters and prefer simplicity over space efficiency.
///     </para>
/// </remarks>
public sealed class Utf32Encoder : IEncoder
{
    /// <summary>
    ///     Encodes a string as UTF-32 bytes.
    /// </summary>
    /// <param name="payload">The string to encode.</param>
    /// <returns>The encoded <paramref name="payload"/> as a <see cref="ReadOnlyMemory{T}"/> of bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="payload"/> is null.</exception>
    /// <remarks>
    ///     <para>
    ///         Each character in the string is encoded as exactly 4 bytes, regardless of its Unicode code point.
    ///         The resulting byte array will include a Byte Order Mark (BOM) at the beginning.
    ///     </para>
    /// </remarks>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.UTF32.GetBytes(payload);
    }

    /// <summary>
    ///     Decodes UTF-32 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The UTF-32 encoded bytes to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <remarks>
    ///     <para>
    ///         The input byte array should contain valid UTF-32 encoded data. The byte array length must be
    ///         a multiple of 4 for proper decoding. If the byte array includes a BOM, it will be used to determine
    ///         the byte order.
    ///     </para>
    ///     <para>
    ///         If the byte array length is not a multiple of 4, the extra bytes at the end will be ignored as they
    ///         cannot form complete 32-bit code units.
    ///     </para>
    /// </remarks>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.UTF32.GetString(bytes);
    }
}
