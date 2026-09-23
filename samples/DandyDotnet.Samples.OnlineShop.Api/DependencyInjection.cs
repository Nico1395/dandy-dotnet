using DandyDotnet.Samples.OnlineShop.Api.Carts.Infrastructure;
using DandyDotnet.Samples.OnlineShop.Api.Inventory.Infrastructure;
using DandyDotnet.Samples.OnlineShop.Api.Orders.Infrastructure;
using DandyDotnet.Samples.OnlineShop.Api.Payment.Infrastructure;
using DandyDotnet.Samples.OnlineShop.Api.Products.Infrastructure;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure;
using DandyDotnet.Samples.OnlineShop.Api.Users.Infrastructure;

namespace DandyDotnet.Samples.OnlineShop.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddOnlineShopApi(this IServiceCollection services, IConfiguration configuration)
    {
        var assemblies = Assemblies.GetDomainAssemblies().ToArray();

        services.AddOnlineShopApiSharedKernel(configuration, assemblies);
        services.AddOnlineShopApiOrders();
        services.AddOnlineShopApiUsers();
        services.AddOnlineShopApiPayment();
        services.AddOnlineShopApiCarts();
        services.AddOnlineShopApiProducts();
        services.AddOnlineShopApiInventory();

        return services;
    }
}