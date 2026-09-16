namespace DandyDotnet.Encoding.Abstractions;

/// <summary>
///     Defines the contract for encoding and decoding string payloads to and from byte sequences.
/// </summary>
/// <remarks>
///     <para>
///         Implementations of this interface provide specific encoding schemes (such as UTF-8, UTF-16, ASCII, etc.)
///         for converting between strings and their byte representations. This abstraction allows for flexible
///         encoding strategies that can be selected based on application requirements.
///     </para>
///     <para>
///         The <see cref="Encode"/> method converts a string to its byte representation, while the <see cref="Decode"/>
///         method performs the reverse operation, converting bytes back to a string.
///     </para>
/// </remarks>
public interface IEncoder
{
    /// <summary>
    ///     Encodes a string payload into a sequence of bytes.
    /// </summary>
    /// <param name="payload">The string to encode.</param>
    /// <returns>A <see cref="ReadOnlyMemory{T}"/> of bytes representing the encoded string.</returns>
    /// <remarks>
    ///     <para>
    ///         The encoding process uses a specific character encoding scheme (such as UTF-8, UTF-16, ASCII, etc.)
    ///         to convert the string into its byte representation. The specific encoding used depends on the
    ///         implementation.
    ///     </para>
    /// </remarks>
    ReadOnlyMemory<byte> Encode(string payload);

    /// <summary>
    ///     Decodes a sequence of bytes into a string.
    /// </summary>
    /// <param name="bytes">The byte sequence to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <remarks>
    ///     <para>
    ///         The decoding process interprets the byte sequence using a specific character encoding scheme
    ///         (such as UTF-8, UTF-16, ASCII, etc.) to produce the original string. The specific encoding used
    ///         depends on the implementation and should match the encoding used for the corresponding
    ///         <see cref="Encode"/> call.
    ///     </para>
    ///     <para>
    ///         If the byte sequence is not valid for the encoding, the behavior is implementation-specific.
    ///         Some implementations may throw an exception, while others may replace invalid sequences with
    ///         replacement characters.
    ///     </para>
    /// </remarks>
    string Decode(ReadOnlySpan<byte> bytes);
}