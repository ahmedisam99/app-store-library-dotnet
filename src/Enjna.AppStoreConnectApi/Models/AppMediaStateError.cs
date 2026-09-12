using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A problem App Store Connect found while it processed an uploaded media asset.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/appmediastateerror"/>
public sealed class AppMediaStateError
{
    /// <summary>
    /// The code that identifies the kind of problem.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    /// A human-readable explanation of the problem.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}
