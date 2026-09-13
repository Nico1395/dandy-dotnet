using System.Data;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Connections;

public static class DbConnectionFactoryExtensions
{
    public static IDbConnection CreateAndOpen(this IDbConnectionFactory factory)
    {
        var connection = factory.Create();

        connection.Open();

        return connection;
    }
}