namespace DandyDotnet.Persistence.Sql.Migrations.Postgres.Tests.Mocks;

internal static class MigrationExecutionRecorder
{
    private static readonly AsyncLocal<ExecutionLog?> CurrentLog = new();

    public static void Reset()
    {
        CurrentLog.Value = new ExecutionLog();
    }

    public static long[] GetUpVersions()
    {
        return GetLog().UpVersions.ToArray();
    }

    public static long[] GetDownVersions()
    {
        return GetLog().DownVersions.ToArray();
    }

    public static void RecordUp(long version)
    {
        GetLog().UpVersions.Add(version);
    }

    public static void RecordDown(long version)
    {
        GetLog().DownVersions.Add(version);
    }

    private static ExecutionLog GetLog()
    {
        return CurrentLog.Value ??= new ExecutionLog();
    }

    private sealed class ExecutionLog
    {
        public List<long> UpVersions { get; } = [];
        public List<long> DownVersions { get; } = [];
    }
}
