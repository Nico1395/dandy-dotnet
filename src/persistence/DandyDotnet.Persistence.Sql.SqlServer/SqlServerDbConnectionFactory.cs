using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using Microsoft.Data.SqlClient;

namespace DandyDotnet.Persistence.Sql.SqlServer;

public sealed class SqlServerDbConnectionFactory(string? connectionString) : IDbConnectionFactory
{
    public IDbConnection Create()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return new SqlConnection(connectionString);
    }
}