using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using Microsoft.Data.Sqlite;

namespace DandyDotnet.Persistence.Sql.Sqlite;

public sealed class SqliteDbConnectionFactory : IDbConnectionFactory
{
    public IDbConnection Create(string? connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return new SqliteConnection(connectionString);
    }
}