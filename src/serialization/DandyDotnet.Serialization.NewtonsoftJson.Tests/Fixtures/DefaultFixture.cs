using DandyDotnet.Serialization.Abstractions;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Serialization.NewtonsoftJson.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSerializer(cfg => cfg.UseNewtonsoftJson());
    }

    public ISerializer GetSerializer()
    {
        return ServiceProvider.GetRequiredService<ISerializer>();
    }
}