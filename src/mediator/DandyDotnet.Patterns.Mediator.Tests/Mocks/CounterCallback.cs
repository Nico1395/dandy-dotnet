namespace DandyDotnet.Patterns.Mediator.Tests.Mocks;

internal sealed class CounterCallback
{
    public int Successes { get; private set; }

    public void Success()
    {
        Successes++;
    }
}