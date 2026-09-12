using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to update an Apps resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/appupdaterequest"/>
public sealed class AppUpdateRequest
{
    /// <summary>
    /// The resource data. This member is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required AppUpdateRequestData Data { get; set; }
}
