using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Configuration;

/// <summary>
///     Provides extension methods for selecting specific payload encoder implementations on
///     <see cref="EncodingConfigurationBuilder"/>.
/// </summary>
/// <remarks>
///     <para>
///         These extension methods provide convenient, type-safe ways to configure specific encoder implementations
///         without having to directly reference the encoder types. Each method configures the builder to use a
///         particular encoding scheme.
///     </para>
/// </remarks>
public static class EncodingConfigurationBuilderExtensions
{
    /// <summary>
    ///     Selects the ASCII payload encoder for the configuration.
    /// </summary>
    /// <param name="builder">The encoding configuration builder to configure.</param>
    /// <returns>The updated <paramref name="builder"/> for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         ASCII encoding is a 7-bit encoding that supports characters in the range 0-127.
    ///         It is suitable for basic Latin text but does not support Unicode characters outside this range.
    ///     </para>
    ///     <para>
    ///         See <see cref="AsciiEncoder"/> for more information about the ASCII encoder implementation.
    ///     </para>
    /// </remarks>
    public static EncodingConfigurationBuilder UseAsciiPayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(AsciiEncoder));
    }

    /// <summary>
    ///     Selects the big-endian Unicode (UTF-16) payload encoder for the configuration.
    /// </summary>
    /// <param name="builder">The encoding configuration builder to configure.</param>
    /// <returns>The updated <paramref name="builder"/> for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         Big-endian UTF-16 encoding uses 16 bits per character (2 bytes) for most Unicode characters,
    ///         with the most significant byte stored first. It supports all Unicode characters but uses more space
    ///         than UTF-8 for ASCII and Western European text.
    ///     </para>
    ///     <para>
    ///         See <see cref="BigEndianUnicodeEncoder"/> for more information about the big-endian UTF-16 encoder implementation.
    ///     </para>
    /// </remarks>
    public static EncodingConfigurationBuilder UseBigEndianUnicodePayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(BigEndianUnicodeEncoder));
    }

    /// <summary>
    ///     Selects the ISO-8859-1 (Latin-1) payload encoder for the configuration.
    /// </summary>
    /// <param name="builder">The encoding configuration builder to configure.</param>
    /// <returns>The updated <paramref name="builder"/> for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         ISO-8859-1 encoding is an 8-bit encoding that supports characters used in Western European languages.
    ///         It provides a good balance between simplicity and support for accented characters but cannot
    ///         represent all Unicode characters.
    ///     </para>
    ///     <para>
    ///         See <see cref="Latin1Encoder"/> for more information about the ISO-8859-1 encoder implementation.
    ///     </para>
    /// </remarks>
    public static EncodingConfigurationBuilder UseLatin1PayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(Latin1Encoder));
    }

    /// <summary>
    ///     Selects the Unicode (UTF-16) payload encoder with system default byte order for the configuration.
    /// </summary>
    /// <param name="builder">The encoding configuration builder to configure.</param>
    /// <returns>The updated <paramref name="builder"/> for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         UTF-16 encoding uses 16 bits per character (2 bytes) for most Unicode characters.
    ///         The byte order (endianness) is determined by the system's native byte order. It supports all
    ///         Unicode characters but uses more space than UTF-8 for ASCII and Western European text.
    ///     </para>
    ///     <para>
    ///         See <see cref="UnicodeEncoder"/> for more information about the UTF-16 encoder implementation.
    ///     </para>
    /// </remarks>
    public static EncodingConfigurationBuilder UseUnicodePayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(UnicodeEncoder));
    }

    /// <summary>
    ///     Selects the UTF-8 payload encoder for the configuration.
    /// </summary>
    /// <param name="builder">The encoding configuration builder to configure.</param>
    /// <returns>The updated <paramref name="builder"/> for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         UTF-8 is a variable-width encoding that uses between 1 and 4 bytes per character.
    ///         It is space-efficient for ASCII and Western European text while supporting all Unicode characters.
    ///         UTF-8 is the default encoding for many web standards and is recommended for most applications.
    ///     </para>
    ///     <para>
    ///         See <see cref="Utf8Encoder"/> for more information about the UTF-8 encoder implementation.
    ///     </para>
    /// </remarks>
    public static EncodingConfigurationBuilder UseUtf8PayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(Utf8Encoder));
    }

    /// <summary>
    ///     Selects the UTF-32 payload encoder for the configuration.
    /// </summary>
    /// <param name="builder">The encoding configuration builder to configure.</param>
    /// <returns>The updated <paramref name="builder"/> for method chaining.</returns>
    /// <remarks>
    ///     <para>
    ///         UTF-32 encoding uses exactly 4 bytes per character for all Unicode characters.
    ///         This fixed-width encoding simplifies string manipulation and random access to characters
    ///         but uses more space than variable-width encodings like UTF-8 or UTF-16.
    ///     </para>
    ///     <para>
    ///         See <see cref="Utf32Encoder"/> for more information about the UTF-32 encoder implementation.
    ///     </para>
    /// </remarks>
    public static EncodingConfigurationBuilder UseUtf32PayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(Utf32Encoder));
    }
}
