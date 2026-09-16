using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Tests.Fixtures;

public sealed class IntegrationFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
    }
}