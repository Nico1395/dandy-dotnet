using DandyDotnet.Encoding.Codes;
using DandyDotnet.Encoding.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Encoding.Tests.Configuration;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDandyEncoder_RegistersConfiguredEncoder()
    {
        var services = new ServiceCollection();

        services.AddDandyEncoder(cfg => cfg.UseUtf8PayloadEncoder());

        using var serviceProvider = services.BuildServiceProvider();
        var encoder = serviceProvider.GetRequiredService<IEncoder>();

        Assert.IsType<Utf8Encoder>(encoder);
    }

    [Fact]
    public void AddDandyEncoder_DoesNotOverwritePreviousEncoderRegistration()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IEncoder, AsciiEncoder>();

        services.AddDandyEncoder(cfg => cfg.UseUtf8PayloadEncoder());

        using var serviceProvider = services.BuildServiceProvider();
        var encoders = serviceProvider.GetServices<IEncoder>().ToArray();

        var encoder = Assert.Single(encoders);
        Assert.IsType<AsciiEncoder>(encoder);
    }
}
