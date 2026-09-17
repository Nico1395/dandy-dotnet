using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Carts;

[Aggregate]
internal sealed class Cart
{
    public Cart()
    {
        UserId = string.Empty;
    }

    public Cart(CartCreatedV1 created, DateTime timestamp)
    {
        if (string.IsNullOrWhiteSpace(created.UserId))
            throw new InvalidOperationException("Cannot construct a cart with a null or whitespace user ID");

        Id = Guid.NewGuid();
        UserId = created.UserId;
        CreatedAt = timestamp;
        UpdatedAt = timestamp;
    }

    public Guid Id { get; init; }
    public string UserId { get; init; }
    public long Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CartLineItem> LineItems { get; init; } = [];

    [AggregateFactory]
    public static Cart Create(Cart? snapshot, IReadOnlyEnvelope[] envelopes)
    {
        var createdEnvelope = envelopes.SingleOrDefault(e => e.RuntimeType == typeof(CartCreatedV1));
        if (createdEnvelope is not { Event: CartCreatedV1 created })
            throw new InvalidOperationException("Could not construct cart from the given stream.");

        var shoppingCart = snapshot ?? new Cart(created, createdEnvelope.Timestamp);
        shoppingCart.Apply(envelopes);

        return shoppingCart;
    }

    public void Apply(IEnumerable<IReadOnlyEnvelope> envelopes)
    {
        foreach (var envelope in envelopes)
        {
            if (envelope.Event is CartCreatedV1)
                continue;

            switch (envelope.Event)
            {
                case ProductAddedV1 productAdded:
                    Apply(productAdded);
                    break;
                case ProductRemovedV1 productRemoved:
                    Apply(productRemoved);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown event type: {envelope.Event.GetType().Name}");
            };

            UpdatedAt = envelope.Timestamp;
        }
    }

    public void Apply(ProductAddedV1 productAdded)
    {
        var lineItem = GetLineItem(productAdded.ProductId);
        if (lineItem != null)
        {
            lineItem.AddQuantity(productAdded.Quantity);
            return;
        }

        lineItem = new CartLineItem
        {
            CartId = Id,
            ProductId = productAdded.ProductId,
            Quantity = productAdded.Quantity,
        };

        LineItems.Add(lineItem);
        Version++;
    }

    public void Apply(ProductRemovedV1 productRemoved)
    {
        var lineItem = GetLineItem(productRemoved.ProductId);
        if (lineItem == null)
            return;

        lineItem.RemoveQuantity(productRemoved.Quantity);
        if (lineItem.IsEmpty())
            LineItems.Remove(lineItem);

        Version++;
    }

    private CartLineItem? GetLineItem(Guid productId)
    {
        return LineItems.SingleOrDefault(li => li.ProductId == productId);
    }
}