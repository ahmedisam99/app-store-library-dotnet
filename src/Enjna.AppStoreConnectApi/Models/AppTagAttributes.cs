using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an App Tags resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/apptag/attributes-data.dictionary"/>
public sealed class AppTagAttributes
{
    /// <summary>
    /// The name of the tag as customers see it on the App Store.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// A Boolean value that indicates whether the App Store shows the tag on the app's product
    /// page and features the app under the tag. Set it to <c>false</c> to opt the app out.
    /// </summary>
    [JsonPropertyName("visibleInAppStore")]
    public bool? VisibleInAppStore { get; set; }
}
