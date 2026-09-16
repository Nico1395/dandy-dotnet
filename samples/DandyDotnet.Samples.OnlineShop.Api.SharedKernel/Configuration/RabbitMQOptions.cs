namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Configuration;

public sealed class RabbitMQOptions
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string[]? Urls { get; set; }
}