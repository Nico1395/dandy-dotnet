using System.Net.Http.Json;
using DandyDotnet.Samples.OnlineShop.App.Models.Products;
using DandyDotnet.Samples.OnlineShop.App.Persistence.Products.Contracts;

namespace DandyDotnet.Samples.OnlineShop.App.Persistence.Products;

internal sealed class ProductStore(IHttpClientFactory httpClientFactory) : IProductStore
{
    private ProductModelV1[] _models = [];

    public IReadOnlyList<ProductModelV1> Products => _models;

    public async Task<ProductModelV1[]> GetAsync(CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient();

        var products = await httpClient.GetFromJsonAsync<ProductModelV1[]>("api/v1/products", cancellationToken);
        return _models = products ?? throw new HttpRequestException("Products were null");
    }

    public async Task<ProductModelV1?> GetByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient();
        return await httpClient.GetFromJsonAsync<ProductModelV1>($"api/v1/products/{productId}", cancellationToken);
    }

    public async Task<AddProductResponseV1?> AddAsync(AddProductRequestV1 request, CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient();

        var httpResponse = await httpClient.PostAsJsonAsync("api/v1/products", request, cancellationToken);
        httpResponse.EnsureSuccessStatusCode();

        return await httpResponse.Content.ReadFromJsonAsync<AddProductResponseV1>(cancellationToken);
    }

    public async Task UpdateAsync(ProductModelV1 product, CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient();
        
        var request = new UpdateProductRequestV1(product.Name, product.Description, product.PriceValue, product.PriceCurrency);
        var httpResponse = await httpClient.PatchAsJsonAsync($"api/v1/products/{product.Id}", request, cancellationToken);

        httpResponse.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid productId, CancellationToken cancellationToken)
    {
        using var httpClient = httpClientFactory.CreateClient();
        await httpClient.DeleteAsync($"api/v1/products/{productId}", cancellationToken);
    }
}