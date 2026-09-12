using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Patterns.EventSourcing.Tests.Mocks.ShoppingCart.Events;

[Event]
internal sealed record CartCreatedV1(
    Guid CartId,
    string UserId);