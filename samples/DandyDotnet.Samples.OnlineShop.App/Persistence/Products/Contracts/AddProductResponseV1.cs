namespace DandyDotnet.Samples.OnlineShop.App.Persistence.Products.Contracts;

internal sealed record AddProductResponseV1(
    Guid Id,
    string Sku);