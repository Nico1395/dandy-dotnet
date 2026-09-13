using System.Reflection;
using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Sample.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
var assemblies = new [] { Assembly.Load("DandyDotnet.EventDrivenArchitecture.RabbitMQ.Sample.Producer"), Assembly.Load("DandyDotnet.EventDrivenArchitecture.RabbitMQ.Sample.Shared") };

builder.Services.AddHostedService<ProducerMenu>();
builder.Services.AddDandySerializer(cfg => cfg.UseSystemTextJson());
builder.Services.AddDandyEncoder();
builder.Services.AddDandyRabbitMQProducer(cfg =>
{
    cfg.Connectivity.ConnectToCluster("dev", "dev", [new Uri("localhost:5672"), new Uri("localhost:5673")], recoveryInterval: null);
    cfg.Connectivity.OnConnectionException((_, ex) => Console.WriteLine($"Exception occurred: {ex}"));
    cfg.Declarations.SubscribeChannel("messages", "consumer-1", channel =>
    {
        channel.Queue.RoutingKeys = ["all"];
    });
    cfg.Messages.ScanInAssemblies(assemblies);
    cfg.ScanInAssemblies(assemblies);
});

Console.WriteLine("...done!");
Console.WriteLine("Starting...");

builder.Build().Run();
