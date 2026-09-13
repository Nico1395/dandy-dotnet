using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.Sqlite;

internal sealed class SqliteMigrationsDriver : MigrationsDriver
{
    public override void ConfigureServices(IServiceCollection services, MigrationsConfiguration configuration)
    {
        if (configuration.ServiceKey == null)
        {
            services.AddSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>();
            services.AddSingleton<MigrationsSqlStrings, SqliteMigrationsSqlStrings>();
        }
        else
        {
            services.AddKeyedSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>(configuration.ServiceKey);
            services.AddKeyedSingleton<MigrationsSqlStrings, SqliteMigrationsSqlStrings>(configuration.ServiceKey);
        }
    }
}