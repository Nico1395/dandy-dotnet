using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;
using DandyDotnet.Patterns.EventSourcing.Persistence;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests.Fixtures;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Sqlite.Tests;

public sealed class TransactionTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture _fixture;

    public TransactionTests(DefaultFixture fixture)
    {
        _fixture = fixture;
        fixture.ResetDatabase();
    }

    [Fact]
    public async Task UnitOfWork_WhenDisposedBeforeCommit_ShouldRollbackPendingChanges()
    {
        var streamId = Guid.NewGuid().ToString();
        using (var scope = _fixture.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Envelopes.InsertAsync(streamId, [new EnvelopeEntity
            {
                StreamId = streamId,
                Version = 0,
                Timestamp = DateTime.UtcNow,
                EventKey = nameof(TestEvent),
                Payload = "{}",
                Tags = [],
            }], CancellationToken.None);
        }

        using var verificationScope = _fixture.CreateScope();
        Assert.Empty(await verificationScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Envelopes.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
    }

    [Fact]
    public async Task UnitOfWork_WhenCommitted_ShouldPersistPendingChanges()
    {
        var streamId = Guid.NewGuid().ToString();
        using (var scope = _fixture.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Envelopes.InsertAsync(streamId, [new EnvelopeEntity
            {
                StreamId = streamId,
                Version = 0,
                Timestamp = DateTime.UtcNow,
                EventKey = nameof(TestEvent),
                Payload = "{}",
                Tags = [],
            }], CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);
        }

        using var verificationScope = _fixture.CreateScope();
        Assert.Single(await verificationScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Envelopes.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
    }

    [Fact]
    public async Task UnitOfWork_CommitWithCancelledToken_ShouldNotCommit()
    {
        var streamId = Guid.NewGuid().ToString();
        using (var scope = _fixture.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Envelopes.InsertAsync(streamId, [new EnvelopeEntity
            {
                StreamId = streamId,
                Version = 0,
                Timestamp = DateTime.UtcNow,
                EventKey = nameof(TestEvent),
                Payload = "{}",
                Tags = [],
            }], CancellationToken.None);
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => unitOfWork.CommitAsync(cancellation.Token));
        }

        using var verificationScope = _fixture.CreateScope();
        Assert.Empty(await verificationScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Envelopes.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
    }
}
