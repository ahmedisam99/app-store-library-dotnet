using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreServerLibrary.Models.Enums;

namespace Enjna.AppStoreServerLibrary.Models;

/// <summary>
/// A response that contains the contents of the test notification sent by the App Store server and the result from your server.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/checktestnotificationresponse"/>
public sealed class CheckTestNotificationResponse
{
    /// <summary>
    /// A cryptographically signed payload, in JSON Web Signature (JWS) format, containing the response body for a version 2 notification.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/signedpayload"/>
    [JsonPropertyName("signedPayload")]
    public string? SignedPayload { get; set; }

    /// <summary>
    /// An array of information the App Store server records for its attempts to send the TEST notification to your server. The array may contain a maximum of six sendAttemptItem objects.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/sendattemptitem"/>
    [JsonPropertyName("sendAttempts")]
    public SendAttemptItem[]? SendAttempts { get; set; }

    /// <summary>
    /// The result of the App Store server's first attempt to send the TEST notification to your server.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/sendattemptresult"/>
    [Obsolete("Use the first SendAttemptItem in SendAttempts instead.")]
    [JsonPropertyName("firstSendAttemptResult")]
    public SendAttemptResult? FirstSendAttemptResult { get; set; }
}
