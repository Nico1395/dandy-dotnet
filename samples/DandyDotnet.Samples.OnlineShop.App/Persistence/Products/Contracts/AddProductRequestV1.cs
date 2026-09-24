namespace DandyDotnet.Samples.OnlineShop.App.Persistence.Products.Contracts;

internal sealed class AddProductRequestV1
{
    public string Name { get; set; } = "Product";
    public string? Description { get; set; }
    public double PriceValue { get; set; } = 0;
    public string PriceCurrency { get; set; } = "USD";
}