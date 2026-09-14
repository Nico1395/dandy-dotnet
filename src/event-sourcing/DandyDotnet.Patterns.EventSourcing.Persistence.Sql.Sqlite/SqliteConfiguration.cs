using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Sqlite;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite;

internal sealed class SqliteConfiguration : PersistenceConfiguration
{
    private static readonly Assembly[]? _assemblies = [typeof(SqliteConfiguration).Assembly];

    public override string Slot => "persistence";

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.ConfigureRunner(runner =>
        {
            runner.AddSQLite()
                .WithGlobalConnectionString(ConnectionString)
                .ScanIn(_assemblies).For.Migrations();
        });

        services.AddKeyedSingleton<IDbConnectionFactory>(EventSourcingConstants.ServiceKey, new SqliteDbConnectionFactory(ConnectionString));
        services.AddSingleton<SqlStrings, SqliteSqlStrings>();
        services.AddSingleton(this);
    }
}