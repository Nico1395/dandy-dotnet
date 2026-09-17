namespace DandyDotnet.Samples.OnlineShop.Api.Carts;

internal sealed record CartLineItem
{
    public required Guid CartId { get; init; }
    public required Guid ProductId { get; init; }
    public int Quantity { get; set; }

    public void AddQuantity(int quantity)
    {
        var newQuantity = Quantity + quantity;
        if (newQuantity < 0)
            newQuantity = 0;

        Quantity = newQuantity;
    }

    public void RemoveQuantity(int quantity)
    {
        AddQuantity(-quantity);
    }

    public bool IsEmpty()
    {
        return Quantity <= 0;
    }
}