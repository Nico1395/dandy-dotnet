using DandyDotnet.Samples.OnlineShop.App.Models.Products;
using DandyDotnet.Samples.OnlineShop.App.Persistence.Products.Contracts;

namespace DandyDotnet.Samples.OnlineShop.App.Persistence.Products;

internal interface IProductStore
{
    IReadOnlyList<ProductModelV1> Products { get; }
    Task<ProductModelV1[]> GetAsync(CancellationToken cancellationToken);
    Task<ProductModelV1?> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<AddProductResponseV1?> AddAsync(AddProductRequestV1 request, CancellationToken cancellationToken);
    Task UpdateAsync(ProductModelV1 product, CancellationToken cancellationToken);
    Task DeleteAsync(Guid productId, CancellationToken cancellationToken);
}