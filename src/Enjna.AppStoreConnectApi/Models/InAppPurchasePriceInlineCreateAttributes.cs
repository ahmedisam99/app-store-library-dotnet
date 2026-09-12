using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of a price you create inline in a price schedule request.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasepriceinlinecreate/attributes"/>
public sealed class InAppPurchasePriceInlineCreateAttributes
{
    /// <summary>
    /// The date the price takes effect. Leave it <c>null</c> for the price the schedule starts
    /// from, which the App Store charges right away.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonPropertyName("startDate")]
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// The last date the price applies on. Leave it unset to keep the price in effect until the
    /// next scheduled price change.
    /// </summary>
    /// <remarks>
    /// Leaving this <c>null</c> leaves the attribute out of the request, which is the shape Apple's
    /// own example sends. Apple documents no meaning for an explicit <c>null</c> end date, so the
    /// library doesn't send one.
    /// </remarks>
    [JsonPropertyName("endDate")]
    public DateOnly? EndDate { get; set; }
}
