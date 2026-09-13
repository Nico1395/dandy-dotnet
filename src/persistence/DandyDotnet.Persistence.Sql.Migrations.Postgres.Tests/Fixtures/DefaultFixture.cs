using DandyDotnet.Tests.Core.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Fixtures;

public sealed class DefaultFixture : Fixture
{
    private PostgreSqlContainer? _postgres;

    public string ConnectionString => _postgres?.GetConnectionString() ?? throw new InvalidOperationException("The PostgreSQL container has not been started.");

    protected override void ConfigureServices(IServiceCollection services)
    {
        try
        {
            _postgres = new PostgreSqlBuilder()
                .WithImage("postgres:16-alpine")
                .WithDatabase("dandy_dotnet_tests")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            _postgres.StartAsync().GetAwaiter().GetResult();

            services.AddDandyMigrations(configuration =>
            {
                configuration.UsePostgres();
                configuration.Driver!.ConnectionString = ConnectionString;
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public override async Task DisposeAsync()
    {
        ServiceProvider.Dispose();

        if (_postgres is not null)
            await _postgres.DisposeAsync();
    }
}