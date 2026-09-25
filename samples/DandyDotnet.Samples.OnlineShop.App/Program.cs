using DandyDotnet.Samples.OnlineShop.App.Persistence.Products;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace DandyDotnet.Samples.OnlineShop.App;

internal sealed class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddHttpClient("api", httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://localhost:7020/");
        });

        builder.Services.AddSingleton<IProductStore, ProductStore>();

        await builder.Build().RunAsync();
    }
}