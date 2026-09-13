using System;
using System.Text.Json.Serialization;
using Enjna.AppStoreServerLibrary.Models.Enums;

namespace Enjna.AppStoreServerLibrary.Models;

/// <summary>
/// The request body for notification history.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/notificationhistoryrequest"/>
public sealed class NotificationHistoryRequest
{
    /// <summary>
    /// The start date of the timespan for the requested App Store Server Notification history records. The startDate needs to precede the endDate. Choose a startDate that's within the past 180 days from the current date.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/startdate"/>
    [JsonPropertyName("startDate")]
    public long? StartDate { get; set; }

    /// <summary>
    /// The end date of the timespan for the requested App Store Server Notification history records. Choose an endDate that's later than the startDate. If you choose an endDate in the future, the endpoint automatically uses the current date as the endDate.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/enddate"/>
    [JsonPropertyName("endDate")]
    public long? EndDate { get; set; }

    /// <summary>
    /// A notification type. Provide this field to limit the notification history records to those with this one notification type.
    /// Include either the transactionId or the notificationType in your query, but not both.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/notificationtype"/>
    [JsonPropertyName("notificationType")]
    public NotificationTypeV2? NotificationType { get; set; }

    /// <summary>
    /// A notification subtype. Provide this field to limit the notification history records to those with this one notification subtype.
    /// If you specify a notificationSubtype, you need to also specify its related notificationType.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/notificationsubtype"/>
    [JsonPropertyName("notificationSubtype")]
    public Subtype? NotificationSubtype { get; set; }

    /// <summary>
    /// The transaction identifier, which may be an original transaction identifier, of any transaction belonging to the customer.
    /// Include either the transactionId or the notificationType in your query, but not both.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/transactionid"/>
    [JsonPropertyName("transactionId")]
    public string? TransactionId { get; set; }

    /// <summary>
    /// The original transaction identifier of any transaction belonging to the customer.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/originaltransactionid"/>
    [Obsolete("Use TransactionId instead.")]
    [JsonPropertyName("originalTransactionId")]
    public string? OriginalTransactionId { get; set; }

    /// <summary>
    /// A Boolean value you set to true to request only the notifications that haven't reached your server successfully.
    /// </summary>
    /// <seealso href="https://developer.apple.com/documentation/appstoreserverapi/onlyfailures"/>
    [JsonPropertyName("onlyFailures")]
    public bool? OnlyFailures { get; set; }
}
