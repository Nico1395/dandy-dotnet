using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Fixtures;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests;

public sealed class SnapshotTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture fixture;

    public SnapshotTests(DefaultFixture fixture)
    {
        this.fixture = fixture;
        fixture.ResetDatabase();
    }
    [Fact]
    public async Task AppendEvents_BelowSnapshotInterval_ShouldNotCreateSnapshot()
    {
        var id = Guid.NewGuid();
        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync<TestAggregate>(id.ToString(), [new TestEvent(id, "one"), new SecondTestEvent(id, 2), new SecondTestEvent(id, 3)], CancellationToken.None);
        Assert.Null(await store.GetLastSnapshotAsync(id.ToString(), null, CancellationToken.None));
    }

    [Fact]
    public async Task AppendEvents_AtSnapshotInterval_ShouldCreateSnapshot()
    {
        var id = Guid.NewGuid();
        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync<TestAggregate>(id.ToString(), [
            new TestEvent(id, "one"), new SecondTestEvent(id, 2), new SecondTestEvent(id, 3), new SecondTestEvent(id, 4),
        ], CancellationToken.None);

        var snapshot = await store.GetLastSnapshotAsync(id.ToString(), null, CancellationToken.None);
        Assert.NotNull(snapshot);
        Assert.Equal(3, snapshot.Version);
        Assert.Equal(9, Assert.IsType<TestAggregate>(snapshot.Aggregate).Amount);
    }

    [Fact]
    public async Task GetLastSnapshot_WithVersionBeforeSnapshot_ShouldReturnNull()
    {
        var id = Guid.NewGuid();
        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync<TestAggregate>(id.ToString(), [
            new TestEvent(id, "one"), new SecondTestEvent(id, 2), new SecondTestEvent(id, 3), new SecondTestEvent(id, 4),
        ], CancellationToken.None);

        Assert.Null(await store.GetLastSnapshotAsync(id.ToString(), 2, CancellationToken.None));
        Assert.NotNull(await store.GetLastSnapshotAsync(id.ToString(), 3, CancellationToken.None));
    }

    [Fact]
    public async Task ReplayAggregate_WithSnapshot_ShouldReplayFromSnapshot()
    {
        var id = Guid.NewGuid();
        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync<TestAggregate>(id.ToString(), [
            new TestEvent(id, "one"), new SecondTestEvent(id, 2), new SecondTestEvent(id, 3), new SecondTestEvent(id, 4),
        ], CancellationToken.None);
        await store.AppendAsync<TestAggregate>(id.ToString(), new SecondTestEvent(id, 5), CancellationToken.None);

        var aggregate = await store.ReplayAggregateAsync<TestAggregate>(id.ToString(), null, null, CancellationToken.None);
        Assert.NotNull(aggregate);
        Assert.Equal(14, aggregate.Amount);
        Assert.Equal(4, aggregate.Version);
    }

    [Fact]
    public async Task ReplayAggregate_WithToVersion_ShouldReplayOnlyThroughRequestedVersion()
    {
        var id = Guid.NewGuid();
        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync<TestAggregate>(id.ToString(), [
            new TestEvent(id, "one"), new SecondTestEvent(id, 2), new SecondTestEvent(id, 3), new SecondTestEvent(id, 4),
        ], CancellationToken.None);

        var aggregate = await store.ReplayAggregateAsync<TestAggregate>(id.ToString(), 2, null, CancellationToken.None);
        Assert.NotNull(aggregate);
        Assert.Equal(5, aggregate.Amount);
        Assert.Equal(2, aggregate.Version);
    }

    [Fact]
    public async Task ReplayAggregate_WithTimestampBeforeSnapshot_ShouldNotUseFutureSnapshot()
    {
        var id = Guid.NewGuid();
        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync<TestAggregate>(id.ToString(), [
            new TestEvent(id, "one"), new SecondTestEvent(id, 2), new SecondTestEvent(id, 3), new SecondTestEvent(id, 4),
        ], CancellationToken.None);

        var events = await store.GetStreamAsync(id.ToString(), null, null, null, null, CancellationToken.None);
        var snapshot = await store.GetLastSnapshotAsync(id.ToString(), null, CancellationToken.None);
        var aggregate = await store.ReplayAggregateAsync<TestAggregate>(id.ToString(), null, snapshot!.Timestamp.AddMilliseconds(-1), CancellationToken.None);
        Assert.NotNull(aggregate);
        Assert.Equal(9, aggregate.Amount);
        Assert.Equal(3, aggregate.Version);
    }

    [Fact]
    public async Task ReplayAggregate_ForUnknownStream_ShouldReturnNull()
    {
        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        Assert.Null(await store.ReplayAggregateAsync<TestAggregate>(Guid.NewGuid().ToString(), null, null, CancellationToken.None));
    }
}

