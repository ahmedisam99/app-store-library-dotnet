using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The base shape of every App Store Connect API resource.
/// </summary>
/// <typeparam name="TAttributes">The type that describes the resource's attributes.</typeparam>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi"/>
public abstract class Resource<TAttributes>
{
    /// <summary>
    /// The resource type, such as <c>apps</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    /// <summary>
    /// The opaque resource ID that uniquely identifies the resource. It is unique only within its type.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// The attributes that describe the resource.
    /// </summary>
    [JsonPropertyName("attributes")]
    public TAttributes? Attributes { get; set; }

    /// <summary>
    /// The relationships to other resources, keyed by the relationship name.
    /// </summary>
    [JsonPropertyName("relationships")]
    public Dictionary<string, Relationship>? Relationships { get; set; }

    /// <summary>
    /// Navigational links that include the self-link.
    /// </summary>
    [JsonPropertyName("links")]
    public ResourceLinks? Links { get; set; }
}
