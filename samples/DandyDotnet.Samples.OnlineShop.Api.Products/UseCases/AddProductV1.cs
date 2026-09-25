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

internal static class AddProductV1
{
    private sealed record AddProductRequestV1(
        string Name,
        string? Description,
        double PriceValue,
        string PriceCurrency);

    [HttpPost("api/v1/products")]
    private static async Task<IResult> AddAsync(
        [FromServices] IMediator mediator,
        [FromBody] AddProductRequestV1 request,
        CancellationToken cancellationToken)
    {
        var price = new Money(request.PriceValue, request.PriceCurrency);
        var command = new Command(request.Name, request.Description, price);
        var response = await mediator.SendAsync(command, cancellationToken);

        return response.Map(ProductV1.Create).ToResult();
    }

    private sealed record Command(
        [NotWhitespace, MaxLength(128)] string Name,
        [MaxLength(512)] string? Description,
        Money Price) : ICommand<Product>;

    private sealed class CommandHandler(DbContext context) : ICommandHandler<Command, Product>
    {
        public async Task<ICommandResponse<Product>> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            if (request.Price.Value < 0 || string.IsNullOrWhiteSpace(request.Price.Currency))
                return CommandResponse.UnprocessableEntity_422<Product>().Build();

            // Beware: This kind of sku is probably silly, but it's enough dummy logic for demo purposes.

            var count = await context.Set<Product>().CountAsync(cancellationToken);
            var no = (count + 1).ToString().PadLeft(20, '0');
            var sku = $"P{no}";     // P + 20 digits = 21 characters
            var skuExists = await context.Set<Product>().AnyAsync(p => p.Sku == sku, cancellationToken);
            if (skuExists)
                return CommandResponse.Conflict_409<Product>().Build();

            var product = new Product
            {
                Sku = sku,
                Name = request.Name,
                Description = request.Description,
            };

            context.Add(product);
            await context.SaveChangesAsync(cancellationToken);

            return CommandResponse
                .OK_200(product)
                .Build();
        }
    }
}