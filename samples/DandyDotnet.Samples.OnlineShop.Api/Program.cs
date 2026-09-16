using DandyDotnet.Http.StaticEndpoints;

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

        app.Run();
    }
}