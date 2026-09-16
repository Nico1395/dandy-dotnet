using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres.Tests.Mocks;

[Event]
public sealed record TestEvent(Guid Id, string Value);

[Event]
public sealed record SecondTestEvent(Guid Id, int Amount);

[Event]
public sealed record ExpiringEvent(Guid Id);

[Event]
public sealed record InlineEvent(Guid Id);

[Event]
public sealed record AsyncEvent(Guid Id);

[Event]
public sealed record FailingEvent(Guid Id);

[Aggregate(SnapshotInterval = 3)]
public sealed class TestAggregate
{
    public required Guid Id { get; init; }
    public string Value { get; set; } = string.Empty;
    public int Amount { get; set; }
    public long Version { get; set; }

    [AggregateFactory]
    public static TestAggregate Create(TestAggregate? snapshot, IReadOnlyEnvelope[] envelopes)
    {
        var aggregate = snapshot ?? new TestAggregate
        {
            Id = envelopes.Select(e => e.Event).OfType<TestEvent>().First().Id,
        };

        foreach (var envelope in envelopes)
        {
            switch (envelope.Event)
            {
                case TestEvent e:
                    aggregate.Value = e.Value;
                    break;
                case SecondTestEvent e:
                    aggregate.Amount += e.Amount;
                    break;
            }

            aggregate.Version = envelope.Version;
        }

        return aggregate;
    }
}

