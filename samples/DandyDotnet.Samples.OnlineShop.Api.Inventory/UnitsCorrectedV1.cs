using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Inventory;

[Event]
internal sealed record UnitsCorrectedV1(string Sku, int Difference);