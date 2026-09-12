using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update a Builds resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/buildupdaterequest"/>
public sealed class BuildUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required BuildUpdateRequestData Data { get; set; }
}
