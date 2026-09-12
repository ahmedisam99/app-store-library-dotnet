using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Prerelease Versions resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/prereleaseversion/attributes"/>
public sealed class PrereleaseVersionAttributes
{
    /// <summary>
    /// The version number of the pre-release version, such as <c>1.4</c>.
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// The platform the pre-release version's builds target.
    /// </summary>
    [JsonPropertyName("platform")]
    public Platform? Platform { get; set; }
}
