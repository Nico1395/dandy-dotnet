using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.EventSourcing.Persistence.Sql;

/// <summary>
///     Represents the base configuration contract for SQL-based event persistence plugins.
/// </summary>
/// <remarks>
///     <para>
///         Subclasses provide database-specific implementations (e.g., PostgreSQL, SQL Server, SQLite)
///         for connection management, schema migration, and repository bindings.
///     </para>
/// </remarks>
public abstract class PersistenceConfiguration : PluginConfiguration
{
    /// <summary>
    ///     Gets or sets the database connection string used by the persistence plugin.
    /// </summary>
    public string? ConnectionString { get; set; }

    /// <inheritdoc />
    public override string Slot => "persistence";

    /// <inheritdoc />
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}