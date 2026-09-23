using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions.Requests;
using DandyDotnet.Patterns.Mediator.Queries;
using DandyDotnet.Patterns.Mediator.Queries.Abstractions;
using DandyDotnet.Samples.OnlineShop.Api.Products.UseCases.Contracts;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure.Persistence;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure.Persistence.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DandyDotnet.Samples.OnlineShop.Api.Products.UseCases;

internal static class GetProductsV1
{
    [HttpGet("api/v1/products")]
    private static async Task<IResult> GetAsync(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new Query();
        var response = await mediator.SendAsync(query, cancellationToken);

        return response.Map(p => p.Select(ProductV1.Create).ToArray()).ToResult();
    }

    private sealed record Query : IQuery<Product[]>;

    private sealed class QueryHandler(ApiDbContext context) : IQueryHandler<Query, Product[]>
    {
        public async Task<IQueryResponse<Product[]>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var products = await context.Set<Product>().ToArrayAsync(cancellationToken);
            return QueryResponse.OK_200(products).Build();
        }
    }
}