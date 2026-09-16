using Dapper;

namespace DandyDotnet.Persistence.Sql.Migrations.Abstractions;

/// <summary>
///     Extension methods for <see cref="IMigrationBuilder" /> to simplify SQL command execution.
/// </summary>
public static class MigrationBuilderExtensions
{
    /// <summary>
    ///     Executes a SQL command against the database using the migration builder's connection and transaction.
    /// </summary>
    /// <param name="builder">The migration builder providing the connection and transaction.</param>
    /// <param name="sql">The SQL command text to execute.</param>
    /// <param name="parameters">Optional parameters for the SQL command.</param>
    /// <exception cref="System.Data.Common.DbException">
    ///     Thrown when an error occurs during SQL command execution.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method executes the specified SQL command using the <see cref="IMigrationBuilder.Connection" />
    ///         and <see cref="IMigrationBuilder.Transaction" /> from the provided builder.
    ///     </para>
    ///     <para>
    ///         The command is executed within the context of the current transaction, ensuring that all
    ///         migration operations are atomic.
    ///     </para>
    ///     <para>
    ///         Use this method in the <see cref="IMigration.Up" /> and <see cref="IMigration.Down" /> methods to
    ///         execute SQL commands for your migrations.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    ///         public class MyMigration : IMigration
    ///         {
    ///             public long Version => 1;
    ///             
    ///             public void Up(IMigrationBuilder builder)
    ///             {
    ///                 builder.Execute("CREATE TABLE Users (Id INT PRIMARY KEY, Name NVARCHAR(100))");
    ///             }
    ///             
    ///             public void Down(IMigrationBuilder builder)
    ///             {
    ///                 builder.Execute("DROP TABLE Users");
    ///             }
    ///         }
    ///     </code>
    /// </example>
    public static void Execute(this IMigrationBuilder builder, string sql, object? parameters = null)
    {
        var command = new CommandDefinition(sql, parameters, transaction: builder.Transaction);
        builder.Connection.Execute(command);
    }
}