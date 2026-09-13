using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.Postgres;

internal sealed class PostgresMigrationsDriver : MigrationsDriver
{
    public override void ConfigureServices(IServiceCollection services, MigrationsConfiguration configuration)
    {
        if (configuration.ServiceKey == null)
        {
            services.AddSingleton<IDbConnectionFactory, PostgresDbConnectionFactory>();
            services.AddSingleton<MigrationsSqlStrings, PostgresMigrationsSqlStrings>();
        }
        else
        {
            services.AddKeyedSingleton<IDbConnectionFactory, PostgresDbConnectionFactory>(configuration.ServiceKey);
            services.AddKeyedSingleton<MigrationsSqlStrings, PostgresMigrationsSqlStrings>(configuration.ServiceKey);
        }
    }
}