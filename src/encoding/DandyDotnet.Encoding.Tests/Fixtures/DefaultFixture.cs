using DandyDotnet.Encoding.Configuration;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Encoding.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddDandyEncoder(cfg => cfg.UseUtf8PayloadEncoder());
    }
}