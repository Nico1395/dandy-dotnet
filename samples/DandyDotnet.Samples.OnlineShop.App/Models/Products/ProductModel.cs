namespace DandyDotnet.Samples.OnlineShop.App.Models.Products;

internal sealed class ProductModel
{
    public required Guid Id { get; init; }
    public required string Sku { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required double PriceValue { get; set; }
    public required string PriceCurrency { get; set; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required bool Deleted { get; init; }
    public required DateTime? DeletedAt { get; init; }
}