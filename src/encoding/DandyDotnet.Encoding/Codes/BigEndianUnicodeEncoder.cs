using DandyDotnet.Encoding.Abstractions;

namespace DandyDotnet.Encoding.Codes;

/// <summary>
///     An <see cref="IEncoder"/> implementation that encodes and decodes string payloads using big-endian UTF-16 encoding.
/// </summary>
/// <remarks>
///     <para>
///         UTF-16 (16-bit Unicode Transformation Format) encodes Unicode characters using 16 bits per character
///         for most common characters (those in the Basic Multilingual Plane, BMP). Characters outside the BMP
///         are encoded as surrogate pairs (two 16-bit code units).
///     </para>
///     <para>
///         Big-endian UTF-16 stores the most significant byte first. This encoding is useful for interoperability
///         with systems that require big-endian byte order, such as some network protocols or file formats.
///         The encoded data starts with a Byte Order Mark (BOM) of 0xFEFF to indicate big-endian encoding.
///     </para>
///     <para>
///         This encoder supports all Unicode characters and is suitable for applications that need full Unicode support
///         with big-endian byte ordering.
///     </para>
/// </remarks>
public sealed class BigEndianUnicodeEncoder : IEncoder
{
    /// <summary>
    ///     Encodes a string as big-endian UTF-16 bytes.
    /// </summary>
    /// <param name="payload">The string to encode.</param>
    /// <returns>The encoded <paramref name="payload"/> as a <see cref="ReadOnlyMemory{T}"/> of bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="payload"/> is null.</exception>
    /// <remarks>
    ///     <para>
    ///         The resulting byte array will include a Byte Order Mark (BOM) at the beginning (0xFEFF for big-endian).
    ///         Each character in the string is encoded as two bytes (or four bytes for surrogate pairs).
    ///     </para>
    /// </remarks>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.BigEndianUnicode.GetBytes(payload);
    }

    /// <summary>
    ///     Decodes big-endian UTF-16 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The big-endian UTF-16 encoded bytes to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <remarks>
    ///     <para>
    ///         The input byte array should contain valid big-endian UTF-16 encoded data. If the byte array
    ///         includes a BOM (0xFEFF), it will be used to confirm the encoding. If the BOM indicates little-endian
    ///         (0xFFFE), the data may be decoded incorrectly.
    ///     </para>
    ///     <para>
    ///         For byte arrays with an odd number of bytes, the last byte will be ignored as it cannot form
    ///         a complete 16-bit code unit.
    ///     </para>
    /// </remarks>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.BigEndianUnicode.GetString(bytes);
    }
}
