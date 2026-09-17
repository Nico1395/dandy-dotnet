using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Commands;
using DandyDotnet.Patterns.Mediator.Commands.Abstractions;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Contracts.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DandyDotnet.Samples.OnlineShop.Api.Carts.UseCases;

internal static class AddProductV1
{
    private sealed record AddProductToCartRequestV1(
        Guid CartId,
        Guid ProductId,
        int Quantity);

    [HttpPost("api/v1/carts/line-items/add")]
    private static async Task<IResult> AddToCartV1(
        [FromServices] IMediator mediator,
        [FromBody] AddProductToCartRequestV1 request,
        CancellationToken cancellationToken)
    {
        var command = new Command(request.CartId, request.ProductId, request.Quantity);
        var response = await mediator.SendAsync(command, cancellationToken);

        return response.ToResult();
    }

    private sealed record Command(Guid CartId, Guid ProductId, int Quantity) : ICommand;

    private sealed class CommandHandler(
        IMediator mediator,
        IEventStore eventStore) : ICommandHandler<Command>
    {
        public async Task<ICommandResponse> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            // Get cart
            var cart = await eventStore.ReplayAggregateAsync<Cart>(
                request.CartId.ToString(),
                toVersion: null,
                toTimestamp: null,
                cancellationToken);
            if (cart == null)
                return CommandResponse.NotFound_404().Build();

            // Check if actually in stock
            var productQuantity = new ProductQuantityV1(request.ProductId, request.Quantity);
            var inStockQuery = new InStockQueryV1(productQuantity);
            var inStockResponse = await mediator.SendAsync(inStockQuery, cancellationToken);
            if (!inStockResponse.IsSuccess_2xx() || inStockResponse.Data == null)
                return CommandResponse.BadRequest_400().Build();

            var result = inStockResponse.Data.TryGetValue(productQuantity, out var inStock) && inStock;
            if (!result)
                return CommandResponse.UnprocessableEntity_422().Build();

            // Add product
            var productAdded = new ProductAddedV1(cart.Id, request.ProductId, request.Quantity);
            cart.Apply(productAdded);

            await eventStore.AppendAsync(
                cart.Id.ToString(),
                productAdded,
                cancellationToken);

            return CommandResponse.NoContent_204().Build();
        }
    }
}