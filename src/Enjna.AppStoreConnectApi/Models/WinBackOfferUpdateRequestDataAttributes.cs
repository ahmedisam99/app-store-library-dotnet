using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that changes a win-back offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackofferupdaterequest/data/attributes"/>
public sealed class WinBackOfferUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The number of months a customer must have paid for the subscription before their lapse to
    /// qualify for the offer.
    /// </summary>
    [JsonPropertyName("customerEligibilityPaidSubscriptionDurationInMonths")]
    public int? CustomerEligibilityPaidSubscriptionDurationInMonths { get => Get<int?>(); set => Set(value); }

    /// <summary>
    /// The window, in months since the customer's subscription lapsed, within which they qualify
    /// for the offer.
    /// </summary>
    [JsonPropertyName("customerEligibilityTimeSinceLastSubscribedInMonths")]
    public IntegerRange? CustomerEligibilityTimeSinceLastSubscribedInMonths { get => Get<IntegerRange?>(); set => Set(value); }

    /// <summary>
    /// The number of months that must pass before a customer who declined the offer can receive it
    /// again.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("customerEligibilityWaitBetweenOffersInMonths")]
    public int? CustomerEligibilityWaitBetweenOffersInMonths { get => Get<int?>(); set => Set(value); }

    /// <summary>
    /// The day the offer starts.
    /// </summary>
    [JsonPropertyName("startDate")]
    public DateOnly? StartDate { get => Get<DateOnly?>(); set => Set(value); }

    /// <summary>
    /// The last day the offer is available.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get => Get<DateOnly?>(); set => Set(value); }

    /// <summary>
    /// Which offer wins when a customer qualifies for more than one win-back offer at a time.
    /// </summary>
    [JsonPropertyName("priority")]
    public WinBackOfferPriority? Priority { get => Get<WinBackOfferPriority?>(); set => Set(value); }

    /// <summary>
    /// Whether the App Store promotes the offer for you, or leaves presenting it to your app.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("promotionIntent")]
    public WinBackOfferPromotionIntent? PromotionIntent { get => Get<WinBackOfferPromotionIntent?>(); set => Set(value); }
}
