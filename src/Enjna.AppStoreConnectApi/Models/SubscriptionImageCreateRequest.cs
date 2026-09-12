using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to reserve a promotional image for an auto-renewable subscription. App
/// Store Connect answers with the operations that upload the file's bytes.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionimagecreaterequest"/>
public sealed class SubscriptionImageCreateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required SubscriptionImageCreateRequestData Data { get; set; }
}
