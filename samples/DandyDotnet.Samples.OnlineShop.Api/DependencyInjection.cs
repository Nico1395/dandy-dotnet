using DandyDotnet.Samples.OnlineShop.Api.Orders;
using DandyDotnet.Samples.OnlineShop.Api.Payment;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel;
using DandyDotnet.Samples.OnlineShop.Api.Users;

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

        return services;
    }
}