using DandyDotnet.Validation.Abstractions;

namespace DandyDotnet.Validation.Tests.Attributes;

public class ValueAttributeTests
{
    [Fact]
    public void Min_GreaterValue_ReturnsTrue()
    {
        var attribute = new ValueAttribute
        {
            Min = 10,
        };

        Assert.True(attribute.IsValid(11));
    }

    [Fact]
    public void MinAndEquals_EqualValue_ReturnsTrue()
    {
        var attribute = new ValueAttribute
        {
            Min = 10,
            AllowEquals = true,
        };

        Assert.True(attribute.IsValid(10));
    }

    [Fact]
    public void MinAndNotEquals_EqualValue_ReturnsFalse()
    {
        var attribute = new ValueAttribute
        {
            Min = 10,
            AllowEquals = false,
        };

        Assert.False(attribute.IsValid(10));
    }

    [Fact]
    public void Min_LowerValue_ReturnsFalse()
    {
        var attribute = new ValueAttribute
        {
            Min = 10,
        };

        Assert.False(attribute.IsValid(9));
    }

    [Fact]
    public void Max_GreaterValue_ReturnsFalse()
    {
        var attribute = new ValueAttribute
        {
            Max = 10,
        };

        Assert.False(attribute.IsValid(11));
    }

    [Fact]
    public void MaxAndEquals_EqualValue_ReturnsTrue()
    {
        var attribute = new ValueAttribute
        {
            Max = 10,
            AllowEquals = true,
        };

        Assert.True(attribute.IsValid(10));
    }

    [Fact]
    public void MaxAndNotEquals_EqualValue_ReturnsFalse()
    {
        var attribute = new ValueAttribute
        {
            Max = 10,
            AllowEquals = false,
        };

        Assert.False(attribute.IsValid(10));
    }

    [Fact]
    public void Max_LowerValue_ReturnsTrue()
    {
        var attribute = new ValueAttribute
        {
            Max = 10,
        };

        Assert.True(attribute.IsValid(9));
    }

    [Fact]
    public void MinAndMaxNull_ReturnsTrue()
    {
        var attribute = new ValueAttribute();
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void Min_NullValue_ReturnsFalse()
    {
        var attribute = new ValueAttribute
        {
            Min = 10,
        };

        Assert.False(attribute.IsValid(null));
    }

    [Fact]
    public void Max_NullValue_ReturnsFalse()
    {
        var attribute = new ValueAttribute
        {
            Max = 10,
        };

        Assert.False(attribute.IsValid(null));
    }

    [Fact]
    public void Value_NotComparable_ThrowsArgumentException()
    {
        var attribute = new ValueAttribute();
        Assert.Throws<ArgumentException>(() => attribute.IsValid(new ValueAttribute()));
    }

    [Fact]
    public void Min_NotComparable_ThrowsArgumentException()
    {
        var attribute = new ValueAttribute()
        {
            Min = new ValueAttribute(),
        };
            
        Assert.Throws<ArgumentException>(() => attribute.IsValid(null));
    }

    [Fact]
    public void Max_NotComparable_ThrowsArgumentException()
    {
        var attribute = new ValueAttribute()
        {
            Max = new ValueAttribute(),
        };
            
        Assert.Throws<ArgumentException>(() => attribute.IsValid(null));
    }
}