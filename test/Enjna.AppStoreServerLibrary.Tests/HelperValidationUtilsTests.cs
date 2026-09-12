using System;
using Xunit;

namespace Enjna.AppStoreServerLibrary.Tests;

public class HelperValidationUtilsTests
{
    [Fact]
    public void ValidateDescription_AcceptsValidInput()
    {
        Assert.True(HelperValidationUtils.ValidateDescription("Valid description"));
    }

    [Fact]
    public void ValidateDescription_AcceptsAtMaxLength()
    {
        Assert.True(HelperValidationUtils.ValidateDescription(new string('A', 45)));
    }

    [Fact]
    public void ValidateDescription_RejectsAboveMaxLength()
    {
        Assert.False(HelperValidationUtils.ValidateDescription(new string('A', 46)));
    }

    [Fact]
    public void ValidateDescription_RejectsNull()
    {
        Assert.False(HelperValidationUtils.ValidateDescription(null));
    }

    [Fact]
    public void ValidateDisplayName_AcceptsValidInput()
    {
        Assert.True(HelperValidationUtils.ValidateDisplayName("Valid Name"));
    }

    [Fact]
    public void ValidateDisplayName_AcceptsAtMaxLength()
    {
        Assert.True(HelperValidationUtils.ValidateDisplayName(new string('A', 30)));
    }

    [Fact]
    public void ValidateDisplayName_RejectsAboveMaxLength()
    {
        Assert.False(HelperValidationUtils.ValidateDisplayName(new string('A', 31)));
    }

    [Fact]
    public void ValidateDisplayName_RejectsNull()
    {
        Assert.False(HelperValidationUtils.ValidateDisplayName(null));
    }

    [Fact]
    public void ValidateSku_AcceptsValidInput()
    {
        Assert.True(HelperValidationUtils.ValidateSku("valid.sku.123"));
    }

    [Fact]
    public void ValidateSku_AcceptsAtMaxLength()
    {
        Assert.True(HelperValidationUtils.ValidateSku(new string('A', 128)));
    }

    [Fact]
    public void ValidateSku_RejectsAboveMaxLength()
    {
        Assert.False(HelperValidationUtils.ValidateSku(new string('A', 129)));
    }

    [Fact]
    public void ValidateSku_RejectsNull()
    {
        Assert.False(HelperValidationUtils.ValidateSku(null));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(12)]
    public void ValidatePeriodCount_AcceptsValidValues(int value)
    {
        Assert.True(HelperValidationUtils.ValidatePeriodCount(value));
    }

    [Fact]
    public void ValidatePeriodCount_RejectsBelowMinimum()
    {
        Assert.False(HelperValidationUtils.ValidatePeriodCount(0));
    }

    [Fact]
    public void ValidatePeriodCount_RejectsAboveMaximum()
    {
        Assert.False(HelperValidationUtils.ValidatePeriodCount(13));
    }

    [Fact]
    public void ValidatePeriodCount_RejectsNull()
    {
        Assert.False(HelperValidationUtils.ValidatePeriodCount(null));
    }

    [Fact]
    public void ValidateItems_AcceptsNonEmptyList()
    {
        var items = new[] { new object() };
        Assert.True(HelperValidationUtils.ValidateItems(items));
    }

    [Fact]
    public void ValidateItems_RejectsNull()
    {
        Assert.False(HelperValidationUtils.ValidateItems<object>(null));
    }

    [Fact]
    public void ValidateItems_RejectsEmptyArray()
    {
        Assert.False(HelperValidationUtils.ValidateItems(Array.Empty<object>()));
    }

    [Fact]
    public void ValidateItems_RejectsArrayWithNullElement()
    {
        Assert.False(HelperValidationUtils.ValidateItems(new object?[] { null }));
    }
}
