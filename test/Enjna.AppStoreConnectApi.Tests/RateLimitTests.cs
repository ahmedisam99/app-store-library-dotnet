using Enjna.AppStoreConnectApi.Models;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class RateLimitTests
{
    /// <summary>
    /// The header <c>GET /v1/apps</c> answered with, measured against Apple.
    /// </summary>
    private const string HourlyHeader = "user-hour-lim:3600;user-hour-rem:3577;";

    /// <summary>
    /// The header <c>POST /v1/inAppPurchasePriceSchedules</c> answered with, measured against
    /// Apple. A write reports a per-minute budget and no hourly figure at all.
    /// </summary>
    private const string PerMinuteHeader = "user-minute-lim:250;user-minute-rem:244;";

    private const string BothBudgetsHeader =
        "user-hour-lim:3600;user-hour-rem:3577;user-minute-lim:250;user-minute-rem:244;";

    /// <summary>
    /// A header carrying a key Apple doesn't document today and this library has never seen.
    /// </summary>
    private const string UnknownKeyHeader = "user-hour-lim:3600;user-hour-rem:3577;user-day-lim:50000;";

    [Fact]
    public void ReadsTheHourlyBudgetAReadAnswersWith()
    {
        var rateLimit = RateLimit.Parse(HourlyHeader);

        Assert.NotNull(rateLimit);
        Assert.Equal(3600, rateLimit.UserHourLimit);
        Assert.Equal(3577, rateLimit.UserHourRemaining);
        Assert.Equal(HourlyHeader, rateLimit.Raw);
    }

    [Fact]
    public void ReadsThePerMinuteBudgetAWriteAnswersWith()
    {
        var rateLimit = RateLimit.Parse(PerMinuteHeader);

        Assert.NotNull(rateLimit);
        Assert.Equal(250, rateLimit.UserMinuteLimit);
        Assert.Equal(244, rateLimit.UserMinuteRemaining);
        Assert.Equal(PerMinuteHeader, rateLimit.Raw);
    }

    [Fact]
    public void ReportsNoPerMinuteBudgetWhenTheHeaderCarriesOnlyTheHourlyOne()
    {
        var rateLimit = RateLimit.Parse(HourlyHeader);

        Assert.NotNull(rateLimit);
        Assert.Null(rateLimit.UserMinuteLimit);
        Assert.Null(rateLimit.UserMinuteRemaining);
        Assert.DoesNotContain("user-minute-lim", rateLimit.Fields.Keys);
        Assert.DoesNotContain("user-minute-rem", rateLimit.Fields.Keys);
    }

    [Fact]
    public void ReportsNoHourlyBudgetWhenTheHeaderCarriesOnlyThePerMinuteOne()
    {
        var rateLimit = RateLimit.Parse(PerMinuteHeader);

        Assert.NotNull(rateLimit);
        Assert.Null(rateLimit.UserHourLimit);
        Assert.Null(rateLimit.UserHourRemaining);
        Assert.DoesNotContain("user-hour-lim", rateLimit.Fields.Keys);
        Assert.DoesNotContain("user-hour-rem", rateLimit.Fields.Keys);
    }

    [Fact]
    public void ReadsBothBudgetsWhenTheHeaderCarriesThem()
    {
        var rateLimit = RateLimit.Parse(BothBudgetsHeader);

        Assert.NotNull(rateLimit);
        Assert.Equal(3600, rateLimit.UserHourLimit);
        Assert.Equal(3577, rateLimit.UserHourRemaining);
        Assert.Equal(250, rateLimit.UserMinuteLimit);
        Assert.Equal(244, rateLimit.UserMinuteRemaining);
        Assert.Equal(4, rateLimit.Fields.Count);
    }

    [Fact]
    public void ReadsEveryTypedBudgetOutOfTheSameFieldsItExposes()
    {
        var rateLimit = RateLimit.Parse(BothBudgetsHeader);

        Assert.NotNull(rateLimit);
        Assert.Equal(rateLimit.Fields["user-hour-lim"], rateLimit.UserHourLimit);
        Assert.Equal(rateLimit.Fields["user-hour-rem"], rateLimit.UserHourRemaining);
        Assert.Equal(rateLimit.Fields["user-minute-lim"], rateLimit.UserMinuteLimit);
        Assert.Equal(rateLimit.Fields["user-minute-rem"], rateLimit.UserMinuteRemaining);
    }

    [Fact]
    public void ReachesTheCallerWithAKeyThisLibraryHasNeverSeen()
    {
        var rateLimit = RateLimit.Parse(UnknownKeyHeader);

        Assert.NotNull(rateLimit);
        Assert.Equal(50000, rateLimit.Fields["user-day-lim"]);
    }

    [Fact]
    public void KeepsTheKnownBudgetsWhenTheHeaderCarriesAKeyThisLibraryHasNeverSeen()
    {
        var rateLimit = RateLimit.Parse(UnknownKeyHeader);

        Assert.NotNull(rateLimit);
        Assert.Equal(3600, rateLimit.UserHourLimit);
        Assert.Equal(3577, rateLimit.UserHourRemaining);
        Assert.Equal(3, rateLimit.Fields.Count);
    }

    [Fact]
    public void SkipsASegmentWithoutASeparator()
    {
        var rateLimit = RateLimit.Parse("user-hour-lim:3600;nonsense;user-hour-rem:3577;");

        Assert.NotNull(rateLimit);
        Assert.Equal(3600, rateLimit.UserHourLimit);
        Assert.Equal(3577, rateLimit.UserHourRemaining);
        Assert.Equal(2, rateLimit.Fields.Count);
    }

    [Fact]
    public void SkipsAFieldWhoseValueIsntAnInteger()
    {
        const string header = "user-hour-lim:unlimited;user-hour-rem:3577;";

        var rateLimit = RateLimit.Parse(header);

        Assert.NotNull(rateLimit);
        Assert.Null(rateLimit.UserHourLimit);
        Assert.Equal(3577, rateLimit.UserHourRemaining);
        Assert.DoesNotContain("user-hour-lim", rateLimit.Fields.Keys);
        Assert.Equal(header, rateLimit.Raw);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ReturnsNullForAnAbsentHeader(string? header)
    {
        Assert.Null(RateLimit.Parse(header));
    }

    [Fact]
    public void ExposesNoFieldsWhenNoSegmentIsWellFormed()
    {
        const string header = "nonsense";

        var rateLimit = RateLimit.Parse(header);

        Assert.NotNull(rateLimit);
        Assert.Empty(rateLimit.Fields);
        Assert.Equal(header, rateLimit.Raw);
    }

    [Fact]
    public void LooksUpAFieldWithoutRegardToCase()
    {
        var rateLimit = RateLimit.Parse("USER-HOUR-LIM:3600;");

        Assert.NotNull(rateLimit);
        Assert.Equal(3600, rateLimit.UserHourLimit);
        Assert.Equal(3600, rateLimit.Fields["user-hour-lim"]);
    }

    [Fact]
    public void TakesTheLastValueWhenAKeyRepeats()
    {
        var rateLimit = RateLimit.Parse("user-hour-rem:3577;user-hour-rem:3572;");

        Assert.NotNull(rateLimit);
        Assert.Equal(3572, rateLimit.UserHourRemaining);
        Assert.Single(rateLimit.Fields);
    }

    [Fact]
    public void ReadsAHeaderWithoutATrailingSeparator()
    {
        var rateLimit = RateLimit.Parse("user-hour-lim:3600;user-hour-rem:3577");

        Assert.NotNull(rateLimit);
        Assert.Equal(3600, rateLimit.UserHourLimit);
        Assert.Equal(3577, rateLimit.UserHourRemaining);
    }
}
