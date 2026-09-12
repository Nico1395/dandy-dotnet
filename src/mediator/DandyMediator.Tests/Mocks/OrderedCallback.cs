namespace DandyMediator.Tests.Mocks;

internal sealed class OrderedCallback
{
    private readonly List<(DateTime SucceededAt, string HandlerName)> _successes = new();

    public IReadOnlyList<(DateTime SucceededAt, string HandlerName)> Successes => _successes;

    public void Success(object location)
    {
        _successes.Add((DateTime.UtcNow, location.GetType().Name));
    }

    public bool SucceededInExpectedOrder(params string[] handlers)
    {
        return _successes
            .OrderBy(s => s.SucceededAt)
            .Select(s => s.HandlerName)
            .SequenceEqual(handlers);
    }
}