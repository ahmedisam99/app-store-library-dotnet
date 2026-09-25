using System;
using System.Text.Json;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class ExplicitNullSerializationTests
{
    private const string ImmediateInAppPurchasePrice = "{\"startDate\":null}";

    private const string ScheduledInAppPurchasePrice = "{\"startDate\":\"2026-03-01\"}";

    private const string ImmediateInAppPurchasePriceThatEnds = "{\"startDate\":null,\"endDate\":\"2026-03-31\"}";

    private const string ScheduledInAppPurchasePriceThatEnds = "{\"startDate\":\"2026-03-01\",\"endDate\":\"2026-03-31\"}";

    private const string ImmediateSubscriptionPrice = "{\"startDate\":null}";

    private const string ImmediateMonthlySubscriptionPrice = "{\"startDate\":null,\"planType\":\"MONTHLY\"}";

    private const string ScheduledSubscriptionPrice = "{\"startDate\":\"2026-01-15\",\"preserveCurrentPrice\":true}";

    private const string PriceScheduleIncludedEntry =
        "{\"type\":\"inAppPurchasePrices\",\"id\":\"${price1}\",\"attributes\":{\"startDate\":null},"
        + "\"relationships\":{"
        + "\"inAppPurchasePricePoint\":{\"data\":{\"type\":\"inAppPurchasePricePoints\",\"id\":\"NjQ0NjQ1MjYxNV91c181\"}},"
        + "\"inAppPurchaseV2\":{\"data\":{\"type\":\"inAppPurchases\",\"id\":\"6446452615\"}}}}";

    [Fact]
    public void SendsAnExplicitNullStartDateForAnInAppPurchasePriceThatTakesEffectImmediately()
    {
        var attributes = new InAppPurchasePriceInlineCreateAttributes();

        Assert.Equal(ImmediateInAppPurchasePrice, Serialize(attributes));
    }

    [Fact]
    public void SendsTheStartDateOfAScheduledInAppPurchasePrice()
    {
        var attributes = new InAppPurchasePriceInlineCreateAttributes
        {
            StartDate = new DateOnly(2026, 3, 1)
        };

        Assert.Equal(ScheduledInAppPurchasePrice, Serialize(attributes));
    }

    [Fact]
    public void LeavesTheInAppPurchasePriceEndDateOutOfTheRequestWhenItIsNull()
    {
        var attributes = new InAppPurchasePriceInlineCreateAttributes
        {
            StartDate = new DateOnly(2026, 3, 1)
        };

        Assert.DoesNotContain("endDate", Serialize(attributes));
    }

    [Fact]
    public void SendsTheInAppPurchasePriceEndDateWhenItIsSet()
    {
        var immediate = new InAppPurchasePriceInlineCreateAttributes
        {
            EndDate = new DateOnly(2026, 3, 31)
        };

        var scheduled = new InAppPurchasePriceInlineCreateAttributes
        {
            StartDate = new DateOnly(2026, 3, 1),
            EndDate = new DateOnly(2026, 3, 31)
        };

        Assert.Equal(ImmediateInAppPurchasePriceThatEnds, Serialize(immediate));
        Assert.Equal(ScheduledInAppPurchasePriceThatEnds, Serialize(scheduled));
    }

    [Fact]
    public void WritesThePriceScheduleEntryApplesExampleSends()
    {
        var price = new InAppPurchasePriceInlineCreate
        {
            Id = "${price1}",
            Attributes = new InAppPurchasePriceInlineCreateAttributes(),
            Relationships = new InAppPurchasePriceInlineCreateRelationships
            {
                InAppPurchasePricePoint = RelationshipDeclaration.To(
                    "inAppPurchasePricePoints",
                    "NjQ0NjQ1MjYxNV91c181"),
                InAppPurchaseV2 = RelationshipDeclaration.To("inAppPurchases", "6446452615")
            }
        };

        Assert.Equal(PriceScheduleIncludedEntry, Serialize(price));
    }

    [Fact]
    public void SendsAnExplicitNullStartDateForASubscriptionPriceThatTakesEffectImmediately()
    {
        var created = new SubscriptionPriceCreateRequestDataAttributes();
        var inline = new SubscriptionPriceInlineCreateAttributes();

        Assert.Equal(ImmediateSubscriptionPrice, Serialize(created));
        Assert.Equal(ImmediateSubscriptionPrice, Serialize(inline));
    }

    [Fact]
    public void SendsTheSubscriptionPriceAttributesApplesExampleSends()
    {
        var created = new SubscriptionPriceCreateRequestDataAttributes
        {
            PlanType = SubscriptionPlanType.Monthly
        };

        var inline = new SubscriptionPriceInlineCreateAttributes
        {
            PlanType = SubscriptionPlanType.Monthly
        };

        Assert.Equal(ImmediateMonthlySubscriptionPrice, Serialize(created));
        Assert.Equal(ImmediateMonthlySubscriptionPrice, Serialize(inline));
    }

    [Fact]
    public void SendsTheStartDateOfAScheduledSubscriptionPrice()
    {
        var created = new SubscriptionPriceCreateRequestDataAttributes
        {
            StartDate = new DateOnly(2026, 1, 15),
            PreserveCurrentPrice = true
        };

        var inline = new SubscriptionPriceInlineCreateAttributes
        {
            StartDate = new DateOnly(2026, 1, 15),
            PreserveCurrentPrice = true
        };

        Assert.Equal(ScheduledSubscriptionPrice, Serialize(created));
        Assert.Equal(ScheduledSubscriptionPrice, Serialize(inline));
    }

    // The rest of this class guards the members an update request leaves out. Update attributes
    // track assignment, so a member nobody assigned stays off the wire and Apple keeps the value it
    // has. Assigning null is the separate, deliberate way to clear one.

    [Fact]
    public void KeepsTheIntroductoryOfferEndDateOutOfAnUpdateRequest()
    {
        var attributes = new SubscriptionIntroductoryOfferUpdateRequestDataAttributes();

        Assert.Equal("{}", Serialize(attributes));
    }

    [Fact]
    public void KeepsAWinBackOfferUpdateFreeOfNullAttributes()
    {
        var attributes = new WinBackOfferUpdateRequestDataAttributes
        {
            Priority = WinBackOfferPriority.High
        };

        Assert.Equal("{\"priority\":\"HIGH\"}", Serialize(attributes));
    }

    [Fact]
    public void KeepsTheReviewNoteOutOfAnInAppPurchaseUpdateRequestWhenNobodyAssignedIt()
    {
        var attributes = new InAppPurchaseV2UpdateRequestDataAttributes
        {
            Name = "Pro Unlock"
        };

        Assert.Equal("{\"name\":\"Pro Unlock\"}", Serialize(attributes));
    }

#pragma warning disable CS0618 // Apple deprecated the v1 localization request in 4.4.1; the library still sends it.
    [Fact]
    public void KeepsTheLocalizationDescriptionOutOfAnUpdateRequestWhenNobodyAssignedIt()
    {
        var attributes = new InAppPurchaseLocalizationUpdateRequestDataAttributes
        {
            Name = "Pro Unlock"
        };

        Assert.Equal("{\"name\":\"Pro Unlock\"}", Serialize(attributes));
    }
#pragma warning restore CS0618

    [Fact]
    public void StillClearsARelationshipThroughAnExplicitNullLinkage()
    {
        var declaration = new RelationshipDeclaration { Data = null };

        Assert.Equal("{\"data\":null}", Serialize(declaration));
    }

    private static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, AppStoreConnectAPIClient.JsonOptions);
    }
}
