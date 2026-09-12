using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to change a win-back offer's schedule or its eligibility rules.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackofferupdaterequest"/>
public sealed class WinBackOfferUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required WinBackOfferUpdateRequestData Data { get; set; }
}
