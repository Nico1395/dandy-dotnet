using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Tests.Mocks.ShoppingCart.Events;

[Event]
internal sealed record CartLineItemAddedV1(
    Guid CartId,
    Guid ProductId,
    int Quantity);