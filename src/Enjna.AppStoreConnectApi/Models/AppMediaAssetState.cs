using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// How far along an uploaded media asset is, and anything App Store Connect flagged while it
/// processed the asset.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/appmediaassetstate"/>
public sealed class AppMediaAssetState
{
    /// <summary>
    /// The stage the asset reached.
    /// </summary>
    [JsonPropertyName("state")]
    public AppMediaAssetStateState? State { get; set; }

    /// <summary>
    /// The problems that stopped App Store Connect from accepting the asset.
    /// </summary>
    [JsonPropertyName("errors")]
    public AppMediaStateError[]? Errors { get; set; }

    /// <summary>
    /// The problems App Store Connect accepted the asset in spite of.
    /// </summary>
    [JsonPropertyName("warnings")]
    public AppMediaStateError[]? Warnings { get; set; }
}
