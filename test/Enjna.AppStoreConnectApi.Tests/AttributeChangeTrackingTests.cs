using System;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreConnectApi.Models;
using Enjna.AppStoreConnectApi.Models.Enums;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class AttributeChangeTrackingTests
{
    private const string IntroductoryOfferResponse =
        "{\"data\":{\"type\":\"subscriptionIntroductoryOffers\",\"id\":\"6446075100\",\"attributes\":{}},"
        + "\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/subscriptionIntroductoryOffers/6446075100\"}}";

    private const string SubscriptionResponse =
        "{\"data\":{\"type\":\"subscriptions\",\"id\":\"6446075067\",\"attributes\":{}},"
        + "\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v1/subscriptions/6446075067\"}}";

    [Fact]
    public async Task SendsAnExplicitNullForAnAttributeAssignedNull()
    {
        var attributes = new SubscriptionIntroductoryOfferUpdateRequestDataAttributes
        {
            EndDate = null
        };

        var endDate = await PatchIntroductoryOfferAsync(attributes);

        Assert.Equal(JsonValueKind.Null, endDate!.Value.ValueKind);
    }

    [Fact]
    public async Task LeavesAnAttributeOutOfTheRequestWhenItIsNeverAssigned()
    {
        var attributes = new SubscriptionIntroductoryOfferUpdateRequestDataAttributes();

        Assert.Null(await PatchIntroductoryOfferAsync(attributes));
    }

    [Fact]
    public async Task SendsTheValueOfAnAttributeAssignedAValue()
    {
        var attributes = new SubscriptionIntroductoryOfferUpdateRequestDataAttributes
        {
            EndDate = new DateOnly(2026, 3, 31)
        };

        var endDate = await PatchIntroductoryOfferAsync(attributes);

        Assert.Equal("2026-03-31", endDate!.Value.GetString());
    }

    [Fact]
    public async Task ClearsOnlyTheAttributesAssignedNullAndLeavesTheRestAlone()
    {
        var (client, handler) = TestUtilities.GetClientWithBody(SubscriptionResponse);

        using (client)
        {
            await client.UpdateSubscriptionAsync(
                "6446075067",
                new SubscriptionUpdateRequest
                {
                    Data = new SubscriptionUpdateRequestData
                    {
                        Id = "6446075067",
                        Attributes = new SubscriptionUpdateRequestDataAttributes
                        {
                            Name = "Pro Monthly",
                            ReviewNote = null
                        }
                    }
                },
                TestContext.Current.CancellationToken);
        }

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);
        var attributes = body.RootElement.GetProperty("data").GetProperty("attributes");

        Assert.Equal("Pro Monthly", attributes.GetProperty("name").GetString());
        Assert.Equal(JsonValueKind.Null, attributes.GetProperty("reviewNote").ValueKind);

        Assert.False(attributes.TryGetProperty("familySharable", out _));
        Assert.False(attributes.TryGetProperty("subscriptionPeriod", out _));
        Assert.False(attributes.TryGetProperty("groupLevel", out _));
    }

    [Fact]
    public void RecordsAssignmentRatherThanValue()
    {
        var attributes = new SubscriptionUpdateRequestDataAttributes
        {
            Name = "Pro Monthly",
            ReviewNote = null
        };

        Assert.True(attributes.IsAssigned(nameof(attributes.Name)));
        Assert.True(attributes.IsAssigned(nameof(attributes.ReviewNote)));
        Assert.False(attributes.IsAssigned(nameof(attributes.GroupLevel)));

        Assert.Equal(
            new[] { nameof(attributes.Name), nameof(attributes.ReviewNote) },
            attributes.AssignedAttributes);
    }

    [Fact]
    public void ReadsBackEveryAssignedValueThroughItsProperty()
    {
        var attributes = new WinBackOfferUpdateRequestDataAttributes
        {
            StartDate = new DateOnly(2026, 1, 15),
            EndDate = null,
            CustomerEligibilityWaitBetweenOffersInMonths = 3
        };

        Assert.Equal(new DateOnly(2026, 1, 15), attributes.StartDate);
        Assert.Null(attributes.EndDate);
        Assert.Equal(3, attributes.CustomerEligibilityWaitBetweenOffersInMonths);
        Assert.Null(attributes.Priority);
    }

    [Fact]
    public void TreatsAnAttributeReadFromJsonAsAssigned()
    {
        var attributes = JsonSerializer.Deserialize<SubscriptionUpdateRequestDataAttributes>(
            "{\"name\":\"Pro Monthly\",\"reviewNote\":null}",
            AppStoreConnectAPIClient.JsonOptions)!;

        Assert.True(attributes.IsAssigned(nameof(attributes.ReviewNote)));
        Assert.False(attributes.IsAssigned(nameof(attributes.GroupLevel)));

        Assert.Equal(
            "{\"name\":\"Pro Monthly\",\"reviewNote\":null}",
            JsonSerializer.Serialize(attributes, AppStoreConnectAPIClient.JsonOptions));
    }

    [Fact]
    public void LeavesCreateAttributesOmittingTheirNullsAsBefore()
    {
        var attributes = new SubscriptionCreateRequestDataAttributes
        {
            Name = "Pro Monthly",
            ProductId = "com.example.pro.monthly",
            SubscriptionPeriod = SubscriptionPeriod.OneMonth,
            ReviewNote = null
        };

        var body = JsonSerializer.Serialize(attributes, AppStoreConnectAPIClient.JsonOptions);

        Assert.DoesNotContain("reviewNote", body);
    }

    private static async Task<JsonElement?> PatchIntroductoryOfferAsync(
        SubscriptionIntroductoryOfferUpdateRequestDataAttributes attributes)
    {
        var (client, handler) = TestUtilities.GetClientWithBody(IntroductoryOfferResponse);

        using (client)
        {
            await client.UpdateSubscriptionIntroductoryOfferAsync(
                "6446075100",
                new SubscriptionIntroductoryOfferUpdateRequest
                {
                    Data = new SubscriptionIntroductoryOfferUpdateRequestData
                    {
                        Id = "6446075100",
                        Attributes = attributes
                    }
                },
                TestContext.Current.CancellationToken);
        }

        using var body = JsonDocument.Parse(handler.CapturedRequestBody!);

        return body.RootElement.GetProperty("data").GetProperty("attributes")
            .TryGetProperty("endDate", out var endDate)
            ? endDate.Clone()
            : null;
    }
}
