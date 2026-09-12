using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Build Beta Details resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/buildbetadetail/attributes"/>
public sealed class BuildBetaDetailAttributes
{
    /// <summary>
    /// A Boolean value that indicates whether testers are notified automatically once the build
    /// becomes available to them.
    /// </summary>
    [JsonPropertyName("autoNotifyEnabled")]
    public bool? AutoNotifyEnabled { get; set; }

    /// <summary>
    /// The state of the build for internal testing, which needs no beta review.
    /// </summary>
    [JsonPropertyName("internalBuildState")]
    public InternalBetaState? InternalBuildState { get; set; }

    /// <summary>
    /// The state of the build for external testing, which Apple reviews first.
    /// </summary>
    [JsonPropertyName("externalBuildState")]
    public ExternalBetaState? ExternalBuildState { get; set; }
}
