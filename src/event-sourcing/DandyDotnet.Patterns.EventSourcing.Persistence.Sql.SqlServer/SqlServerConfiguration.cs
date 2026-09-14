using System.Reflection;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Abstractions;
using DandyDotnet.Persistence.Sql.Migrations;
using DandyDotnet.Persistence.Sql.Migrations.SqlServer;
using DandyDotnet.Persistence.Sql.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer;

public sealed class SqlServerConfiguration : PersistenceConfiguration
{
    private static readonly Assembly[] _assemblies = [typeof(SqlServerConfiguration).Assembly];

    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddDandyMigrations(configuration =>
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