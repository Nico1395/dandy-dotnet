using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using Microsoft.Data.Sqlite;

namespace DandyDotnet.Persistence.Sql.Sqlite;

public sealed class SqliteDbConnectionFactory(string? connectionString) : IDbConnectionFactory
{
    public IDbConnection Create()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return new SqliteConnection(connectionString);
    }
}