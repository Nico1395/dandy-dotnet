// using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
// using DandyDotnet.Patterns.EventSourcing.Abstractions;
// using DandyDotnet.Patterns.Mediator.Abstractions;
// using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
// using DandyDotnet.Patterns.Mediator.Commands;
// using DandyDotnet.Patterns.Mediator.Commands.Abstractions;
// using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Contracts.Carts;
// using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Contracts.Inventory;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
//
// namespace DandyDotnet.Samples.OnlineShop.Api.Orders.UseCases;
//
// internal static class PlaceOrderV1
// {
//     // Shipping information
//
//     private sealed record PlaceOrderRequestV1(
//         string UserId,
//         Guid CartId,
//         Guid PaymentOptionId,
//         string CouponCode);
//
//     [HttpPost("api/v1/orders/place")]
//     private static async Task<IResult> PlaceV1(
//         [FromServices] IMediator mediator,
//         [FromBody] PlaceOrderRequestV1 request,
//         CancellationToken cancellationToken)
//     {
//         var command = new Command(
//             request.UserId,
//             request.CartId,
//             request.PaymentOptionId,
//             request.CouponCode);
//         var response = await mediator.SendAsync(command, cancellationToken);
//
//         return response.ToResult();
//     }
//
//     private sealed record Command(
//         string UserId,
//         Guid CartId,
//         Guid PaymentOptionId,
//         string CouponCode) : ICommand;
//
//     private sealed class CommandHandler(
//         IMediator mediator,
//         IProducer producer,
//         IEventStore eventStore) : ICommandHandler<Command>
//     {
//         public async Task<ICommandResponse> HandleAsync(Command request, CancellationToken cancellationToken)
//         {
//             // Check if user exists
//
//             // Get the cart
//             var cartQuery = new GetCartQueryV1(request.CartId);
//             var cartResponse = await mediator.SendAsync(cartQuery, cancellationToken);
//             var cart = cartResponse.Data;
//             if (!cartResponse.IsSuccess_2xx() || cart == null)
//                 return CommandResponse.NotFound_404().Build();
//
//             // Check if products are in stock
//             var productQuantities = cart.LineItems.Select(l => new ProductQuantityV1(l.ProductId, l.Quantity));
//             var inStockQuery = new InStockQueryV1(productQuantities);
//             var inStockResponse = await mediator.SendAsync(inStockQuery, cancellationToken);
//             if (!inStockResponse.IsSuccess_2xx() || inStockResponse.Data == null)
//                 return CommandResponse.BadRequest_400().Build();
//
//             var anyProductNotInStock = inStockResponse.Data.Any(d => !d.Value);
//             if (anyProductNotInStock)
//                 return CommandResponse.UnprocessableEntity_422().Build();
//
//             // Get total price
//             // Execute payment
//             // Create order
//             // Publish order created event
//             // - Remove stock
//             // - Create new cart
//             // - Create dispatch job
//         }
//     }
// }