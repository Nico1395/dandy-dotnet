using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations;
using DandyDotnet.Persistence.Sql.Migrations.Sqlite;
using DandyDotnet.Persistence.Sql.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite;

internal sealed class SqliteConfiguration : PersistenceConfiguration
{
    private static readonly Assembly[] _assemblies = [typeof(SqliteConfiguration).Assembly];

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddMigrations(configuration =>
        {
            configuration
                .UseSqlite(ConnectionString)
                .ScanInAssemblies(_assemblies);
            configuration.ServiceKey = EventSourcingConstants.ServiceKey;
        });

        services.AddKeyedSingleton<IDbConnectionFactory>(EventSourcingConstants.ServiceKey, new SqliteDbConnectionFactory(ConnectionString));
        services.AddSingleton<SqlStrings, SqliteSqlStrings>();
        services.AddSingleton(this);
    }
}