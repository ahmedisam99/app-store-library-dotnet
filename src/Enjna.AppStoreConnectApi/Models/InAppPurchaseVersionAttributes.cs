using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchase Versions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseversion/attributes"/>
public sealed class InAppPurchaseVersionAttributes
{
    /// <summary>
    /// The version number, such as <c>1</c>.
    /// </summary>
    [JsonPropertyName("version")]
    public int? Version { get; set; }

    /// <summary>
    /// Where the version sits in the App Review workflow. Its localizations and images can change
    /// only while it is <c>PREPARE_FOR_SUBMISSION</c>.
    /// </summary>
    [JsonPropertyName("state")]
    public InAppPurchaseVersionState? State { get; set; }
}
