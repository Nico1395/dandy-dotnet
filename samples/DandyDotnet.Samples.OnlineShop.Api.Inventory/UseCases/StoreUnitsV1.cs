using System.ComponentModel.DataAnnotations;
using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Commands;
using DandyDotnet.Patterns.Mediator.Commands.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DandyDotnet.Samples.OnlineShop.Api.Inventory.UseCases;

internal static class StoreUnitsV1
{
    private sealed record StoreUnitsRequestV1(string Sku, int Quantity);
    
    [HttpPost("api/v1/stock/store")]
    private static async Task<IResult> StoreV1(
        [FromServices] IMediator mediator,
        [FromBody] StoreUnitsRequestV1 request,
        CancellationToken cancellationToken)
    {
        var command = new Command(request.Sku, request.Quantity);
        var response = await mediator.SendAsync(command, cancellationToken);

        return response.ToResult();
    }

    private sealed record Command(string Sku, int Quantity) : ICommand;

    private sealed class CommandHandler(IEventStore eventStore) : ICommandHandler<Command>
    {
        public async Task<ICommandResponse> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var stock = await eventStore.ReplayAggregateAsync<ProductStock>(
                request.Sku,
                toVersion: null,
                toTimestamp: null,
                cancellationToken);

            if (stock == null)
                return CommandResponse.NotFound_404().Build();

            var unitsStored = new UnitsStoredV1(request.Sku, request.Quantity);
            stock.Apply(unitsStored);

            await eventStore.AppendAsync(stock.Sku, unitsStored, cancellationToken);
            return CommandResponse.NoContent_204().Build();
        }
    }
}