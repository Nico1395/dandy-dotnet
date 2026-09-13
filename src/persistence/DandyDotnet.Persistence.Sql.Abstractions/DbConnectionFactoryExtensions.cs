using System.Data;

namespace DandyDotnet.Persistence.Sql.Abstractions;

public static class DbConnectionFactoryExtensions
{
    public static IDbConnection CreateAndOpen(this IDbConnectionFactory factory, string? connectionString)
    {
        var connection = factory.Create(connectionString);
        connection.Open();
        return connection;
    }
}