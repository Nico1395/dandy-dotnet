using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Producer.Abstractions;
using DandyDotnet.EventDrivenArchitecture.RabbitMQ.Sample.Shared;
using Microsoft.Extensions.Hosting;

namespace DandyDotnet.EventDrivenArchitecture.RabbitMQ.Sample.Producer;

internal sealed class ProducerMenu(IProducer producer) : BackgroundService
{
    private string? _inputCommand;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (_inputCommand != "exit")
            {
                await DrawMenuAsync(stoppingToken);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private string? AwaitInput()
    {
        Console.Write("> ");
        return Console.ReadLine();
    }

    private async Task DrawMenuAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            Console.WriteLine("\n----------");
            Console.WriteLine("Main menu:");
            Console.WriteLine("1. Send message");
            Console.WriteLine("2. Exit");

            var input = AwaitInput();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            _inputCommand = input switch
            {
                "1" => "send",
                "2" => "exit",
                _ => null,
            };

            if (string.IsNullOrWhiteSpace(_inputCommand))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            await HandleCommandAsync(_inputCommand, cancellationToken);
            _inputCommand = null;

            break;
        }
    }

    private async Task HandleCommandAsync(string command, CancellationToken cancellationToken)
    {
        if (command == "send")
            await DrawSendMessageAsync(cancellationToken);
    }

    private async Task DrawSendMessageAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            Console.WriteLine("\n-------------");
            Console.WriteLine("Send message:");

            var input = AwaitInput();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            try
            {
                var message = new InputMessageV1
                {
                    Text = input,
                };

                await producer.ProduceAsync(message, cancellationToken);
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}