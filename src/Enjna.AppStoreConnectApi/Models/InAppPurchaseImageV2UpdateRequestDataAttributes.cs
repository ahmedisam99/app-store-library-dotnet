using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that commits a v2 In-App Purchase Images resource. Unlike the
/// v1 image, the commit carries no checksum.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimagev2updaterequest/data/attributes"/>
public sealed class InAppPurchaseImageV2UpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// Set this to <c>true</c> once you send every upload operation, to tell App Store Connect the
    /// file is complete.
    /// </summary>
    [JsonPropertyName("uploaded")]
    public bool? Uploaded { get => Get<bool?>(); set => Set(value); }
}
