using DandyDotnet.Patterns.Mediator.Abstractions.Requests.Mapping;
using DandyDotnet.Patterns.Mediator.Commands.Abstractions;
using DandyDotnet.Patterns.Mediator.Configuration;
using DandyDotnet.Patterns.Mediator.Requests.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Mediator.Commands;

internal sealed class MediatorCommandsConfiguration : MediatorPluginConfiguration
{
    public override string Slot => MediatorConstants.Plugins.Commands.Slot;

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(ICommandResponse), typeof(CommandResponse)));
        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(ICommandResponse<>), typeof(CommandResponse<>)));
    }
}
