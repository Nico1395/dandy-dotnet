using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Queries;
using DandyDotnet.Patterns.Mediator.Queries.Abstractions;
using DandyDotnet.Samples.OnlineShop.Api.Products.UseCases.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DandyDotnet.Samples.OnlineShop.Api.Products.UseCases;

internal static class GetProductByIdV1
{
    [HttpGet("api/v1/products/{id:guid}")]
    private static async Task<IResult> GetAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new Query(id);
        var response = await mediator.SendAsync(query, cancellationToken);

        return response.Map(ProductV1.Create).ToResult();
    }

    private sealed record Query(Guid Id) : IQuery<Product>;

    private sealed class QueryHandler(DbContext context) : IQueryHandler<Query, Product>
    {
        public async Task<IQueryResponse<Product>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var products = await context.Set<Product>().SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            return QueryResponse.OkOrNotFound(products).Build();
        }
    }
}