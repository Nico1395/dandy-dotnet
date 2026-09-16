using DandyDotnet.Encoding.Abstractions;

namespace DandyDotnet.Encoding.Codes;

/// <summary>
///     An <see cref="IEncoder"/> implementation that encodes and decodes string payloads using ASCII encoding.
/// </summary>
/// <remarks>
///     <para>
///         ASCII (American Standard Code for Information Interchange) is a 7-bit character encoding that represents
///         128 characters, including control characters, digits, punctuation, and basic Latin letters.
///         Each character is encoded as a single byte (8 bits), with the most significant bit set to 0.
///     </para>
///     <para>
///         This encoder is suitable for applications that need to work with ASCII-compatible text only.
///         For Unicode characters outside the ASCII range (code points 0-127), this encoder will either
///         replace them with a question mark (?) or throw an exception, depending on the underlying
///         <see cref="System.Text.Encoding.ASCII"/> behavior.
///     </para>
///     <para>
///         See <see cref="Utf8Encoder"/> for a more versatile encoding that supports all Unicode characters.
///     </para>
/// </remarks>
public sealed class AsciiEncoder : IEncoder
{
    /// <summary>
    ///     Encodes a string as ASCII bytes.
    /// </summary>
    /// <param name="payload">The string to encode.</param>
    /// <returns>The encoded <paramref name="payload"/> as a <see cref="ReadOnlyMemory{T}"/> of bytes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="payload"/> is null.</exception>
    /// <remarks>
    ///     <para>
    ///         Characters outside the ASCII range (code points 0-127) will be replaced with a question mark (?)
    ///         in the resulting byte array.
    ///     </para>
    /// </remarks>
    public ReadOnlyMemory<byte> Encode(string payload)
    {
        return System.Text.Encoding.ASCII.GetBytes(payload);
    }

    /// <summary>
    ///     Decodes ASCII <paramref name="bytes"/> into a string.
    /// </summary>
    /// <param name="bytes">The ASCII-encoded bytes to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <remarks>
    ///     <para>
    ///         This method assumes that the input byte array contains valid ASCII-encoded data.
    ///         Each byte should represent a value between 0 and 127. Bytes with values greater than 127
    ///         will be treated as invalid ASCII and may result in replacement characters or exceptions,
    ///         depending on the underlying <see cref="System.Text.Encoding.ASCII"/> behavior.
    ///     </para>
    /// </remarks>
    public string Decode(ReadOnlySpan<byte> bytes)
    {
        return System.Text.Encoding.ASCII.GetString(bytes);
    }
}
