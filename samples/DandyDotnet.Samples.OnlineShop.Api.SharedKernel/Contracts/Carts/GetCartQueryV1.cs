using DandyDotnet.Patterns.Mediator.Queries.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Contracts.Carts;

public sealed record GetCartQueryV1(Guid CartId) : IQuery<CartV1>;
