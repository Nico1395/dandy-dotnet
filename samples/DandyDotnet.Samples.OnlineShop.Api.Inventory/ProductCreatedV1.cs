using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Inventory;

[Event]
internal sealed record ProductCreatedV1(Guid ProductId);