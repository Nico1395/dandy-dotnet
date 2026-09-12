using System.Data;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

public interface IReadOnlyUnitOfWorkContext
{
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }
    bool Completed { get; }
}