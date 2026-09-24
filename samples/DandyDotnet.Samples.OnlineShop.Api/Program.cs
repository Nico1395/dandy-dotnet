using DandyDotnet.Http.StaticEndpoints;
using DandyDotnet.Persistence.Sql.Migrations.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DandyDotnet.Samples.OnlineShop.Api;

internal sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();
        builder.Services.AddOnlineShopApi(builder.Configuration);

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapStaticEndpoints();

        RunEntityFrameworkCoreMigrations(app);
        RunEventSourcingMigrations(app);

        app.Run();
    }

    private static void RunEntityFrameworkCoreMigrations(WebApplication app)
    {
        try
        {
            using var context = app.Services.GetRequiredService<DbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private static void RunEventSourcingMigrations(WebApplication app)
    {
        try
        {
            var migrationRunner = app.Services.GetRequiredKeyedService<IMigrationRunner>("event-sourcing");
            migrationRunner.MigrateUp();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
}