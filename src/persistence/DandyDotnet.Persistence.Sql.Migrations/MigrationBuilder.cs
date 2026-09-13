using System.Data;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;

namespace DandyDotnet.Persistence.Sql.Migrations;

internal sealed class MigrationBuilder(IDbConnection connection, IDbTransaction transaction) : IMigrationBuilder
{
    public IDbConnection Connection => connection;
    public IDbTransaction Transaction => transaction;
}