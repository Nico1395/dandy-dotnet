using Microsoft.EntityFrameworkCore;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure.Persistence;

public sealed class ApiDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assemblies = Assemblies.GetDomainAssemblies();
        foreach (var assembly in assemblies)
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}