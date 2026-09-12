using DandyDotnet.Patterns.Mediator.Configuration;
using DandyDotnet.Patterns.Mediator.Queries.Abstractions;

namespace DandyDotnet.Patterns.Mediator.Queries;

/// <summary>
/// Contains extensions for <see cref="MediatorConfigurationBuilder"/>.
/// </summary>
public static class MediatorConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds validation to the mediator.
    /// </summary>
    /// <param name="builder">The mediator configuration builder to add validation to.</param>
    /// <returns>The mediator configuration builder.</returns>
    public static MediatorConfigurationBuilder UseQueries(this MediatorConfigurationBuilder builder)
    {
        builder.UsePlugin(new MediatorQueriesConfiguration());
        builder.ScanForServiceType(typeof(IQueryHandler<,>));

        return builder;
    }
}
