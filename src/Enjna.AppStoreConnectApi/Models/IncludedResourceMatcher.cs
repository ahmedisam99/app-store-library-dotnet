using System;
using System.Text.Json;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// Matches an entry of a response's <c>included</c> array against a resource type and ID pair.
/// </summary>
internal static class IncludedResourceMatcher
{
    /// <summary>
    /// Determines whether an included entry is the resource identified by the given type and ID.
    /// </summary>
    /// <param name="element">The included entry to test.</param>
    /// <param name="type">The resource type to match.</param>
    /// <param name="id">The resource ID to match.</param>
    /// <returns><c>true</c> when both the type and the ID match; otherwise, <c>false</c>.</returns>
    internal static bool Matches(JsonElement element, string type, string id)
    {
        if (element.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        if (!element.TryGetProperty("type", out var typeProperty) || typeProperty.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        if (!element.TryGetProperty("id", out var idProperty) || idProperty.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        return string.Equals(typeProperty.GetString(), type, StringComparison.Ordinal)
            && string.Equals(idProperty.GetString(), id, StringComparison.Ordinal);
    }
}
