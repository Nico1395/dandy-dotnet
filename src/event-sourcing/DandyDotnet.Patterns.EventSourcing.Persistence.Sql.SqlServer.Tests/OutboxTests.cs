using System.Text.Json;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.EventSourcing.Outbox;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Constants;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Fixtures;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests;

public sealed class OutboxTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture fixture;

    public OutboxTests(DefaultFixture fixture)
    {
        this.fixture = fixture;
        fixture.ResetDatabase();
    }
    [Fact]
    public async Task AppendEvents_ShouldPublishEventsToOutbox()
    {
        var id = Guid.NewGuid();
        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync(id.ToString(), new TestEvent(id, "outbox"), CancellationToken.None);

        var envelope = Assert.Single(await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().Outbox.GetEnvelopesAsync(CancellationToken.None));
        Assert.Equal(id.ToString(), envelope.StreamId);
        Assert.Equal(0, envelope.Version);
        Assert.Empty(envelope.Consumers);
    }

    [Fact]
    public async Task InlineSubscriber_WhenItSucceeds_ShouldBeRecordedAsConsumed()
    {
        SubscriberRecorder.Clear();
        var id = Guid.NewGuid();
        using var scope = fixture.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEventStore>();
        await store.AppendAsync(id.ToString(), new InlineEvent(id), CancellationToken.None);

        Assert.Contains($"inline:{id}:Inline", SubscriberRecorder.Calls);
        var consumer = Assert.Single((await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().Outbox.GetEnvelopesAsync(CancellationToken.None)).Single().Consumers);
        Assert.True(consumer.ConsumedAt.HasValue);
        Assert.Null(consumer.FailedAt);
    }

    [Fact]
    public async Task CheckAndProcessOutbox_WithAsyncSubscriber_ShouldInvokeAndRecordConsumer()
    {
        SubscriberRecorder.Clear();
        var id = Guid.NewGuid();
        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IEventStore>().AppendAsync(id.ToString(), new AsyncEvent(id), CancellationToken.None);

        using var processingScope = fixture.CreateScope();
        await processingScope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(CancellationToken.None);
        Assert.Contains($"async:{id}:Async", SubscriberRecorder.Calls);
        var consumer = Assert.Single((await processingScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Outbox.GetEnvelopesAsync(CancellationToken.None)).Single().Consumers);
        Assert.True(consumer.ConsumedAt.HasValue);
    }

    [Fact]
    public async Task CheckAndProcessOutbox_WhenSubscriberFails_ShouldRecordFailureAndInvokeHandler()
    {
        SubscriberRecorder.Clear();
        var id = Guid.NewGuid();
        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IEventStore>().AppendAsync(id.ToString(), new FailingEvent(id), CancellationToken.None);

        using var processingScope = fixture.CreateScope();
        await processingScope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(CancellationToken.None);
        Assert.Contains($"handler:{id}:Async", SubscriberRecorder.Calls);
        var consumer = Assert.Single((await processingScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Outbox.GetEnvelopesAsync(CancellationToken.None)).Single().Consumers);
        Assert.Null(consumer.ConsumedAt);
        Assert.True(consumer.FailedAt.HasValue);
    }

    [Fact]
    public async Task CheckAndProcessOutbox_WhenEnvelopeIsExpired_ShouldDeleteEnvelope()
    {
        var id = Guid.NewGuid();
        using (var connection = fixture.OpenConnection())
        using (var command = connection.CreateCommand())
        {
            command.CommandText = $"INSERT INTO {Schema.Name}.{Tables.OutboxEnvelopes.Table} (stream_id, payload, version, timestamp, event_key) VALUES (@stream, @payload, 0, @timestamp, @key)";
            command.Parameters.AddWithValue("@stream", id.ToString());
            command.Parameters.AddWithValue("@payload", JsonSerializer.Serialize(new ExpiringEvent(id)));
            command.Parameters.AddWithValue("@timestamp", DateTime.UtcNow.AddMinutes(-1));
            command.Parameters.AddWithValue("@key", nameof(ExpiringEvent));
            command.ExecuteNonQuery();
        }

        using var scope = fixture.CreateScope();
        await scope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(CancellationToken.None);
        Assert.Empty(await scope.ServiceProvider.GetRequiredService<IUnitOfWork>().Outbox.GetEnvelopesAsync(CancellationToken.None));
    }

    [Fact]
    public async Task CheckAndProcessOutbox_WhenConsumerAlreadySucceeded_ShouldNotInvokeItAgain()
    {
        SubscriberRecorder.Clear();
        var id = Guid.NewGuid();
        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IEventStore>().AppendAsync(id.ToString(), new AsyncEvent(id), CancellationToken.None);

        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(CancellationToken.None);
        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(CancellationToken.None);

        Assert.Single(SubscriberRecorder.Calls);
    }

    [Fact]
    public async Task CheckAndProcessOutbox_WhenConsumerFailed_ShouldRetryIt()
    {
        SubscriberRecorder.Clear();
        var id = Guid.NewGuid();
        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IEventStore>().AppendAsync(id.ToString(), new FailingEvent(id), CancellationToken.None);

        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(CancellationToken.None);
        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(CancellationToken.None);

        Assert.Equal(2, SubscriberRecorder.Exceptions.Count);
        Assert.Equal(2, SubscriberRecorder.Calls.Count(c => c.StartsWith("handler:")));
    }

    [Fact]
    public async Task CheckAndProcessOutbox_WhenMaximumRetriesAreReached_ShouldStopProcessingConsumer()
    {
        SubscriberRecorder.Clear();
        var id = Guid.NewGuid();
        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IEventStore>().AppendAsync(id.ToString(), new FailingEvent(id), CancellationToken.None);

        for (var attempt = 0; attempt < 4; attempt++)
        {
            using var scope = fixture.CreateScope();
            await scope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(CancellationToken.None);
        }

        Assert.Equal(3, SubscriberRecorder.Exceptions.Count);
        Assert.Equal(3, SubscriberRecorder.Calls.Count(c => c.StartsWith("handler:")));
        using var verificationScope = fixture.CreateScope();
        var consumer = Assert.Single((await verificationScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Outbox.GetEnvelopesAsync(CancellationToken.None)).Single().Consumers);
        Assert.Equal(3, consumer.Tries);
    }

    [Fact]
    public async Task CheckAndProcessOutbox_WithCancelledToken_ShouldThrow()
    {
        var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        using var scope = fixture.CreateScope();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => scope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(cancellation.Token));
    }

    [Fact]
    public async Task CheckAndProcessOutbox_WithNoSubscribers_ShouldKeepEnvelope()
    {
        var id = Guid.NewGuid();
        using (var scope = fixture.CreateScope())
            await scope.ServiceProvider.GetRequiredService<IEventStore>().AppendAsync(id.ToString(), new TestEvent(id, "no subscriber"), CancellationToken.None);

        using var processingScope = fixture.CreateScope();
        await processingScope.ServiceProvider.GetRequiredService<IOutbox>().CheckAndProcessAsync(CancellationToken.None);
        var envelope = Assert.Single(await processingScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Outbox.GetEnvelopesAsync(CancellationToken.None));
        Assert.Empty(envelope.Consumers);
    }
}
