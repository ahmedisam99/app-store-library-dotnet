using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A range of whole numbers, inclusive at both ends.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/integerrange"/>
public sealed class IntegerRange
{
    /// <summary>
    /// The lowest value the range covers.
    /// </summary>
    [JsonPropertyName("minimum")]
    public int? Minimum { get; set; }

    /// <summary>
    /// The highest value the range covers.
    /// </summary>
    [JsonPropertyName("maximum")]
    public int? Maximum { get; set; }
}
