namespace DandyDotnet.Persistence.Sql.Migrations;

public static class MigrationsConstants
{
    public static class Tables
    {
        public static class Migrations
        {
            public const string TableName = "__migrations";
            public const string Version = "version";
            public const string AppliedAt = "applied_at";
        }
    }
}