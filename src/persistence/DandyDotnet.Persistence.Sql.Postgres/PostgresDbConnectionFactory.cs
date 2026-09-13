using System.Data;
using DandyDotnet.Persistence.Sql.Abstractions;
using Npgsql;

namespace DandyDotnet.Persistence.Sql.Postgres;

public sealed class PostgresDbConnectionFactory(string? connectionString) : IDbConnectionFactory
{
    public IDbConnection Create()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        return new NpgsqlConnection(connectionString);
    }
}