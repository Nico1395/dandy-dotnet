using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Tests.Mocks.ShoppingCart.Events;

[Event]
internal sealed record CartLineItemRemovedV1(
    Guid CartId,
    Guid ProductId);