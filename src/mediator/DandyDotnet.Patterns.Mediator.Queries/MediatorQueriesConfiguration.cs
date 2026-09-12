using DandyDotnet.Patterns.Mediator.Abstractions.Requests.Mapping;
using DandyDotnet.Patterns.Mediator.Configuration;
using DandyDotnet.Patterns.Mediator.Queries.Abstractions;
using DandyDotnet.Patterns.Mediator.Requests.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Patterns.Mediator.Queries;

internal sealed class MediatorQueriesConfiguration : MediatorPluginConfiguration
{
    public override string Slot => MediatorConstants.Plugins.Queries.Slot;

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IRequestResponseMap>(_ => new RequestResponseMap(typeof(IQueryResponse<>), typeof(QueryResponse<>)));
    }
}
