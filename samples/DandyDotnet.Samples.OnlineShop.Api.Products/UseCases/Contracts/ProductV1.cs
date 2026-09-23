namespace DandyDotnet.Samples.OnlineShop.Api.Products.UseCases.Contracts;

internal sealed class ProductV1
{
    public required Guid Id { get; init; }
    public required string Sku { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required double PriceValue { get; init; }
    public required string PriceCurrency { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required bool Deleted { get; init; }
    public required DateTime? DeletedAt { get; init; }

    public static ProductV1 Create(Product product)
    {
        return new ProductV1
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Description = product.Description,
            PriceValue = product.Price.Value,
            PriceCurrency = product.Price.Currency,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            Deleted = product.Deleted,
            DeletedAt = product.DeletedAt,
        };
    }
}