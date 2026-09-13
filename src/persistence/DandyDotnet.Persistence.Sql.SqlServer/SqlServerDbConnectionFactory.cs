using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using Microsoft.Data.SqlClient;

namespace DandyDotnet.Persistence.Sql.SqlServer;

public sealed class SqlServerDbConnectionFactory : IDbConnectionFactory
{
    public IDbConnection Create(string? connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return new SqlConnection(connectionString);
    }
}