using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Products;

internal sealed class Product : ICreatedAt, IUpdatedAt, ISoftDeletedAt
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Sku { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Money Price { get; set; } = Money.Usd(0);
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Deleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}