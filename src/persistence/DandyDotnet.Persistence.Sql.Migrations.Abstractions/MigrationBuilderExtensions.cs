using Dapper;

namespace DandyDotnet.Persistence.Sql.Migrations.Abstractions;

public static class MigrationBuilderExtensions
{
    public static void Execute(this IMigrationBuilder builder, string sql, object? parameters = null)
    {
        var command = new CommandDefinition(sql, parameters, transaction: builder.Transaction);
        builder.Connection.Execute(command);
    }
}