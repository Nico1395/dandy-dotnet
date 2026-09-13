using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Persistence.Sql.Migrations.SqlServer;

internal sealed class SqlServerMigrationsDriver : MigrationsDriver
{
    public override void ConfigureServices(IServiceCollection services, MigrationsConfiguration configuration)
    {
        if (configuration.ServiceKey == null)
        {
            services.AddSingleton<IDbConnectionFactory>(new SqlServerDbConnectionFactory(ConnectionString));
            services.AddSingleton<MigrationsSqlStrings>(new SqlServerMigrationsSqlStrings(configuration));
        }
        else
        {
            services.AddKeyedSingleton<IDbConnectionFactory>(configuration.ServiceKey, new SqlServerDbConnectionFactory(ConnectionString));
            services.AddKeyedSingleton<MigrationsSqlStrings>(configuration.ServiceKey, new SqlServerMigrationsSqlStrings(configuration));
        }
    }
}