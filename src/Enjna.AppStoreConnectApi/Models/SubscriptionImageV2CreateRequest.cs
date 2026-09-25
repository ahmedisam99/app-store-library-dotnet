using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to reserve a promotional image for a subscription version. App Store
/// Connect answers with the operations that upload the file's bytes.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagev2createrequest"/>
public sealed class SubscriptionImageV2CreateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionImageV2CreateRequestData Data { get; set; }
}
