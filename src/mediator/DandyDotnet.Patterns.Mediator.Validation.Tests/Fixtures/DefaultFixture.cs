using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Mediator.Validation.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddMediator(config => config
            .ScanInAssemblies(typeof(DefaultFixture).Assembly)
            .UseValidation());
    }

    public IMediator GetMediator()
    {
        return ServiceProvider.GetRequiredService<IMediator>();
    }
}