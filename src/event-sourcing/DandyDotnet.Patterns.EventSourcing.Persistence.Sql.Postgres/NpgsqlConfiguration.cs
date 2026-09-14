using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Postgres;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres;

internal sealed class NpgsqlConfiguration : PersistenceConfiguration
{
    private static readonly Assembly[]? _assemblies = [typeof(NpgsqlConfiguration).Assembly];

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.ConfigureRunner(runner =>
        {
            runner.AddPostgres()
                .WithGlobalConnectionString(ConnectionString)
                .ScanIn(_assemblies).For.Migrations();
        });

        services.AddKeyedSingleton<IDbConnectionFactory>(EventSourcingConstants.ServiceKey, new PostgresDbConnectionFactory(ConnectionString));
        services.AddSingleton<SqlStrings, NpgsqlSqlStrings>();
        services.AddSingleton(this);
    }
}