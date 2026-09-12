using System.Data;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Connections;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}