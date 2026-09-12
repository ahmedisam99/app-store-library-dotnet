using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// An image that App Store Connect finished processing, described by a template you fill in to
/// build the URL of a particular size and format.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/imageasset"/>
public sealed class ImageAsset
{
    /// <summary>
    /// The URL template of the processed image. Replace the <c>{w}</c>, <c>{h}</c>, and <c>{f}</c>
    /// placeholders with the width, the height, and the file format you want, such as <c>png</c>.
    /// </summary>
    [JsonPropertyName("templateUrl")]
    public string? TemplateUrl { get; set; }

    /// <summary>
    /// The width of the source image, in pixels.
    /// </summary>
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    /// <summary>
    /// The height of the source image, in pixels.
    /// </summary>
    [JsonPropertyName("height")]
    public int? Height { get; set; }
}
