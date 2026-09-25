using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that reserves a Subscription Images resource of the v2 API.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagev2createrequest/data/attributes"/>
public sealed class SubscriptionImageV2CreateRequestDataAttributes
{
    /// <summary>
    /// The size of the image file, in bytes. App Store Connect uses it to decide how to split the
    /// upload. This attribute is required.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public required long FileSize { get; set; }

    /// <summary>
    /// The name of the image file you are about to upload, including its extension. This attribute
    /// is required.
    /// </summary>
    [JsonPropertyName("fileName")]
    public required string FileName { get; set; }
}
