using DandyDotnet.Patterns.EventSourcing.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Inventory;

[Aggregate(SnapshotInterval = 10)]
internal sealed class ProductStock
{
    public ProductStock()
    {
        Sku = string.Empty;
    }

    public ProductStock(ProductCreatedV1 created, DateTime timestamp)
    {
        Sku = created.ProductId.ToString();
        CreatedAt = timestamp;
        UpdatedAt = timestamp;
    }

    public string Sku { get; init; }
    public int Quantity { get; set; }
    public long Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    [AggregateFactory]
    public static ProductStock Create(ProductStock? snapshot, IReadOnlyEnvelope[] envelopes)
    {
        var createdEnvelope = envelopes.SingleOrDefault(e => e.RuntimeType == typeof(ProductCreatedV1));
        if (createdEnvelope is not { Event: ProductCreatedV1 created })
            throw new InvalidOperationException("Could not construct cart from the given stream.");

        var shoppingCart = snapshot ?? new ProductStock(created, createdEnvelope.Timestamp);
        shoppingCart.Apply(envelopes);

        return shoppingCart;
    }

    public void Apply(IEnumerable<IReadOnlyEnvelope> envelopes)
    {
        foreach (var envelope in envelopes)
        {
            if (envelope.Event is ProductCreatedV1)
                continue;

            switch (envelope.Event)
            {
                case UnitsStoredV1 stored:
                    Apply(stored);
                    break;
                case UnitsRemovedV1 sold:
                    Apply(sold);
                    break;
                case UnitsCorrectedV1 corrected:
                    Apply(corrected);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown event type: {envelope.Event.GetType().Name}");
            }

            UpdatedAt = envelope.Timestamp;
        }
    }

    public void Apply(UnitsStoredV1 stored)
    {
        AddQuantity(stored.Quantity);
        Version++;
    }

    public void Apply(UnitsRemovedV1 removed)
    {
        AddQuantity(-removed.Quantity);
        Version++;
    }

    public void Apply(UnitsCorrectedV1 corrected)
    {
        AddQuantity(corrected.Difference);
        Version++;
    }

    public bool IsInStock(int quantity)
    {
        return Quantity - quantity >= 0;
    }

    private void AddQuantity(int quantity)
    {
        var newQuantity = Quantity + quantity;
        if (newQuantity < 0)
            newQuantity = 0;

        Quantity = newQuantity;
    }
}