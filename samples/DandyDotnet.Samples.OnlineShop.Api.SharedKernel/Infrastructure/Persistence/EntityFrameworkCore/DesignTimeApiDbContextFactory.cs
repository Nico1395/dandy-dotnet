using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure.Persistence.EntityFrameworkCore;

internal sealed class DesignTimeApiDbContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
{
    public ApiDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        
        var defaultConnectionString = configuration.GetConnectionString("Default");
        if (defaultConnectionString == null)
            throw new InvalidOperationException("Default connection string is not set in configuration");

        var builder = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql(defaultConnectionString);
        return new ApiDbContext(builder.Options);
    }
}