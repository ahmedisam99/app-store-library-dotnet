using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to set the price schedule of an in-app purchase. It replaces the whole
/// schedule, so send every manual price you want to keep.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasepriceschedulecreaterequest"/>
public sealed class InAppPurchasePriceScheduleCreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required InAppPurchasePriceScheduleCreateRequestData Data { get; set; }

    /// <summary>
    /// The prices and territories you create along with the schedule, each identified by the same
    /// placeholder ID the relationships refer to. Every entry is an
    /// <see cref="InAppPurchasePriceInlineCreate"/> or a <see cref="TerritoryInlineCreate"/>, the
    /// two types Apple allows here, and every entry serializes by its own runtime type.
    /// </summary>
    [JsonPropertyName("included")]
    public IInAppPurchasePriceScheduleCreateRequestIncludedResource[]? Included { get; set; }
}
