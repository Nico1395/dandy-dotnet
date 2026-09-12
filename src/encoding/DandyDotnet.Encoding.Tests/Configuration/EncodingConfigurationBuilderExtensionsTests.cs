using DandyDotnet.Encoding.Codes;
using DandyDotnet.Encoding.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Encoding.Tests.Configuration;

public class EncodingConfigurationBuilderExtensionsTests
{
    [Fact]
    public void UseAsciiPayloadEncoder_RegistersAsciiEncoder()
    {
        AssertEncoder<AsciiEncoder>(builder => builder.UseAsciiPayloadEncoder());
    }

    [Fact]
    public void UseBigEndianUnicodePayloadEncoder_RegistersBigEndianUnicodeEncoder()
    {
        AssertEncoder<BigEndianUnicodeEncoder>(builder => builder.UseBigEndianUnicodePayloadEncoder());
    }

    [Fact]
    public void UseLatin1PayloadEncoder_RegistersLatin1Encoder()
    {
        AssertEncoder<Latin1Encoder>(builder => builder.UseLatin1PayloadEncoder());
    }

    [Fact]
    public void UseUnicodePayloadEncoder_RegistersUnicodeEncoder()
    {
        AssertEncoder<UnicodeEncoder>(builder => builder.UseUnicodePayloadEncoder());
    }

    [Fact]
    public void UseUtf8PayloadEncoder_RegistersUtf8Encoder()
    {
        AssertEncoder<Utf8Encoder>(builder => builder.UseUtf8PayloadEncoder());
    }

    [Fact]
    public void UseUtf32PayloadEncoder_RegistersUtf32Encoder()
    {
        AssertEncoder<Utf32Encoder>(builder => builder.UseUtf32PayloadEncoder());
    }

    private static void AssertEncoder<TEncoder>(Action<EncodingConfigurationBuilder> configure)
        where TEncoder : IEncoder
    {
        var services = new ServiceCollection();
        services.AddDandyEncoder(configure);

        using var serviceProvider = services.BuildServiceProvider();
        var encoder = serviceProvider.GetRequiredService<IEncoder>();

        Assert.IsType<TEncoder>(encoder);
    }
}
