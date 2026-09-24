using Microsoft.EntityFrameworkCore;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure.Persistence.EntityFrameworkCore;

internal sealed class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assemblies = Assemblies.GetDomainAssemblies();
        foreach (var assembly in assemblies)
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}