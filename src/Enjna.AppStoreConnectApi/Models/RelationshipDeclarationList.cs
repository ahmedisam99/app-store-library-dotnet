using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A to-many relationship payload that you send in a create, update, or linkages request body.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi"/>
public sealed class RelationshipDeclarationList
{
    /// <summary>
    /// The related resources. Set it to <c>null</c> to clear the relationship, which sends an
    /// explicit <c>"data": null</c> rather than omitting the member. An empty array sends
    /// <c>"data": []</c>, which clears the members of a to-many relationship.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    [JsonPropertyName("data")]
    public required ResourceIdentifier[]? Data { get; set; }

    /// <summary>
    /// Creates a to-many relationship payload that points at zero or more resources of the same type.
    /// </summary>
    /// <param name="type">The resource type of the related resources, such as <c>builds</c>.</param>
    /// <param name="ids">The resource IDs of the related resources.</param>
    /// <returns>A relationship payload that points at the resources.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the type or the IDs are <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when an ID is <c>null</c> or blank.</exception>
    public static RelationshipDeclarationList To(string type, params string[] ids)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(ids);

        var data = new ResourceIdentifier[ids.Length];

        for (var index = 0; index < ids.Length; index++)
        {
            if (string.IsNullOrWhiteSpace(ids[index]))
            {
                throw new ArgumentException(
                    $"The resource ID at index {index} is null or blank.",
                    nameof(ids));
            }

            data[index] = new ResourceIdentifier
            {
                Type = type,
                Id = ids[index]
            };
        }

        return new RelationshipDeclarationList
        {
            Data = data
        };
    }
}
