namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Contracts.Carts;

public sealed class CartV1
{
    public required Guid Id { get; init; }
    public required string UserId { get; init; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required List<CartLineItemV1> LineItems { get; init; } = [];
}