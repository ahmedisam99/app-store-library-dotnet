using System;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe an In-App Purchase Contents resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchasecontent/attributes"/>
public sealed class InAppPurchaseContentAttributes
{
    /// <summary>
    /// The name of the hosted content file.
    /// </summary>
    [JsonPropertyName("fileName")]
    public string? FileName { get; set; }

    /// <summary>
    /// The size of the hosted content file, in bytes.
    /// </summary>
    [JsonPropertyName("fileSize")]
    public long? FileSize { get; set; }

    /// <summary>
    /// The URL to download the hosted content from.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// The date you last changed the hosted content.
    /// </summary>
    [JsonPropertyName("lastModifiedDate")]
    public DateTimeOffset? LastModifiedDate { get; set; }
}
