namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Mocks;

internal static class MigrationExecutionRecorder
{
    private static readonly List<long> ExecutedVersions = [];

    public static void Reset()
    {
        lock (ExecutedVersions)
            ExecutedVersions.Clear();
    }

    public static long[] GetExecutedVersions()
    {
        lock (ExecutedVersions)
            return ExecutedVersions.ToArray();
    }

    public static void Record(long version)
    {
        lock (ExecutedVersions)
            ExecutedVersions.Add(version);
    }
}
