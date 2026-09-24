using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Commands;
using DandyDotnet.Patterns.Mediator.Commands.Abstractions;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DandyDotnet.Samples.OnlineShop.Api.Products.UseCases;

internal static class DeleteProductV1
{
    [HttpDelete("api/v1/products/{id:guid}")]
    private static async Task<IResult> DeleteAsync(
        [FromRoute] Guid id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new Command(id);
        var response = await mediator.SendAsync(command, cancellationToken);

        return response.ToResult();
    }

    private sealed record Command(Guid Id) : ICommand;
    
    private sealed class CommandHandler(DbContext context) : ICommandHandler<Command>
    {
        public async Task<ICommandResponse> HandleAsync(Command request, CancellationToken cancellationToken)
        {
            var product = await context.Set<Product>().SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (product == null)
                return CommandResponse.NotFound_404().Build();

            if (product.Deleted)
                return CommandResponse.Conflict_409().Build();

            product.Delete();
            await context.SaveChangesAsync(cancellationToken);

            return CommandResponse.NoContent_204().Build();
        }
    }
}