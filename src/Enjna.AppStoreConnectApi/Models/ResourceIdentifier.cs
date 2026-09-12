using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A reference to another resource, identified by its type and its ID.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi"/>
public sealed class ResourceIdentifier
{
    /// <summary>
    /// The resource type of the referenced resource, such as <c>apps</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    /// <summary>
    /// The opaque resource ID that uniquely identifies the referenced resource.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
}
