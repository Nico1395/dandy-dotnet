using System.Reflection;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel;

public static class Assemblies
{
    public static IEnumerable<Assembly> GetDomainAssemblies()
    {
        return YieldDomainAssemblyNames().Select(Assembly.Load);
    }

    private static IEnumerable<string> YieldDomainAssemblyNames()
    {
        yield return "DandyDotnet.Samples.OnlineShop.Api.SharedKernel";
        yield return "DandyDotnet.Samples.OnlineShop.Api.Orders";
        yield return "DandyDotnet.Samples.OnlineShop.Api.Users";
        yield return "DandyDotnet.Samples.OnlineShop.Api.Payment";
    }
}