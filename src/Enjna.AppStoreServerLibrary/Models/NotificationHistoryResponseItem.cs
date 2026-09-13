using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreServerLibrary.Models.Enums;

namespace Enjna.AppStoreServerLibrary.Models;

/// <summary>
/// The App Store server notification history record, including the signed notification payload and the result of the server's first send attempt.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/notificationhistoryresponseitem"/>
public sealed class NotificationHistoryResponseItem
{
    /// <summary>
    /// A cryptographically signed payload, in JSON Web Signature (JWS) format, containing the response body for a version 2 notification.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreservernotifications/signedpayload"/>
    [JsonPropertyName("signedPayload")]
    public string? SignedPayload { get; set; }

    /// <summary>
    /// An array of information the App Store server records for its attempts to send a notification to your server. The maximum number of entries in the array is six.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/sendattemptitem"/>
    [JsonPropertyName("sendAttempts")]
    public SendAttemptItem[]? SendAttempts { get; set; }

    /// <summary>
    /// The result of the App Store server's first attempt to send the notification to your server's App-Store-Server-Notifications-V2 endpoint.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/sendattemptresult"/>
    [Obsolete("Use the earliest SendAttemptItem in SendAttempts instead.")]
    [JsonPropertyName("firstSendAttemptResult")]
    public SendAttemptResult? FirstSendAttemptResult { get; set; }
}
