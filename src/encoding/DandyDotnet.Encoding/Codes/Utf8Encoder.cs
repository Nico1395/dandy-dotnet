using DandyDotnet.Encoding.Abstractions;
using DandyDotnet.Encoding.Configuration;

namespace DandyDotnet.Encoding.Codes;

/// <summary>
///     An <see cref="IEncoder"/> implementation that encodes and decodes string payloads using UTF-8 encoding.
/// </summary>
/// <remarks>
///     <para>
///         UTF-8 (8-bit Unicode Transformation Format) is a variable-width character encoding that can represent
///         every character in the Unicode character set. It uses between one and four bytes per character,
///         making it space-efficient for ASCII and Western European text while still supporting all Unicode characters.
///     </para>
///     <para>
///         UTF-8 is the most widely used Unicode encoding and is the default encoding for web pages, emails,
///         and many other applications. It is backward compatible with ASCII - all ASCII characters are encoded
///         as single bytes with the same values as in ASCII.
///     </para>
///     <para>
///         This encoder is the default encoding used by <see cref="EncodingConfiguration"/> and is recommended
///         for most applications due to its balance of space efficiency and full Unicode support.
///     </para>
/// </remarks>
public sealed class Utf8Encoder : IEncoder
{
    /// <summary>
    ///     Encodes a string as UTF-8 bytes.
    /// </summary>
    /// <param name="payload">The string to encode.</param>
    /// <returns>The encoded <paramref name="payload"/> as a <see cref="ReadOnlyMemory{T}"/> of bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="payload"/> is null.</exception>
    /// <remarks>
    ///     <para>
    ///         ASCII characters (code points 0-127) are encoded as single bytes. Characters beyond the ASCII range
    ///         are encoded using 2, 3, or 4 bytes depending on their Unicode code point. The encoding does not
    ///         include a Byte Order Mark (BOM).
    ///     </para>
    ///     <para>
    ///         This encoding is optimal for text that primarily contains ASCII characters, as it uses the least
    ///         amount of space for such text while still supporting the full range of Unicode characters.
    ///     </para>
    /// </remarks>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.UTF8.GetBytes(payload);
    }

    /// <summary>
    ///     Decodes UTF-8 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The UTF-8 encoded bytes to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <remarks>
    ///     <para>
    ///         The input byte array should contain valid UTF-8 encoded data. The decoder will interpret the
    ///         byte sequence according to UTF-8 rules, where the first byte of each character indicates how many
    ///         additional bytes are part of that character.
    ///     </para>
    ///     <para>
    ///         Invalid UTF-8 sequences (such as incomplete multi-byte sequences or invalid byte patterns) may
    ///         result in replacement characters (U+FFFD) or exceptions, depending on the underlying
    ///         <see cref="System.Text.Encoding.UTF8"/> behavior.
    ///     </para>
    /// </remarks>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}
