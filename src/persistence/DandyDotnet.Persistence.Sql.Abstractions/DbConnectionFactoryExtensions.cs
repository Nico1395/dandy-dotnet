using System.Data;

namespace DandyDotnet.Persistence.Sql.Abstractions;

public static class DbConnectionFactoryExtensions
{
    public static IDbConnection CreateAndOpen(this IDbConnectionFactory factory)
    {
        var connection = factory.Create();
        connection.Open();
        return connection;
    }
}