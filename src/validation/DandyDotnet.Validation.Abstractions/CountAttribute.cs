using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DandyDotnet.Validation.Abstractions;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property)]
public sealed class CountAttribute : ValidationAttribute
{
    public int Min { get; init; } = -1;
    public int Max { get; init; } = -1;
    public int Count { get; init; } = -1;
    public bool AllowEquals { get; init; } = true;
    public bool AllowNullItems { get; init; } = true;

    public override bool IsValid(object? value)
    {
        if (value is not IEnumerable enumerable)
            throw new ArgumentException($"Value has to implement '{typeof(IEnumerable)}'", nameof(value));

        var items = enumerable.Cast<object?>();
        if (!AllowNullItems)
            items = items.Where(i => i != null);

        var count = items.Count();
        if (Count > -1)
            return count == Count;

        if (Min > -1)
        {
            if (!AllowEquals && Min == count || Min < count)
                return false;
        }

        if (Max > -1)
        {
            if (!AllowEquals && Max == count || Max < count)
                return false;
        }

        return true;
    }
}