using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Orders;

[Event]
internal sealed record OrderPlacedV1();