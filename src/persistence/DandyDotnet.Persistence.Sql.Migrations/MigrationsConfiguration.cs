using System.Reflection;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Persistence.Sql.Migrations;

public sealed class MigrationsConfiguration
{
    public object? ServiceKey { get; set; }
    public List<Assembly> Assemblies { get; set; } = [];
    public List<Type> MigrationTypes { get; set; } = [];
    public string? Schema { get; set; }
    public string Table { get; set; } = MigrationsConstants.Tables.Migrations.TableName;
    public MigrationsDriver? Driver { get; set; }
    public Action<IServiceProvider, Exception>? OnExceptionDuringOpeningConnection { get; set; }
    public Action<IServiceProvider, Exception>? OnExceptionDuringMigrateUp { get; set; }
    public Action<IServiceProvider, Exception>? OnExceptionDuringMigrateDown { get; set; }
    public Action<IServiceProvider, Exception>? OnExceptionDuringCreatingSchemaAndTable { get; set; }

    public MigrationsConfiguration AddMigration<TMigration>()
        where TMigration : class, IMigration
    {
        MigrationTypes.Add(typeof(TMigration));
        return this;
    }

    public MigrationsConfiguration ScanInAssemblies(params Assembly[] assemblies)
    {
        Assemblies.AddRange(assemblies);
        Assemblies = Assemblies.Distinct().ToList();

        return this;
    }
}