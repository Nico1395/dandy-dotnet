using System.Reflection;
using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Initializing...");

var settings = new HostApplicationBuilderSettings
{
    Args = args,
    Configuration = new ConfigurationManager(),
    ContentRootPath = Directory.GetCurrentDirectory(),
};

settings.Configuration.AddJsonFile("appsettings.json", optional: false);

var builder = Host.CreateApplicationBuilder(settings);
var assemblies = new [] { Assembly.Load("DandyDotnet.EventDrivenArchitecture.RabbitMQ.Sample.Consumer"), Assembly.Load("DandyDotnet.EventDrivenArchitecture.RabbitMQ.Sample.Shared") };

builder.Services.AddSerializer(cfg => cfg.UseSystemTextJson());
builder.Services.AddEncoder();
builder.Services.AddRabbitMQConsumer(cfg =>
{
    cfg.Connectivity.ConnectToCluster("dev", "dev", [new Uri("localhost:5672"), new Uri("localhost:5673")], recoveryInterval: null);
    cfg.Connectivity.OnConnectionException((_, ex) => Console.WriteLine($"Exception occurred: {ex}"));
    cfg.Declarations.SubscribeChannel("messages", "consumer-1", channel =>
    {
        channel.Queue.RoutingKeys = ["all"];
    });
    cfg.ScanInAssemblies(assemblies);
});

Console.WriteLine("...done!");
Console.WriteLine("Starting...");

builder.Build().Run();