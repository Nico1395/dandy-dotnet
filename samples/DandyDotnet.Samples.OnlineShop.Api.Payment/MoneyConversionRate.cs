using DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain.Abstractions;

namespace DandyDotnet.Samples.OnlineShop.Api.Payment;

internal sealed class MoneyConversionRate : IUpdatedAt
{
    public required string Currency { get; init; }
    public double RateToUsd { get; init; }
    public DateTime UpdatedAt { get; set; }
}