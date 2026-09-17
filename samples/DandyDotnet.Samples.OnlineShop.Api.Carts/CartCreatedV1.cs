using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Carts;

[Event]
internal sealed record CartCreatedV1(string UserId);