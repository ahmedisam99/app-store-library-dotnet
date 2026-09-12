using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The links to the related data and the relationship's self-link.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/relationshiplinks"/>
public sealed class RelationshipLinks
{
    /// <summary>
    /// The link to the relationship itself, which you use to read and modify the relationship.
    /// </summary>
    [JsonPropertyName("self")]
    public string? Self { get; set; }

    /// <summary>
    /// The link to the related resources.
    /// </summary>
    [JsonPropertyName("related")]
    public string? Related { get; set; }
}
