using DandyDotnet.Encoding.Abstractions;

namespace DandyDotnet.Encoding.Codes;

/// <summary>
///     An <see cref="IEncoder"/> implementation that encodes and decodes string payloads using ISO-8859-1 (Latin-1) encoding.
/// </summary>
/// <remarks>
///     <para>
///         ISO-8859-1, also known as Latin-1, is an 8-bit single-byte character encoding that covers
///         characters used in Western European languages. It supports 256 characters (code points 0-255),
///         including all ASCII characters plus additional characters such as accented letters,
///         currency symbols, and other special characters.
///     </para>
///     <para>
///         This encoding maps each character to a single byte, making it efficient for storage and transmission.
///         However, it only supports a limited set of characters and cannot represent all Unicode characters.
///         Characters outside the ISO-8859-1 range will be handled according to the underlying
///         <see cref="System.Text.Encoding.Latin1"/> behavior.
///     </para>
///     <para>
///         This encoder is suitable for applications that work with Western European languages and need
///         a simple, efficient encoding scheme.
///     </para>
/// </remarks>
public sealed class Latin1Encoder : IEncoder
{
    /// <summary>
    ///     Encodes a string as ISO-8859-1 bytes.
    /// </summary>
    /// <param name="payload">The string to encode.</param>
    /// <returns>The encoded <paramref name="payload"/> as a <see cref="ReadOnlyMemory{T}"/> of bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="payload"/> is null.</exception>
    /// <remarks>
    ///     <para>
    ///         Each character in the string is encoded as a single byte. Characters that cannot be represented
    ///         in ISO-8859-1 (code points above 255) will be replaced with a question mark (?) character.
    ///     </para>
    /// </remarks>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.Latin1.GetBytes(payload);
    }

    /// <summary>
    ///     Decodes ISO-8859-1 <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The ISO-8859-1 encoded bytes to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <remarks>
    ///     <para>
    ///         Each byte in the input is directly mapped to a character in the ISO-8859-1 character set.
    ///         The resulting string will contain the corresponding characters for each byte value.
    ///     </para>
    /// </remarks>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.Latin1.GetString(bytes);
    }
}
