using System.ComponentModel.DataAnnotations;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Commands;
using DandyDotnet.Patterns.Mediator.Commands.Abstractions;
using DandyDotnet.Samples.OnlineShop.Api.Products.UseCases.Contracts;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain;
using DandyDotnet.Validation.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DandyDotnet.Samples.OnlineShop.Api.Products.UseCases;

internal static class UpdateProductV1
{
    private sealed record UpdateProductRequestV1(
        string Name,
        string? Description,
        double PriceValue,
        string PriceCurrency);

    [HttpPatch("api/v1/products/{id:guid}")]
    private static async Task<IResult> UpdateAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id,
        [FromBody] UpdateProductRequestV1 request,
        CancellationToken cancellationToken)
    {
        var price = new Money(request.PriceValue, request.PriceCurrency);
        var command = new Command(id, request.Name, request.Description, price);
        var response = await mediator.SendAsync(command, cancellationToken);

        return response.Map(ProductV1.Create).ToResult();
    }

    private sealed record Command(
        Guid Id,
        [NotWhitespace, MaxLength(128)] string Name,
        [MaxLength(512)] string? Description,
        Money Price) : ICommand<Product>;

    private sealed class CommandHandler(DbContext context) : ICommandHandler<Command, Product>
    {
        public async Task<ICommandResponse<Product>> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            if (request.Price.Value < 0 || string.IsNullOrWhiteSpace(request.Price.Currency))
                return CommandResponse.UnprocessableEntity_422<Product>().Build();

            var product = await context.Set<Product>().SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (product == null)
                return CommandResponse.NotFound_404<Product>().Build();

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;

            await context.SaveChangesAsync(cancellationToken);
            return CommandResponse.OK_200(product).Build();
        }
    }
}