using DandyDotnet.Encoding.Codes;

namespace DandyDotnet.Encoding.Configuration;

/// <summary>
/// Provides payload encoder selection extensions.
/// </summary>
public static class EncodingConfigurationBuilderExtensions
{
    /// <summary>
    /// Selects the ASCII payload encoder.
    /// </summary>
    /// <param name="builder">The encoding configuration builder.</param>
    /// <returns>The updated <paramref name="builder"/>.</returns>
    public static EncodingConfigurationBuilder UseAsciiPayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(AsciiEncoder));
    }

    /// <summary>
    /// Selects the big-endian Unicode payload encoder.
    /// </summary>
    /// <param name="builder">The encoding configuration builder.</param>
    /// <returns>The updated <paramref name="builder"/>.</returns>
    public static EncodingConfigurationBuilder UseBigEndianUnicodePayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(BigEndianUnicodeEncoder));
    }

    /// <summary>
    /// Selects the ISO-8859-1 payload encoder.
    /// </summary>
    /// <param name="builder">The encoding configuration builder.</param>
    /// <returns>The updated <paramref name="builder"/>.</returns>
    public static EncodingConfigurationBuilder UseLatin1PayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(Latin1Encoder));
    }

    /// <summary>
    /// Selects the Unicode payload encoder.
    /// </summary>
    /// <param name="builder">The encoding configuration builder.</param>
    /// <returns>The updated <paramref name="builder"/>.</returns>
    public static EncodingConfigurationBuilder UseUnicodePayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(UnicodeEncoder));
    }

    /// <summary>
    /// Selects the UTF-8 payload encoder.
    /// </summary>
    /// <param name="builder">The encoding configuration builder.</param>
    /// <returns>The updated <paramref name="builder"/>.</returns>
    public static EncodingConfigurationBuilder UseUtf8PayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(Utf8Encoder));
    }

    /// <summary>
    /// Selects the UTF-32 payload encoder.
    /// </summary>
    /// <param name="builder">The encoding configuration builder.</param>
    /// <returns>The updated <paramref name="builder"/>.</returns>
    public static EncodingConfigurationBuilder UseUtf32PayloadEncoder(this EncodingConfigurationBuilder builder)
    {
        return builder.UseEncoder(typeof(Utf32Encoder));
    }
}
