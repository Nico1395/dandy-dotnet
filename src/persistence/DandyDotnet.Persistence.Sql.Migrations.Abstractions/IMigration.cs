namespace DandyDotnet.Persistence.Sql.Migrations.Abstractions;

public interface IMigration
{
    long Version { get; }
    void Up(IMigrationBuilder builder);
    void Down(IMigrationBuilder builder);
}