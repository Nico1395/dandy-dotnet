using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Fixtures;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests;

public sealed class EventStoreTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture _fixture;

    public EventStoreTests(DefaultFixture fixture)
    {
        _fixture = fixture;
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
            await Assert.ThrowsAsync<EventStreamException>(() => eventStore.AppendAsync(streamId, new TestEvent(Guid.NewGuid(), "x"), CancellationToken.None));
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
        var scope = _fixture.CreateScope();
        var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await Assert.ThrowsAsync<InvalidOperationException>(() => eventStore.AppendAsync(streamId, new UnconfiguredEvent(id), CancellationToken.None));
        scope.Dispose();

        using var verificationScope = _fixture.CreateScope();
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
            await eventStore.AppendAsync(streamId, new TestEvent(id, "one"), CancellationToken.None);
            await Task.Delay(50);
            await eventStore.AppendAsync(streamId, new TestEvent(id, "two"), CancellationToken.None);
            await Task.Delay(50);
            await eventStore.AppendAsync(streamId, new TestEvent(id, "three"), CancellationToken.None);
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
        using (var connection = _fixture.OpenConnection())
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"INSERT INTO {Schema.Name}.{Tables.Envelopes.Table} (stream_id, version, payload, timestamp, event_key) VALUES (@stream, 0, '{{}}', @timestamp, 'UnknownEvent')";
            command.Parameters.AddWithValue("@stream", streamId);
            command.Parameters.AddWithValue("@timestamp", DateTime.UtcNow);
            command.ExecuteNonQuery();
        }

        using var scope = _fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await Assert.ThrowsAsync<InvalidOperationException>(() => store.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
    }

    [Fact]
    public async Task AppendEvents_WithTags_ShouldStoreTagsOnEnvelopes()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(null, streamId, new TestEvent(id, "tagged"), ["tag-a", "tag-b"], CancellationToken.None);
            var stream = await eventStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None);

            var envelope = Assert.Single(stream);
            Assert.Equal(["tag-a", "tag-b"], envelope.Tags);
        }
    }

    [Fact]
    public async Task AppendEvents_WithDuplicateAndWhitespaceTags_ShouldDeduplicateAndFilterTags()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(null, streamId, new TestEvent(id, "tagged"), ["zebra", "apple", "apple", " ", null!, "zebra"], CancellationToken.None);
            var stream = await eventStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None);

            var envelope = Assert.Single(stream);
            Assert.Equal(["apple", "zebra"], envelope.Tags);
        }
    }

    [Fact]
    public async Task AppendEvents_WithNullTags_ShouldStoreEmptyTags()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(null, streamId, new TestEvent(id, "not-tagged"), null, CancellationToken.None);
            var stream = await eventStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None);

            var envelope = Assert.Single(stream);
            Assert.Empty(envelope.Tags);
        }
    }

    [Fact]
    public async Task AppendEvents_WithMultipleEventsAndTags_ShouldStoreRespectiveTags()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync<TestAggregate>(streamId, [
                (new TestEvent(id, "first"), ["tag1", "common"]),
                (new SecondTestEvent(id, 10), ["tag2", "common"]),
                (new SecondTestEvent(id, 20), null),
            ], CancellationToken.None);

            var stream = await eventStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None);
            Assert.Equal(3, stream.Length);
            Assert.Equal(["common", "tag1"], stream[0].Tags);
            Assert.Equal(["common", "tag2"], stream[1].Tags);
            Assert.Empty(stream[2].Tags);
        }
    }

    [Theory]
    [InlineData("tag;with;delimiter")]
    [InlineData(";leading")]
    [InlineData("trailing;")]
    public async Task AppendEvents_WithTagsContainingDelimiter_ShouldThrow(string invalidTag)
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await Assert.ThrowsAsync<EventStreamException>(() =>
                eventStore.AppendAsync(null, streamId, new TestEvent(id, "invalid"), [invalidTag], CancellationToken.None));
        }
    }

    [Fact]
    public async Task GetEnvelopes_WithMatchingTag_ShouldReturnMatchingEnvelopesAcrossStreams()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        var stream1 = id1.ToString();
        var stream2 = id2.ToString();
        var stream3 = id3.ToString();

        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(null, stream1, new TestEvent(id1, "event1"), ["audit", "order"], CancellationToken.None);
            await eventStore.AppendAsync(null, stream2, new TestEvent(id2, "event2"), ["audit", "customer"], CancellationToken.None);
            await eventStore.AppendAsync(null, stream3, new TestEvent(id3, "event3"), ["inventory"], CancellationToken.None);

            var auditEnvelopes = await eventStore.GetEnvelopesAsync(["audit"], CancellationToken.None);

            Assert.Equal(2, auditEnvelopes.Length);
            Assert.Contains(auditEnvelopes, e => e.StreamId == stream1 && ((TestEvent)e.Event).Value == "event1");
            Assert.Contains(auditEnvelopes, e => e.StreamId == stream2 && ((TestEvent)e.Event).Value == "event2");
        }
    }

    [Fact]
    public async Task GetEnvelopes_WithMultipleTags_ShouldReturnEnvelopesMatchingAnyTag()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var id3 = Guid.NewGuid();
        var stream1 = id1.ToString();
        var stream2 = id2.ToString();
        var stream3 = id3.ToString();

        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(null, stream1, new TestEvent(id1, "event1"), ["alpha"], CancellationToken.None);
            await eventStore.AppendAsync(null, stream2, new TestEvent(id2, "event2"), ["beta"], CancellationToken.None);
            await eventStore.AppendAsync(null, stream3, new TestEvent(id3, "event3"), ["gamma"], CancellationToken.None);

            var result = await eventStore.GetEnvelopesAsync(["alpha", "beta"], CancellationToken.None);

            Assert.Equal(2, result.Length);
            Assert.Contains(result, e => e.StreamId == stream1);
            Assert.Contains(result, e => e.StreamId == stream2);
            Assert.DoesNotContain(result, e => e.StreamId == stream3);
        }
    }

    [Fact]
    public async Task GetEnvelopes_WithNonMatchingTag_ShouldReturnEmpty()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(null, streamId, new TestEvent(id, "event"), ["existing-tag"], CancellationToken.None);
            var result = await eventStore.GetEnvelopesAsync(["non-existent-tag"], CancellationToken.None);

            Assert.Empty(result);
        }
    }

    [Fact]
    public async Task GetEnvelopes_WithEmptyOrWhitespaceTags_ShouldReturnEmpty()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(null, streamId, new TestEvent(id, "event"), ["some-tag"], CancellationToken.None);

            Assert.Empty(await eventStore.GetEnvelopesAsync([], CancellationToken.None));
            Assert.Empty(await eventStore.GetEnvelopesAsync(["", "  ", "\t"], CancellationToken.None));
        }
    }

    [Fact]
    public async Task AppendEvents_WithGenericAggregateAndTags_ShouldStoreTags()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync<TestAggregate>(streamId, new TestEvent(id, "val"), ["generic-tag"], CancellationToken.None);
            var stream = await eventStore.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None);

            var envelope = Assert.Single(stream);
            Assert.Equal(["generic-tag"], envelope.Tags);
        }
    }

    [Fact]
    public async Task GetEnvelopes_WithMatchingTag_ShouldReturnFullyPopulatedEnvelopes()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            var testEvent = new TestEvent(id, "payload-check");
            await eventStore.AppendAsync(null, streamId, testEvent, ["target-tag"], CancellationToken.None);

            var envelopes = await eventStore.GetEnvelopesAsync(["target-tag"], CancellationToken.None);

            var envelope = Assert.Single(envelopes);
            Assert.Equal(streamId, envelope.StreamId);
            Assert.Equal(0, envelope.Version);
            Assert.Equal(testEvent, envelope.Event);
            Assert.Equal(nameof(TestEvent), envelope.EventKey);
            Assert.Equal(typeof(TestEvent), envelope.RuntimeType);
            Assert.Equal(["target-tag"], envelope.Tags);
            Assert.True(envelope.Timestamp <= DateTime.UtcNow);
        }
    }

    [Fact]
    public async Task GetEnvelopes_WithDuplicateAndWhitespaceQueryTags_ShouldSanitizeQueryAndMatch()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(null, streamId, new TestEvent(id, "event"), ["sanitized-query"], CancellationToken.None);
            var result = await eventStore.GetEnvelopesAsync(["sanitized-query", "sanitized-query", " ", null!], CancellationToken.None);

            var envelope = Assert.Single(result);
            Assert.Equal(streamId, envelope.StreamId);
        }
    }

    [Fact]
    public async Task GetEnvelopes_WithSubstringTag_ShouldNotMatch()
    {
        var id = Guid.NewGuid();
        var streamId = id.ToString();
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            await eventStore.AppendAsync(null, streamId, new TestEvent(id, "event"), ["tag12"], CancellationToken.None);
            var result = await eventStore.GetEnvelopesAsync(["tag1"], CancellationToken.None);

            Assert.Empty(result);
        }
    }

    [Fact]
    public async Task GetEnvelopes_WithCancelledToken_ShouldThrow()
    {
        var eventStore = GetEventStore(out var scope);
        using (scope)
        {
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
                eventStore.GetEnvelopesAsync(["any-tag"], cancellation.Token));
        }
    }

    private IEventStore GetEventStore(out IServiceScope scope)
    {
        scope = _fixture.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IEventStore>();
    }

    private sealed record UnconfiguredEvent(Guid Id);
}
