using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure.Persistence.EntityFrameworkCore;

internal sealed class DomainAbstractionsSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        OnSavingChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        OnSavingChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void OnSavingChanges(DbContext? context)
    {
        if (context == null)
            return;

        foreach (var entityEntry in context.ChangeTracker.Entries().Where(e => e.State > EntityState.Deleted))
        {
            switch (entityEntry)
            {
                case { State: EntityState.Added, Entity: ICreatedAt createdAt }:
                    createdAt.CreatedAt = DateTime.UtcNow;
                    break;
                case { State: EntityState.Modified or EntityState.Added, Entity: IUpdatedAt updatedAt }:
                    updatedAt.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
    }
}