using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

/// <summary>
/// Extensions for the <see cref="WebApplication"/> related to SQL-specific event sourcing persistence implementations.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Runs event-sourcing-related migrations.
    /// </summary>
    /// <param name="app">The web application to run the migrations from.</param>
    public static void RunEventSourcingMigrations(this WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var migrationRunner = scope.ServiceProvider.GetEventSourcingMigrationRunner();

            migrationRunner.MigrateUp();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }
    }
}