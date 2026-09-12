using System.Data;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Connections;
using Npgsql;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres;

internal sealed class NpgsqlDbConnectionFactory(NpgsqlConfiguration configuration) : IDbConnectionFactory
{
    public IDbConnection Create()
    {
        return new NpgsqlConnection(configuration.ConnectionString);
    }
}