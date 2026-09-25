using System.Diagnostics;
using DandyDotnet.Patterns.EventSourcing.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DandyDotnet.Patterns.EventSourcing.Outbox;

internal sealed class AsyncOutboxDaemon(
    EventStoreConfiguration eventStoreConfiguration,
    IServiceProvider serviceProvider) : BackgroundService
{
    private IServiceScope? _serviceScope;

    public override void Dispose()
    {
        base.Dispose();
        _serviceScope?.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _serviceScope ??= serviceProvider.CreateScope();
        var outbox = _serviceScope.ServiceProvider.GetRequiredService<IOutbox>();

        while (!stoppingToken.IsCancellationRequested)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await outbox.NotifyAsyncSubscribersAsync(stoppingToken);
            }
            catch (Exception exception)
            {
                eventStoreConfiguration.OnDaemonIterationException?.Invoke(serviceProvider, exception);
            }

            stopwatch.Stop();
            var remainingTime = eventStoreConfiguration.Outbox.Interval - stopwatch.Elapsed;

            await Task.Delay(remainingTime, stoppingToken);
        }
    }
}