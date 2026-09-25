using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

/// <summary>
/// Extensions for the <see cref="IServiceProvider"/> related to SQL-specific event sourcing persistence implementations.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Gets the event-sourcing-related <see cref="IMigrationRunner"/>.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve the migration runner from.</param>
    /// <returns>The event-sourcing-related <see cref="IMigrationRunner"/>.</returns>
    public static IMigrationRunner GetEventSourcingMigrationRunner(this IServiceProvider serviceProvider)
    {
        return serviceProvider.GetRequiredKeyedService<IMigrationRunner>(EventSourcingConstants.ServiceKey);
    }
}