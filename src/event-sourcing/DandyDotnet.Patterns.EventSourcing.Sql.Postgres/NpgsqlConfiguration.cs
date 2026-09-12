using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Sql.Connections;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Sql.Postgres;

internal sealed class NpgsqlConfiguration : PersistenceConfiguration
{
    private static readonly Assembly[]? _assemblies = [typeof(NpgsqlConfiguration).Assembly];

    public override string Slot => "persistence";

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.ConfigureRunner(runner =>
        {
            runner.AddPostgres()
                .WithGlobalConnectionString(ConnectionString)
                .ScanIn(_assemblies).For.Migrations();
        });

        services.AddSingleton<IDbConnectionFactory, NpgsqlDbConnectionFactory>();
        services.AddSingleton<SqlStrings, NpgsqlSqlStrings>();
        services.AddSingleton(this);
    }
}