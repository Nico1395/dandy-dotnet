namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Contracts.Carts;

public sealed class CartLineItemV1
{
    public required Guid CartId { get; init; }
    public required Guid ProductId { get; init; }
    public required int Quantity { get; set; }
}