using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Commands;
using DandyDotnet.Patterns.Mediator.Commands.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DandyDotnet.Samples.OnlineShop.Api.Inventory.UseCases;

internal static class CorrectUnitsV1
{
    private sealed record CorrectUnitsRequestV1(string Sku, int Difference);

    [HttpPost("api/v1/stock/correct")]
    private static async Task<IResult> CorrectV1(
        [FromServices] IMediator mediator,
        [FromBody] CorrectUnitsRequestV1 request,
        CancellationToken cancellationToken)
    {
        var command = new Command(request.Sku, request.Difference);
        var response = await mediator.SendAsync(command, cancellationToken);

        return response.ToResult();
    }

    private sealed record Command(string Sku, int Difference) : ICommand;

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

            var unitsCorrected = new UnitsCorrectedV1(request.Sku, request.Difference);
            stock.Apply(unitsCorrected);

            await eventStore.AppendAsync(stock.Sku, unitsCorrected, cancellationToken);
            return CommandResponse.NoContent_204().Build();
        }
    }
}