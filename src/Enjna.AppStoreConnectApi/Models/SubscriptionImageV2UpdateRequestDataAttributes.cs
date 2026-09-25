using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that commits a Subscription Images resource of the v2 API.
/// Unlike the deprecated v1 request, it takes no source file checksum.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagev2updaterequest/data/attributes"/>
public sealed class SubscriptionImageV2UpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// Set this to <c>true</c> to tell App Store Connect that every upload operation finished.
    /// </summary>
    [JsonPropertyName("uploaded")]
    public bool? Uploaded { get => Get<bool?>(); set => Set(value); }
}
