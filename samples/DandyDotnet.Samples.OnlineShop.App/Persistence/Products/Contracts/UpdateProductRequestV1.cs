namespace DandyDotnet.Samples.OnlineShop.App.Persistence.Products.Contracts;

internal sealed record UpdateProductRequestV1(
    string Name,
    string? Description,
    double PriceValue,
    string PriceCurrency);