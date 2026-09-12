using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request that reserves a subscription promotional image.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagecreaterequest/data/attributes"/>
public sealed class SubscriptionImageCreateRequestDataAttributes
{
    /// <summary>
    /// The size of the image file, in bytes. This value is required.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public required long FileSize { get; set; }

    /// <summary>
    /// The file name of the image you're about to upload. This value is required.
    /// </summary>
    [JsonPropertyName("fileName")]
    public required string FileName { get; set; }
}
