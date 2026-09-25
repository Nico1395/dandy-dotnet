using DandyDotnet.Patterns.Mediator.Commands.Abstractions;
using DandyDotnet.Patterns.Mediator.Configuration;

namespace DandyDotnet.Patterns.Mediator.Commands;

/// <summary>
/// Extension for adding commands to the <see cref="MediatorConfigurationBuilder"/>.
/// </summary>
public static class MediatorConfigurationBuilderExtensions
{
    /// <summary>
    /// Adds commands to the <see cref="MediatorConfigurationBuilder"/>.
    /// </summary>
    /// <param name="builder">The builder to add commands to.</param>
    /// <returns>The <see cref="MediatorConfigurationBuilder"/>.</returns>
    public static MediatorConfigurationBuilder UseCommands(this MediatorConfigurationBuilder builder)
    {
        builder.UsePlugin(new MediatorCommandsConfiguration());
        builder.ScanForServiceType(typeof(ICommandHandler<>));
        builder.ScanForServiceType(typeof(ICommandHandler<,>));

        return builder;
    }
}
