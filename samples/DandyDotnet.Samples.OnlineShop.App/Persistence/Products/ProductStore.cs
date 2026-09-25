using System.Diagnostics;
using System.Net.Http.Json;
using DandyDotnet.Samples.OnlineShop.App.Models.Products;
using DandyDotnet.Samples.OnlineShop.App.Persistence.Products.Contracts;
using Microsoft.JSInterop;

namespace DandyDotnet.Samples.OnlineShop.App.Persistence.Products;

internal sealed class ProductStore(
    IJSRuntime jsRuntime,
    IHttpClientFactory httpClientFactory) : IProductStore
{
    private List<ProductModel> _products = [];

    public IReadOnlyList<ProductModel> Products => _products;

    public async Task<IReadOnlyList<ProductModel>> GetAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateApiClient();

            var products = await httpClient.GetFromJsonAsync<List<ProductModel>>("api/v1/products", cancellationToken);
            return _products = products ?? throw new HttpRequestException("Products were null");
        }
        catch (Exception exception)
        {
            await jsRuntime.InvokeVoidAsync("console.error", cancellationToken: cancellationToken, exception.ToString());
            Debug.WriteLine(exception);
        }

        return _products;
    }

    public async Task<ProductModel?> GetByIdAsync(Guid productId, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateApiClient();
            var product = await httpClient.GetFromJsonAsync<ProductModel>($"api/v1/products/{productId}", cancellationToken);

            // Remove the existing one before adding the new product. But call this after the HTTP call, to ensure
            // we don't remove the product because of a faulty request

            var existing = _products.SingleOrDefault(p => p.Id == productId);
            if (existing != null)
                _products.Remove(existing);

            if (product != null)
                _products.Add(product);

            return product;
        }
        catch (Exception exception)
        {
            await jsRuntime.InvokeVoidAsync("console.error", cancellationToken: cancellationToken, exception.ToString());
            Debug.WriteLine(exception);

            return null;
        }
    }

    public async Task<ProductModel?> AddAsync(AddProductRequestV1 request, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateApiClient();

            var httpResponse = await httpClient.PostAsJsonAsync("api/v1/products", request, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var product = await httpResponse.Content.ReadFromJsonAsync<ProductModel>(cancellationToken: cancellationToken);
            if (product != null)
            {
                var existing = _products.SingleOrDefault(p => p.Id == product.Id);
                if (existing != null)
                    _products.Remove(existing);

                _products.Add(product);
            }

            return product;
        }
        catch (Exception exception)
        {
            await jsRuntime.InvokeVoidAsync("console.error", cancellationToken: cancellationToken, exception.ToString());
            Debug.WriteLine(exception);

            return null;
        }
    }

    public async Task<ProductModel?> UpdateAsync(Guid productId, UpdateProductRequestV1 request, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateApiClient();

            var httpResponse = await httpClient.PatchAsJsonAsync($"api/v1/products/{productId}", request, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();
            var product = await httpResponse.Content.ReadFromJsonAsync<ProductModel>(cancellationToken: cancellationToken);

            var existing = _products.SingleOrDefault(p => p.Id == productId);
            if (existing != null)
                _products.Remove(existing);

            if (product != null)
                _products.Add(product);

            return product;
        }
        catch (Exception exception)
        {
            await jsRuntime.InvokeVoidAsync("console.error", cancellationToken: cancellationToken, exception.ToString());
            Debug.WriteLine(exception);

            return null;
        }
    }

    public async Task DeleteAsync(Guid productId, CancellationToken cancellationToken)
    {
        try
        {
            using var httpClient = httpClientFactory.CreateApiClient();
            await httpClient.DeleteAsync($"api/v1/products/{productId}", cancellationToken);

            var existing = _products.SingleOrDefault(p => p.Id == productId);
            if (existing != null)
                _products.Remove(existing);
        }
        catch (Exception exception)
        {
            await jsRuntime.InvokeVoidAsync("console.error", cancellationToken: cancellationToken, exception.ToString());
            Debug.WriteLine(exception);
        }
    }
}