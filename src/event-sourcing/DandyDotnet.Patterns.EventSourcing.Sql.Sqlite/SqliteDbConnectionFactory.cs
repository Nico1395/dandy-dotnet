using System.Data;
using DandyDotnet.Patterns.EventSourcing.Sql.Connections;
using Microsoft.Data.Sqlite;

namespace DandyDotnet.Patterns.EventSourcing.Sql.Sqlite;

internal sealed class SqliteDbConnectionFactory(SqliteConfiguration configuration) : IDbConnectionFactory
{
    public IDbConnection Create()
    {
        return new SqliteConnection(configuration.ConnectionString);
    }
}