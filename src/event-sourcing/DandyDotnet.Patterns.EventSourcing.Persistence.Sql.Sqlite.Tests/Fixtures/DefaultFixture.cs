using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    protected override void ConfigureServices(IServiceCollection services)
    {
    }
}