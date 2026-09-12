using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a beta group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betagroupcreaterequest"/>
public sealed class BetaGroupCreateRequest
{
    /// <summary>
    /// The resource data. This property is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required BetaGroupCreateRequestData Data { get; set; }
}
