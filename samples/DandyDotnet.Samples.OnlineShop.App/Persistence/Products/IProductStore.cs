using DandyDotnet.Samples.OnlineShop.App.Models.Products;
using DandyDotnet.Samples.OnlineShop.App.Persistence.Products.Contracts;

namespace DandyDotnet.Samples.OnlineShop.App.Persistence.Products;

internal interface IProductStore
{
    IReadOnlyList<ProductModel> Products { get; }
    Task<IReadOnlyList<ProductModel>> GetAsync(CancellationToken cancellationToken);
    Task<ProductModel?> GetByIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<ProductModel?> AddAsync(AddProductRequestV1 request, CancellationToken cancellationToken);
    Task<ProductModel?> UpdateAsync(Guid productId, UpdateProductRequestV1 request, CancellationToken cancellationToken);
    Task DeleteAsync(Guid productId, CancellationToken cancellationToken);
}