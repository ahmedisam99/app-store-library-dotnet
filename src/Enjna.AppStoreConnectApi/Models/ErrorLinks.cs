using System.Text.Json;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// Links related to an error, providing more information about it.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/errorlinks"/>
public sealed class ErrorLinks
{
    /// <summary>
    /// A link to documentation that explains the error.
    /// </summary>
    [JsonPropertyName("about")]
    public string? About { get; set; }

    /// <summary>
    /// A link to an associated resource. Apple sends either a URI string or an object that holds a
    /// <c>href</c> and a <c>meta</c> member, so the raw JSON is preserved here.
    /// </summary>
    [JsonPropertyName("associated")]
    public JsonElement? Associated { get; set; }
}
