using DandyDotnet.Validation.Abstractions;

namespace DandyDotnet.Validation.Tests.Attributes;

public class CountAttributeTests
{
    [Fact]
    public void Count_EqualCount_ReturnsTrue()
    {
        var attribute = new CountAttribute { Count = 3 };
        Assert.True(attribute.IsValid(new[] { 1, 2, 3 }));
    }

    [Fact]
    public void Count_NotEqualCount_ReturnsFalse()
    {
        var attribute = new CountAttribute { Count = 3 };
        Assert.False(attribute.IsValid(new[] { 1, 2, 3, 4 }));
    }

    [Fact]
    public void Count_PrecedesMinOrMax()
    {
        var attribute = new CountAttribute { Count = 3, Min = 2, Max = 10 };
        Assert.True(attribute.IsValid(new[] { 1, 2, 3 }));
        Assert.False(attribute.IsValid(new[] { 1, 2 }));
    }

    [Fact]
    public void Count_AllowNullItems_DoesntFilterItems()
    {
        var attribute = new CountAttribute { Count = 5, AllowNullItems = true };
        Assert.True(attribute.IsValid(new object?[] { 1, null, 3, null, 5 }));
    }

    [Fact]
    public void Count_DontAllowNullItems_FiltersItems()
    {
        var attribute = new CountAttribute { Count = 3, AllowNullItems = false };
        Assert.True(attribute.IsValid(new object?[] { 1, null, 3, null, 5 }));
    }

    [Fact]
    public void Min_GreaterCount_ReturnsTrue()
    {
        var attribute = new CountAttribute { Min = 5 };
        Assert.True(attribute.IsValid(new[] { 1, 2, 3, 4, 5, 6, 7 }));
    }

    [Fact]
    public void MinAndEquals_EqualCount_ReturnsTrue()
    {
        var attribute = new CountAttribute { Min = 5, AllowEquals = true };
        Assert.True(attribute.IsValid(new[] { 1, 2, 3, 4, 5 }));
    }

    [Fact]
    public void MinAndNotEquals_EqualCount_ReturnsFalse()
    {
        var attribute = new CountAttribute { Min = 5, AllowEquals = false };
        Assert.False(attribute.IsValid(new[] { 1, 2, 3, 4, 5 }));
    }

    [Fact]
    public void Min_LowerCount_ReturnsFalse()
    {
        var attribute = new CountAttribute { Min = 5 };
        Assert.False(attribute.IsValid(new[] { 1, 2, 3, 4 }));
    }

    [Fact]
    public void Max_GreaterCount_ReturnsFalse()
    {
        var attribute = new CountAttribute { Max = 5 };
        Assert.False(attribute.IsValid(new[] { 1, 2, 3, 4, 5, 6 }));
    }

    [Fact]
    public void MaxAndEquals_EqualCount_ReturnsTrue()
    {
        var attribute = new CountAttribute { Max = 5, AllowEquals = true };
        Assert.True(attribute.IsValid(new[] { 1, 2, 3, 4, 5 }));
    }

    [Fact]
    public void MaxAndNotEquals_EqualCount_ReturnsFalse()
    {
        var attribute = new CountAttribute { Max = 5, AllowEquals = false };
        Assert.False(attribute.IsValid(new[] { 1, 2, 3, 4, 5 }));
    }

    [Fact]
    public void Max_LowerCount_ReturnsTrue()
    {
        var attribute = new CountAttribute { Max = 5 };
        Assert.True(attribute.IsValid(new[] { 1, 2, 3, 4 }));
    }

    [Fact]
    public void NotEnumerableValue_ThrowsArgumentException()
    {
        var attribute = new CountAttribute { Count = 3 };
        Assert.Throws<ArgumentException>(() => attribute.IsValid(123));
    }
}