using System.Data;

namespace DandyDotnet.Patterns.EventSourcing.Sql.Connections;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}