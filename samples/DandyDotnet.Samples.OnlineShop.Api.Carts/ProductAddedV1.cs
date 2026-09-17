using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Carts;

[Event]
internal sealed record ProductAddedV1(
    Guid CartId,
    Guid ProductId,
    int Quantity);