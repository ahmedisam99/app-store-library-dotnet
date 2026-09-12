using System.Collections.Generic;
using System.Linq;

namespace Enjna.AppStoreServerLibrary;

/// <summary>
/// Validation helpers for Advanced Commerce request fields.
/// </summary>
public static class HelperValidationUtils
{
    /// <summary>
    /// Maximum allowed length for an item description.
    /// </summary>
    public const int MaximumDescriptionLength = 45;

    /// <summary>
    /// Maximum allowed length for an item display name.
    /// </summary>
    public const int MaximumDisplayNameLength = 30;

    /// <summary>
    /// Maximum allowed length for a SKU.
    /// </summary>
    public const int MaximumSkuLength = 128;

    /// <summary>
    /// Minimum allowed value for a period count.
    /// </summary>
    public const int MinPeriodCount = 1;

    /// <summary>
    /// Maximum allowed value for a period count.
    /// </summary>
    public const int MaxPeriodCount = 12;

    /// <summary>
    /// Validates a description is non-null and does not exceed the maximum length.
    /// </summary>
    /// <param name="description">The description to validate.</param>
    /// <returns><c>true</c> if the description is valid; otherwise <c>false</c>.</returns>
    public static bool ValidateDescription(string? description)
    {
        return description is not null && description.Length <= MaximumDescriptionLength;
    }

    /// <summary>
    /// Validates a display name is non-null and does not exceed the maximum length.
    /// </summary>
    /// <param name="displayName">The display name to validate.</param>
    /// <returns><c>true</c> if the display name is valid; otherwise <c>false</c>.</returns>
    public static bool ValidateDisplayName(string? displayName)
    {
        return displayName is not null && displayName.Length <= MaximumDisplayNameLength;
    }

    /// <summary>
    /// Validates a SKU is non-null and does not exceed the maximum length.
    /// </summary>
    /// <param name="sku">The SKU to validate.</param>
    /// <returns><c>true</c> if the SKU is valid; otherwise <c>false</c>.</returns>
    public static bool ValidateSku(string? sku)
    {
        return sku is not null && sku.Length <= MaximumSkuLength;
    }

    /// <summary>
    /// Validates a period count is within the inclusive range [<see cref="MinPeriodCount"/>, <see cref="MaxPeriodCount"/>].
    /// </summary>
    /// <param name="periodCount">The period count to validate.</param>
    /// <returns><c>true</c> if the period count is valid; otherwise <c>false</c>.</returns>
    public static bool ValidatePeriodCount(int? periodCount)
    {
        return periodCount is >= MinPeriodCount and <= MaxPeriodCount;
    }

    /// <summary>
    /// Validates a list of items is non-null, non-empty, and contains no null elements.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="items">The items to validate.</param>
    /// <returns><c>true</c> if the items are valid; otherwise <c>false</c>.</returns>
    public static bool ValidateItems<T>(IEnumerable<T?>? items) where T : class
    {
        if (items is null) return false;
        var materialized = items as ICollection<T?> ?? items.ToList();
        return materialized.Count > 0 && materialized.All(item => item is not null);
    }
}
