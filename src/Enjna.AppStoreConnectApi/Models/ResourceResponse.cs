using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A response that contains a single resource.
/// </summary>
/// <typeparam name="TResource">The type of the resource the response contains.</typeparam>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi"/>
public sealed class ResourceResponse<TResource> where TResource : class
{
    /// <summary>
    /// The resource data.
    /// </summary>
    [JsonPropertyName("data")]
    public TResource Data { get; set; } = null!;

    /// <summary>
    /// The related resources you request with the <c>include</c> query parameter. The array is
    /// heterogeneous, so read an entry with <see cref="TryGetIncluded{TIncluded}"/>.
    /// </summary>
    [JsonPropertyName("included")]
    public JsonElement[]? Included { get; set; }

    /// <summary>
    /// Navigational links that include the self-link.
    /// </summary>
    [JsonPropertyName("links")]
    public DocumentLinks Links { get; set; } = null!;

    /// <summary>
    /// Finds an included resource by its type and its ID, and deserializes it.
    /// </summary>
    /// <typeparam name="TIncluded">The type to deserialize the included resource into.</typeparam>
    /// <param name="type">The resource type of the included resource, such as <c>builds</c>.</param>
    /// <param name="id">The resource ID of the included resource.</param>
    /// <param name="value">The deserialized included resource, or <c>null</c> when it isn't present or doesn't deserialize into <typeparamref name="TIncluded"/>.</param>
    /// <returns><c>true</c> when the included resource is present and deserializes; otherwise, <c>false</c>.</returns>
    public bool TryGetIncluded<TIncluded>(string type, string id, [NotNullWhen(true)] out TIncluded? value)
    {
        value = default;

        if (Included is null)
        {
            return false;
        }

        foreach (var element in Included)
        {
            if (!IncludedResourceMatcher.Matches(element, type, id))
            {
                continue;
            }

            try
            {
                value = element.Deserialize<TIncluded>();
            }
            catch (Exception exception) when (exception is JsonException or NotSupportedException)
            {
                value = default;

                return false;
            }

            return value is not null;
        }

        return false;
    }
}
