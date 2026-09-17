using DandyDotnet.Patterns.Mediator.Queries.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Contracts.Inventory;

public sealed record InStockQueryV1(params IEnumerable<ProductQuantityV1> ProductQuantities) : IQuery<Dictionary<ProductQuantityV1, bool>>; 
