using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an App Categories resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/appcategory/attributes-data.dictionary"/>
public sealed class AppCategoryAttributes
{
    /// <summary>
    /// The platforms whose App Store offers the category. An app can only claim the category on a
    /// platform this list names.
    /// </summary>
    [JsonPropertyName("platforms")]
    public Platform[]? Platforms { get; set; }
}
