using DandyDotnet.Encoding.Abstractions;

namespace DandyDotnet.Encoding.Codes;

/// <summary>
///     An <see cref="IEncoder"/> implementation that encodes and decodes string payloads using UTF-16 encoding
///     with the system's default byte order.
/// </summary>
/// <remarks>
///     <para>
///         UTF-16 (16-bit Unicode Transformation Format) encodes Unicode characters using 16 bits per character
///         for most common characters (those in the Basic Multilingual Plane, BMP). Characters outside the BMP
///         are encoded as surrogate pairs (two 16-bit code units).
///     </para>
///     <para>
///         The byte order (endianness) used by this encoder is determined by the system's native byte order.
///         On little-endian systems, the encoded data will use little-endian byte order, and on big-endian systems,
///         it will use big-endian byte order. The encoded data includes a Byte Order Mark (BOM) at the beginning
///         to indicate the byte order used.
///     </para>
///     <para>
///         This encoder supports all Unicode characters and is suitable for applications that need full Unicode support.
///         For explicit control over byte order, use <see cref="BigEndianUnicodeEncoder"/> instead.
///     </para>
/// </remarks>
public sealed class UnicodeEncoder : IEncoder
{
    /// <summary>
    ///     Encodes a string as UTF-16 bytes with the system's default byte order.
    /// </summary>
    /// <param name="payload">The string to encode.</param>
    /// <returns>The encoded <paramref name="payload"/> as a <see cref="ReadOnlyMemory{T}"/> of bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="payload"/> is null.</exception>
    /// <remarks>
    ///     <para>
    ///         The resulting byte array will include a Byte Order Mark (BOM) at the beginning (0xFFFE for little-endian
    ///         or 0xFEFF for big-endian, depending on the system's byte order). Each character in the string is
    ///         encoded as two bytes (or four bytes for surrogate pairs).
    ///     </para>
    /// </remarks>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.Unicode.GetBytes(payload);
    }

    /// <summary>
    ///     Decodes UTF-16 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The UTF-16 encoded bytes to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <remarks>
    ///     <para>
    ///         The input byte array should contain valid UTF-16 encoded data. If the byte array includes a BOM,
    ///         it will be used to determine the byte order. If no BOM is present, the system's default byte order
    ///         will be used.
    ///     </para>
    ///     <para>
    ///         For byte arrays with an odd number of bytes, the last byte will be ignored as it cannot form
    ///         a complete 16-bit code unit.
    ///     </para>
    /// </remarks>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.Unicode.GetString(bytes);
    }
}
