namespace DandyDotnet.Samples.OnlineShop.App;

internal static class HttpClientFactoryExtensions
{
    public static HttpClient CreateApiClient(this IHttpClientFactory factory)
    {
        return factory.CreateClient("api");
    }
}