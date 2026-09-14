using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.SqlServer;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer;

public sealed class SqlServerConfiguration : PersistenceConfiguration
{
    private static readonly Assembly[]? _assemblies = [typeof(SqlServerConfiguration).Assembly];

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.ConfigureRunner(runner =>
        {
            runner.AddSqlServer()
                .WithGlobalConnectionString(ConnectionString)
                .ScanIn(_assemblies).For.Migrations();
        });

        services.AddKeyedSingleton<IDbConnectionFactory>(EventSourcingConstants.ServiceKey, new SqlServerDbConnectionFactory(ConnectionString));
        services.AddSingleton<SqlStrings, SqlServerSqlStrings>();
        services.AddSingleton(this);
    }
}