using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data element of the request body that updates an Apps resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/appupdaterequest/data"/>
public sealed class AppUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>apps</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "apps";

    /// <summary>
    /// The opaque resource ID of the app to update. This member is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes to change on the app. Leave an attribute out to keep its current value.
    /// </summary>
    [JsonPropertyName("attributes")]
    public AppUpdateRequestDataAttributes? Attributes { get; set; }
}
