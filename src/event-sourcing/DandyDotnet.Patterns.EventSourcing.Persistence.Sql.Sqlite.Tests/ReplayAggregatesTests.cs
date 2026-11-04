using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests.Fixtures;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests;

public sealed class ReplayAggregatesTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture _fixture;

    public ReplayAggregatesTests(DefaultFixture fixture)
    {
        _fixture = fixture;
        fixture.ResetDatabase();
    }

    [Fact]
    public async Task ReplayAggregates_WithMixedSnapshotsAndMissingStream_ShouldReplayEachExistingStream()
    {
        var withTail = Guid.NewGuid();
        var snapshotOnly = Guid.NewGuid();
        var withoutSnapshot = Guid.NewGuid();
        var missing = Guid.NewGuid().ToString();
        using var scope = _fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();

        foreach (var id in new[] { withTail, snapshotOnly })
        {
            await store.AppendAsync<TestAggregate>(id.ToString(), [
                new TestEvent(id, "snapshot"), new SecondTestEvent(id, 2),
                new SecondTestEvent(id, 3), new SecondTestEvent(id, 4),
            ], CancellationToken.None);
            Assert.NotNull(await store.GetLastSnapshotAsync(id.ToString(), null, CancellationToken.None));
        }

        await store.AppendAsync<TestAggregate>(withTail.ToString(), new SecondTestEvent(withTail, 5), CancellationToken.None);
        await store.AppendAsync<TestAggregate>(withoutSnapshot.ToString(), [
            new TestEvent(withoutSnapshot, "first"), new TestEvent(withoutSnapshot, "last"),
            new SecondTestEvent(withoutSnapshot, 7),
        ], CancellationToken.None);
        Assert.Null(await store.GetLastSnapshotAsync(withoutSnapshot.ToString(), null, CancellationToken.None));

        var result = await store.ReplayAggregatesAsync(typeof(TestAggregate),
            [withoutSnapshot.ToString(), missing, withTail.ToString(), snapshotOnly.ToString()], CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.False(result.ContainsKey(missing));
        var tail = Assert.IsType<TestAggregate>(result[withTail.ToString()]);
        Assert.Equal(withTail, tail.Id);
        Assert.Equal(14, tail.Amount);
        Assert.Equal(4, tail.Version);
        var snapshot = Assert.IsType<TestAggregate>(result[snapshotOnly.ToString()]);
        Assert.Equal(snapshotOnly, snapshot.Id);
        Assert.Equal(9, snapshot.Amount);
        Assert.Equal(3, snapshot.Version);
        var events = Assert.IsType<TestAggregate>(result[withoutSnapshot.ToString()]);
        Assert.Equal(withoutSnapshot, events.Id);
        Assert.Equal("last", events.Value);
        Assert.Equal(7, events.Amount);
        Assert.Equal(2, events.Version);
    }

    [Fact]
    public async Task ReplayAggregates_WithDuplicateStreamIds_ShouldReturnOneAggregatePerStream()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        using var scope = _fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync<TestAggregate>(streamId, new TestEvent(id, "one"), CancellationToken.None);

        var result = await store.ReplayAggregatesAsync(typeof(TestAggregate), [streamId, streamId], CancellationToken.None);

        var aggregate = Assert.IsType<TestAggregate>(Assert.Single(result).Value);
        Assert.Equal(id, aggregate.Id);
        Assert.Equal("one", aggregate.Value);
        Assert.Equal(0, aggregate.Version);
    }

    [Fact]
    public async Task ReplayAggregates_WithOnlyMissingStreams_ShouldReturnEmpty()
    {
        using var scope = _fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();

        var result = await store.ReplayAggregatesAsync(typeof(TestAggregate), [Guid.NewGuid().ToString()], CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task ReplayAggregates_WithOptionalFilters_UsesSnapshotsToIdentifyStreamsAndTypes()
    {
        var id = Guid.NewGuid();
        using var scope = _fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync<TestAggregate>(id.ToString(), [
            new TestEvent(id, "snapshot"), new SecondTestEvent(id, 2),
            new SecondTestEvent(id, 3), new SecondTestEvent(id, 4),
        ], CancellationToken.None);
        await store.AppendAsync<TestAggregate>(id.ToString(), new SecondTestEvent(id, 5), CancellationToken.None);

        var byType = await store.ReplayAggregatesAsync(typeof(TestAggregate), null, CancellationToken.None);
        var byId = await store.ReplayAggregatesAsync(null, [id.ToString()], CancellationToken.None);

        Assert.Equal(14, Assert.IsType<TestAggregate>(Assert.Single(byType).Value).Amount);
        Assert.Equal(14, Assert.IsType<TestAggregate>(Assert.Single(byId).Value).Amount);
    }

    [Fact]
    public async Task ReplayAggregates_WithoutTypeOrSnapshot_ShouldThrowForExistingStream()
    {
        var id = Guid.NewGuid();
        using var scope = _fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync<TestAggregate>(id.ToString(), new TestEvent(id, "one"), CancellationToken.None);

        await Assert.ThrowsAsync<DandyDotnet.Patterns.EventSourcing.Abstractions.AggregateException>(() =>
            store.ReplayAggregatesAsync(null, [id.ToString()], CancellationToken.None));
    }

    [Fact]
    public async Task ReplayAggregates_WithoutTypeOrStreamIds_ShouldThrow()
    {
        using var scope = _fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await store.ReplayAggregatesAsync(null, null, CancellationToken.None));
    }
}