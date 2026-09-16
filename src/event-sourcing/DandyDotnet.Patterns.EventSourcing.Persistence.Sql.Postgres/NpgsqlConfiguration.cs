using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations;
using DandyDotnet.Persistence.Sql.Migrations.Postgres;
using DandyDotnet.Persistence.Sql.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres;

/// <summary>
///     Represents the configuration and service registration provider for PostgreSQL event persistence.
/// </summary>
internal sealed class NpgsqlConfiguration : PersistenceConfiguration
{
    private static readonly Assembly[] _assemblies = [typeof(NpgsqlConfiguration).Assembly];

    /// <inheritdoc />
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddMigrations(configuration =>
        {
            configuration
                .UsePostgres(ConnectionString)
                .ScanInAssemblies(_assemblies);
            configuration.Schema = Sql.Constants.Schema.Name;
            configuration.ServiceKey = EventSourcingConstants.ServiceKey;
        });

        services.AddKeyedSingleton<IDbConnectionFactory>(EventSourcingConstants.ServiceKey, new PostgresDbConnectionFactory(ConnectionString));
        services.AddSingleton<SqlStrings, NpgsqlSqlStrings>();
        services.AddSingleton(this);
    }
}