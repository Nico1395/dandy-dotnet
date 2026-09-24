using DandyDotnet.Serialization.Abstractions;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Serialization.SystemTextJson.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSerialization(cfg => cfg.UseSystemTextJson());
    }

    public ISerializer GetSerializer()
    {
        return ServiceProvider.GetRequiredService<ISerializer>();
    }
}