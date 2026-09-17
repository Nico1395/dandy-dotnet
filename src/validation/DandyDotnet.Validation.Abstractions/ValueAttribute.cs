using System.ComponentModel.DataAnnotations;

namespace DandyDotnet.Validation.Abstractions;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class ValueAttribute : ValidationAttribute
{
    public object? Min { get; init; }
    public object? Max { get; init; }
    public bool AllowEquals { get; init; } = true;

    public override bool IsValid(object? value)
    {
        var min = Min as IComparable;
        if (Min != null && Min is not IComparable)
            throw new ArgumentException($"Min has to implement '{typeof(IComparable)}'", nameof(Min));

        var max = Max as IComparable;
        if (Max != null && Max is not IComparable)
            throw new ArgumentException($"Max has to implement '{typeof(IComparable)}'", nameof(Max));

        var comparableValue = value as IComparable;
        if (value != null && value is not IComparable)
            throw new ArgumentException($"Value has to implement '{typeof(IComparable)}'", nameof(value));

        if (min != null)
        {
            if (comparableValue == null)
                return false;

            var minResult = min.CompareTo(comparableValue);

            // Equal is not allowed, and values are equal, OR the value is smaller than the minimum.
            if (!AllowEquals && minResult == 0 || minResult > 0)
                return false;
        }

        if (max != null)
        {
            if (comparableValue == null)
                return false;

            var maxResult = max.CompareTo(comparableValue);

            // Equal is not allowed, and values are equal, OR the value is larger than the maximum.
            if (!AllowEquals && maxResult == 0 || maxResult < 0)
                return false;
        }

        return true;
    }
}