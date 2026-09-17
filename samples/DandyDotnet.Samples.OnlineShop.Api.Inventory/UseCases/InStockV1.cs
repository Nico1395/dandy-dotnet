using DandyDotnet.Patterns.EventSourcing.Abstractions;
using DandyDotnet.Patterns.Mediator.Abstractions;
using DandyDotnet.Patterns.Mediator.Queries;
using DandyDotnet.Patterns.Mediator.Queries.Abstractions;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Contracts.Inventory;

namespace DandyDotnet.Samples.OnlineShop.Api.Inventory.UseCases;

internal sealed class InStockV1
{
    private sealed class InStockQueryHandlerV1(IMediator mediator) : IQueryHandler<InStockQueryV1, Dictionary<ProductQuantityV1, bool>>
    {
        public async Task<IQueryResponse<Dictionary<ProductQuantityV1, bool>>> HandleAsync(InStockQueryV1 request, CancellationToken cancellationToken)
        {
            var query = new Query(request.ProductQuantities.Select(p => new ProductQuantity(p.ProductId, p.Quantity)));
            var response = await mediator.SendAsync(query, cancellationToken);

            return response.Map(p => p.ToDictionary(
                m => new ProductQuantityV1(m.Key.ProductId, m.Key.Quantity),
                m => m.Value));
        }
    }

    private sealed record ProductQuantity(Guid ProductId, int Quantity);

    private sealed record Query(IEnumerable<ProductQuantity> ProductQuantities) : IQuery<Dictionary<ProductQuantity, bool>>;

    private sealed class QueryHandler(IEventStore eventStore) : IQueryHandler<Query, Dictionary<ProductQuantity, bool>>
    {
        public async Task<IQueryResponse<Dictionary<ProductQuantity, bool>>> HandleAsync(Query request, CancellationToken cancellationToken)
        {
            var results = new Dictionary<ProductQuantity, bool>();
            foreach (var productQuantity in request.ProductQuantities)
            {
                var stock = await eventStore.ReplayAggregateAsync<ProductStock>(
                    productQuantity.ProductId.ToString(),
                    toVersion: null,
                    toTimestamp: null,
                    cancellationToken);

                results[productQuantity] = stock != null && stock.IsInStock(productQuantity.Quantity);
            }

            return QueryResponse.OK_200(results).Build();
        }
    }
}