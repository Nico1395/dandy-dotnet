using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using Npgsql;

namespace DandyDotnet.Persistence.Sql.Postgres;

public sealed class PostgresDbConnectionFactory : IDbConnectionFactory
{
    public IDbConnection Create(string? connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return new NpgsqlConnection(connectionString);
    }
}