using System.Reflection;

namespace DandyDotnet.Persistence.Sql.Migrations;

public sealed class MigrationsConfiguration
{
    public object? ServiceKey { get; set; }
    public Assembly[]? Assemblies { get; set; }
    public string? ConnectionString { get; set; }
    public string MigrationsTableName { get; set; } = MigrationsConstants.Tables.Migrations.TableName;
    public MigrationsDriver? Driver { get; set; }
    public Action<IServiceProvider, Exception>? OnExceptionDuringOpeningConnection { get; set; }
    public Action<IServiceProvider, Exception>? OnExceptionDuringMigrateUp { get; set; }
    public Action<IServiceProvider, Exception>? OnExceptionDuringMigrateDown { get; set; }
}