using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that changes an introductory offer.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionintroductoryofferupdaterequest/data/attributes"/>
public sealed class SubscriptionIntroductoryOfferUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The last day the offer is available.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get => Get<DateOnly?>(); set => Set(value); }
}
