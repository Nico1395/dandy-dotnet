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
            services.AddSingleton<IDbConnectionFactory>(new SqliteDbConnectionFactory(ConnectionString));
            services.AddSingleton<MigrationsSqlStrings, SqliteMigrationsSqlStrings>();
        }
        else
        {
            services.AddKeyedSingleton<IDbConnectionFactory>(configuration.ServiceKey, new SqliteDbConnectionFactory(ConnectionString));
            services.AddKeyedSingleton<MigrationsSqlStrings, SqliteMigrationsSqlStrings>(configuration.ServiceKey);
        }
    }
}