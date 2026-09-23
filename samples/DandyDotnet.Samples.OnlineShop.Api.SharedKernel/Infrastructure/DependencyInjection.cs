using System.Reflection;
using DandyDotnet.Encoding.Configuration;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Consumer;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer;
using DandyDotnet.Http.StaticEndpoints;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using DandyDotnet.Patterns.EventSourcing.Persistence.Sql.Postgres;
using DandyDotnet.Patterns.Mediator;
using DandyDotnet.Patterns.Mediator.Commands;
using DandyDotnet.Patterns.Mediator.Queries;
using DandyDotnet.Patterns.Mediator.Validation;
using DandyDotnet.Patterns.Strategies;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Configuration;
using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure.Persistence.EntityFrameworkCore;
using DandyDotnet.Serialization;
using DandyDotnet.Serialization.SystemTextJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOnlineShopApiSharedKernel(this IServiceCollection services, IConfiguration configuration, Assembly[] assemblies)
    {
        // EF Core
        var defaultConnectionString = configuration.GetConnectionString("Default");
        if (defaultConnectionString == null)
            throw new InvalidOperationException("Default connection string is not set in configuration");

        services.AddDbContext<DbContext, ApiDbContext>(cfg =>
        {
            cfg.AddInterceptors(new DomainAbstractionsSaveChangesInterceptor());
            cfg.UseNpgsql(defaultConnectionString);
        });

        // Mediator
        services.AddMediator(cfg =>
        {
            cfg.ScanInAssemblies(assemblies);
            cfg.UseValidation();
            cfg.UseCommands();
            cfg.UseQueries();
        });

        // Event sourcing
        var eventStoreConnectionString = configuration.GetConnectionString("EventStore");
        if (eventStoreConnectionString == null)
            throw new InvalidOperationException("EventStore connection string is not set in configuration");

        services.AddEventSourcing(cfg =>
        {
            cfg.ScanInAssemblies(assemblies);
            cfg.UseNpgsql(npgsql =>
            {
                npgsql.WithConnectionString(eventStoreConnectionString);
            });
        });

        // RabbitMQ
        var rabbitMqOptions = configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
        if (rabbitMqOptions == null)
            throw new InvalidOperationException("RabbitMQ options are not set in configuration");

        services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));

        var encoder = new EncodingConfigurationBuilder().UseServiceKey("event-store");
        var serializer = new SerializerConfigurationBuilder().UseSystemTextJson().UseServiceKey("event-store");
        
        services.AddRabbitMQConsumer(cfg =>
        {
            cfg.ScanInAssemblies(assemblies);

            cfg.Connectivity.ConnectToCluster(
                rabbitMqOptions.UserName ?? throw new InvalidOperationException("RabbitMQ username is not set in configuration"),
                rabbitMqOptions.Password ?? throw new InvalidOperationException("RabbitMQ password is not set in configuration"),
                rabbitMqOptions.Urls?.Select(url => new Uri(url)) ?? throw new InvalidOperationException("RabbitMQ urls are not set in configuration"),
                recoveryInterval: null);

            cfg.Encoder = encoder;
            cfg.Serializer = serializer;
        });

        services.AddRabbitMQProducer(cfg =>
        {
            cfg.Encoder = encoder;
            cfg.Serializer = serializer;
        });

        // Strategies
        services.AddStrategies(cfg =>
        {
            cfg.ScanInAssemblies(assemblies);
        });

        // Static endpoints
        services.AddStaticEndpoints(cfg =>
        {
            cfg.ScanInAssemblies(assemblies);
        });
        
        return services;
    }
}