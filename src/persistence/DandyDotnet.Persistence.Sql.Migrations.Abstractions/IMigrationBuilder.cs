using System.Data;

namespace DandyDotnet.Persistence.Sql.Migrations.Abstractions;

public interface IMigrationBuilder
{
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; }
}