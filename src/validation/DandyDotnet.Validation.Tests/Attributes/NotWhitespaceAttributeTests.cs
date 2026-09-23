using DandyDotnet.Validation.Abstractions;

namespace DandyDotnet.Validation.Tests.Attributes;

public class NotWhitespaceAttributeTests
{
    [Fact]
    public void EmptyValue_ReturnsFalse()
    {
        var result = new NotWhitespaceAttribute().IsValid(string.Empty);
        Assert.False(result);
    }

    [Fact]
    public void WhitespaceValue_ReturnsFalse()
    {
        var result = new NotWhitespaceAttribute().IsValid("   ");
        Assert.False(result);
    }

    [Fact]
    public void NullValue_ReturnsFalse()
    {
        var result = new NotWhitespaceAttribute().IsValid(null);
        Assert.False(result);
    }

    [Fact]
    public void FilledValue_ReturnsTrue()
    {
        var result = new NotWhitespaceAttribute().IsValid("Hello, world!");
        Assert.True(result);
    }

    [Fact]
    public void NotStringValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new NotWhitespaceAttribute().IsValid(123));
    }
}
