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
            services.AddSingleton<IDbConnectionFactory>(new PostgresDbConnectionFactory(ConnectionString));
            services.AddSingleton<MigrationsSqlStrings, PostgresMigrationsSqlStrings>();
        }
        else
        {
            services.AddKeyedSingleton<IDbConnectionFactory>(configuration.ServiceKey, new PostgresDbConnectionFactory(ConnectionString));
            services.AddKeyedSingleton<MigrationsSqlStrings, PostgresMigrationsSqlStrings>(configuration.ServiceKey);
        }
    }
}