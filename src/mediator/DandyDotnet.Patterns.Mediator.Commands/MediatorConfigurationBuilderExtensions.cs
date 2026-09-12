using DandyDotnet.Patterns.Mediator.Commands.Abstractions;
using DandyDotnet.Patterns.Mediator.Configuration;

namespace DandyDotnet.Patterns.Mediator.Commands;

public static class MediatorConfigurationBuilderExtensions
{
    public static MediatorConfigurationBuilder UseCommands(this MediatorConfigurationBuilder builder)
    {
        builder.UsePlugin(new MediatorCommandsConfiguration());
        builder.ScanForServiceType(typeof(ICommandHandler<>));
        builder.ScanForServiceType(typeof(ICommandHandler<,>));

        return builder;
    }
}
