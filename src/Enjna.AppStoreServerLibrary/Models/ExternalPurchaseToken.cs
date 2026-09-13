using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreServerLibrary.Models.Enums;

namespace Enjna.AppStoreServerLibrary.Models;

/// <summary>
/// The payload data that contains an external purchase token.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/externalpurchasetoken"/>
public sealed class ExternalPurchaseToken
{
    /// <summary>
    /// The field of an external purchase token that uniquely identifies the token.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/externalpurchaseid"/>
    [JsonPropertyName("externalPurchaseId")]
    public string? ExternalPurchaseId { get; set; }

    /// <summary>
    /// The field of an external purchase token that contains the UNIX date, in milliseconds, when the system created the token.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/tokencreationdate"/>
    [JsonPropertyName("tokenCreationDate")]
    public long? TokenCreationDate { get; set; }

    /// <summary>
    /// The UTC date and time when the system created the token,
    /// derived from <see cref="TokenCreationDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? TokenCreationDateUtc => TokenCreationDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(TokenCreationDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The unique identifier of an app in the App Store.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/appappleid"/>
    [JsonPropertyName("appAppleId")]
    public long? AppAppleId { get; set; }

    /// <summary>
    /// The bundle identifier of an app.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/bundleid"/>
    [JsonPropertyName("bundleId")]
    public string? BundleId { get; set; }

    /// <summary>
    /// The field of a custom link token that contains the UNIX date, in milliseconds, when the token expires. This field is present only for custom link tokens.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/tokenexpirationdate"/>
    [JsonPropertyName("tokenExpirationDate")]
    public long? TokenExpirationDate { get; set; }

    /// <summary>
    /// The UTC date and time when the token expires,
    /// derived from <see cref="TokenExpirationDate"/>.
    /// </summary>
    [JsonIgnore]
    public DateTime? TokenExpirationDateUtc => TokenExpirationDate.HasValue
        ? DateTimeOffset.FromUnixTimeMilliseconds(TokenExpirationDate.Value).UtcDateTime
        : null;

    /// <summary>
    /// The custom link token type, either SERVICES or ACQUISITION. This field is present only for custom link tokens.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/tokentype"/>
    [JsonPropertyName("tokenType")]
    public TokenType? TokenType { get; set; }
}
