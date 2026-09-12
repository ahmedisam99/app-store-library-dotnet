using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A to-one relationship payload that you send in a create or update request body.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi"/>
public sealed class RelationshipDeclaration
{
    /// <summary>
    /// The related resource. Set it to <c>null</c> to clear the relationship, which sends an
    /// explicit <c>"data": null</c> rather than omitting the member.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonPropertyName("data")]
    public required ResourceIdentifier? Data { get; set; }

    /// <summary>
    /// Creates a to-one relationship payload that points at a single resource.
    /// </summary>
    /// <param name="type">The resource type of the related resource, such as <c>apps</c>.</param>
    /// <param name="id">The resource ID of the related resource.</param>
    /// <returns>A relationship payload that points at the resource.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the type or the ID is <c>null</c>.</exception>
    public static RelationshipDeclaration To(string type, string id)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(id);

        return new RelationshipDeclaration
        {
            Data = new ResourceIdentifier
            {
                Type = type,
                Id = id
            }
        };
    }
}
