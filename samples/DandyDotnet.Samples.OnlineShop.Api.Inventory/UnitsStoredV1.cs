using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Inventory;

[Event]
internal sealed record UnitsStoredV1(string Sku, int Quantity);