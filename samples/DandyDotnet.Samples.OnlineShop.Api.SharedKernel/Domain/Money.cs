using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain;

public readonly record struct Money : IParsable<Money>, IFormattable
{
    public Money(double value, string currency)
    {
        Value = value;
        Currency = currency;
    }

    public double Value { get; }
    public string Currency { get; }

    public bool CurrencyEquals(string currency)
    {
        return Currency == currency;
    }

    public bool CurrencyEquals(Money money)
    {
        return Currency == money.Currency;
    }

    public Money ConvertTo(double conversionRate, string currency)
    {
        if (CurrencyEquals(currency))
            throw new InvalidOperationException($"Trying to convert money '{this}' to '{currency}' is illegal.");

        return new Money(Value * conversionRate, currency);
    }

    public override string ToString()
    {
        return $"{Value.ToString(CultureInfo.InvariantCulture)} {Currency}";
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return ToString();
    }

    public static Money Usd(double value)
    {
        return new Money(value, Currencies.Usd);
    }

    public static Money Eur(double value)
    {
        return new Money(value, Currencies.Eur);
    }

    public static Money Parse(string s, IFormatProvider? provider)
    {
        var segments = s.Split(' ');
        if (segments.Length != 2)
            throw new FormatException($"Invalid format for {nameof(Money)}!");

        var value = double.Parse(segments[0]);
        return new Money(value, segments[1]);
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Money result)
    {
        result = default;

        if (string.IsNullOrWhiteSpace(s))
            return false;

        var segments = s.Split(' ');
        if (segments.Length != 2)
            return false;

        var value = double.Parse(segments[0]);
        result = new Money(value, segments[1]);

        return true;
    }

    public static class Currencies
    {
        public const string Usd = "USD";
        public const string Eur = "EUR";
    }
}