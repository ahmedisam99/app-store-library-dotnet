using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update a beta group.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betagroupupdaterequest"/>
public sealed class BetaGroupUpdateRequest
{
    /// <summary>
    /// The resource data. This property is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required BetaGroupUpdateRequestData Data { get; set; }
}
