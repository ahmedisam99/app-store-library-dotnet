using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to change whether the App Store shows an app tag for an app.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/apptagupdaterequest"/>
public sealed class AppTagUpdateRequest
{
    /// <summary>
    /// The resource data. This value is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required AppTagUpdateRequestData Data { get; set; }
}
