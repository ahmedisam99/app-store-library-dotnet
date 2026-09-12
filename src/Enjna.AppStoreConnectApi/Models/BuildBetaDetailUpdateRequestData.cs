using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data of a request that updates a Build Beta Details resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/buildbetadetailupdaterequest/data"/>
public sealed class BuildBetaDetailUpdateRequestData
{
    /// <summary>
    /// The resource type. The value is always <c>buildBetaDetails</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "buildBetaDetails";

    /// <summary>
    /// The opaque resource ID of the build beta detail to update. This value is required.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The attributes that describe the request that updates a Build Beta Details resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public BuildBetaDetailUpdateRequestDataAttributes? Attributes { get; set; }
}
