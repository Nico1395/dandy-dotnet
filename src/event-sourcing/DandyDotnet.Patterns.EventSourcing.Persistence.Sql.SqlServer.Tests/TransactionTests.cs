using DandyDotnet.Patterns.EventSourcing.Persistence.Entities;
using DandyDotnet.Patterns.EventSourcing.Persistence;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Fixtures;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql.SqlServer.Tests;

public sealed class TransactionTests : IClassFixture<DefaultFixture>
{
    private readonly DefaultFixture fixture;

    public TransactionTests(DefaultFixture fixture)
    {
        this.fixture = fixture;
        fixture.ResetDatabase();
    }

    [Fact]
    public async Task UnitOfWork_WhenDisposedBeforeCommit_ShouldRollbackPendingChanges()
    {
        var streamId = Guid.NewGuid().ToString();
        using (var scope = fixture.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Envelopes.InsertAsync(streamId, [new EnvelopeEntity
            {
                StreamId = streamId,
                Version = 0,
                Timestamp = DateTime.UtcNow,
                EventKey = nameof(TestEvent),
                Payload = "{}",
            }], CancellationToken.None);
        }

        using var verificationScope = fixture.CreateScope();
        Assert.Empty(await verificationScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Envelopes.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
    }

    [Fact]
    public async Task UnitOfWork_WhenCommitted_ShouldPersistPendingChanges()
    {
        var streamId = Guid.NewGuid().ToString();
        using (var scope = fixture.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Envelopes.InsertAsync(streamId, [new EnvelopeEntity
            {
                StreamId = streamId,
                Version = 0,
                Timestamp = DateTime.UtcNow,
                EventKey = nameof(TestEvent),
                Payload = "{}",
            }], CancellationToken.None);
            await unitOfWork.CommitAsync(CancellationToken.None);
        }

        using var verificationScope = fixture.CreateScope();
        Assert.Single(await verificationScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Envelopes.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
    }

    [Fact]
    public async Task UnitOfWork_CommitWithCancelledToken_ShouldNotCommit()
    {
        var streamId = Guid.NewGuid().ToString();
        using (var scope = fixture.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            await unitOfWork.Envelopes.InsertAsync(streamId, [new EnvelopeEntity
            {
                StreamId = streamId,
                Version = 0,
                Timestamp = DateTime.UtcNow,
                EventKey = nameof(TestEvent),
                Payload = "{}",
            }], CancellationToken.None);
            using var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => unitOfWork.CommitAsync(cancellation.Token));
        }

        using var verificationScope = fixture.CreateScope();
        Assert.Empty(await verificationScope.ServiceProvider.GetRequiredService<IUnitOfWork>().Envelopes.GetStreamAsync(streamId, null, null, null, null, CancellationToken.None));
    }
}


