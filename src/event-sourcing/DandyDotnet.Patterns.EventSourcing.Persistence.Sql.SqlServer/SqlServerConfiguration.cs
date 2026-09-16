using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations;
using DandyDotnet.Persistence.Sql.Migrations.SqlServer;
using DandyDotnet.Persistence.Sql.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer;

/// <summary>
///     Represents the configuration and service registration provider for SQL Server event persistence.
/// </summary>
public sealed class SqlServerConfiguration : PersistenceConfiguration
{
    private static readonly Assembly[] _assemblies = [typeof(SqlServerConfiguration).Assembly];

    /// <inheritdoc />
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddMigrations(configuration =>
        {
            configuration
                .UseSqlServer(ConnectionString)
                .ScanInAssemblies(_assemblies);
            configuration.Schema = Sql.Constants.Schema.Name;
            configuration.ServiceKey = EventSourcingConstants.ServiceKey;
        });

        services.AddKeyedSingleton<IDbConnectionFactory>(EventSourcingConstants.ServiceKey, new SqlServerDbConnectionFactory(ConnectionString));
        services.AddSingleton<SqlStrings, SqlServerSqlStrings>();
        services.AddSingleton(this);
    }
}