using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Outbox;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests.Fixtures;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests;

public sealed class EventStoreTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture fixture;

    public EventStoreTests(DefaultFixture fixture)
    {
        this.fixture = fixture;
        fixture.ResetDatabase();
    }

    [Fact]
    public async Task AppendEvents_ShouldAppendEventsToStream()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(streamId, new TestEvent(id, "one"), CancellationToken.None);
            var stream = await eventStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None);

            var envelope = Assert.Single(stream);
            Assert.Equal(streamId, envelope.StreamId);
            Assert.Equal(0, envelope.Version);
            Assert.Equal(new TestEvent(id, "one"), envelope.Event);
        }
    }

    [Fact]
    public async Task AppendEvents_ShouldAppendEventsWithSequentialVersions()
    {
        var id = Guid.NewGuid();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync<TestAggregate>(id.ToString(), [
                new TestEvent(id, "one"),
                new SecondTestEvent(id, 2),
                new SecondTestEvent(id, 3),
            ], CancellationToken.None);

            var stream = await eventStore.GetStreamAsync(id.ToString(), null, null, null, null, CancellationToken.None);
            Assert.Equal([0, 1, 2], stream.Select(e => e.Version));
        }
    }

    [Fact]
    public async Task AppendEvents_ShouldAppendEventsToExistingStream()
    {
        var id = Guid.NewGuid();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(id.ToString(), new TestEvent(id, "one"), CancellationToken.None);
            await eventStore.AppendAsync(id.ToString(), new TestEvent(id, "two"), CancellationToken.None);

            var stream = await eventStore.GetStreamAsync(id.ToString(), null, null, null, null, CancellationToken.None);
            Assert.Equal([0, 1], stream.Select(e => e.Version));
        }
    }

    [Fact]
    public async Task AppendEvents_WithEmptyEvents_ShouldNotChangeStream()
    {
        var streamId = Guid.NewGuid().ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(streamId, [], CancellationToken.None);
            Assert.Empty(await eventStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public async Task AppendEvents_WithInvalidStreamId_ShouldThrow(string streamId)
    {
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => eventStore.AppendAsync(streamId, new TestEvent(Guid.NewGuid(), "x"), CancellationToken.None));
        }
    }

    [Fact]
    public async Task AppendEvents_WithCancelledToken_ShouldThrow()
    {
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => eventStore.AppendAsync(Guid.NewGuid().ToString(), new TestEvent(Guid.NewGuid(), "x"), cancellation.Token));
        }
    }

    [Fact]
    public async Task AppendEvents_WithUnconfiguredEvent_ShouldRollbackEventAndOutbox()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var scope = fixture.CreateScope();
        var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await Assert.ThrowsAsync<InvalidOperationException>(() => eventStore.AppendAsync(streamId, new UnconfiguredEvent(id), CancellationToken.None));
        scope.Dispose();

        using var verificationScope = fixture.CreateScope();
        var verificationStore = verificationScope.ServiceProvider.GetRequiredService<IEventStore>();
        Assert.Empty(await verificationStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
        Assert.Empty(await verificationScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Outbox.GetEnvelopesAsync(CancellationToken.None));
    }

    [Fact]
    public async Task GetStream_WithVersionAndTimestampFilters_ShouldApplyInclusiveBounds()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(streamId, [new TestEvent(id, "one"), new TestEvent(id, "two"), new TestEvent(id, "three")], CancellationToken.None);
            var all = await eventStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None);
            var middle = all[1].Timestamp;

            Assert.Equal(2, (await eventStore.GetStreamAsync(streamId, 1, 2, null, null, CancellationToken.None)).Length);
            Assert.Single(await eventStore.GetStreamAsync(streamId, null, null, middle, middle, CancellationToken.None));
            Assert.Empty(await eventStore.GetStreamAsync(streamId, 3, null, null, null, CancellationToken.None));
            Assert.Empty(await eventStore.GetStreamAsync(streamId, null, 0, middle.AddTicks(1), null, CancellationToken.None));
        }
    }

    [Fact]
    public async Task GetStream_ForUnknownStream_ShouldReturnEmptyAndVersionZero()
    {
        var streamId = Guid.NewGuid().ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            Assert.Empty(await eventStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            Assert.Equal(0, await unitOfWork.Envelopes.GetStreamVersionAsync(streamId, CancellationToken.None));
        }
    }

    [Fact]
    public async Task GetStream_WithUnknownEventKey_ShouldThrow()
    {
        var streamId = Guid.NewGuid().ToString();
        using (var connection = fixture.OpenConnection())
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"INSERT INTO {Tables.Envelopes.Table} (stream_id, version, payload, timestamp, event_key) VALUES ($stream, 0, '{{}}', $timestamp, 'UnknownEvent')";
            command.Parameters.AddWithValue("$stream", streamId);
            command.Parameters.AddWithValue("$timestamp", DateTime.UtcNow);
            command.ExecuteNonQuery();
        }

        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await Assert.ThrowsAsync<InvalidOperationException>(() => store.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
    }

    private IEventStore GetEventStore(out IServiceScope scope)
    {
        scope = fixture.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IEventStore>();
    }

    private sealed record UnconfiguredEvent(Guid Id);
}
