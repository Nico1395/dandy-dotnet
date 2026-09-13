using System.Data;

namespace DandyDotnet.Persistence.Sql.Abstractions;

public interface IDbConnectionFactory
{
    IDbConnection Create(string? connectionString);
}