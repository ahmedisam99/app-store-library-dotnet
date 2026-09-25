using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes that describe a Review Submission Items resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionitem/attributes"/>
public sealed class ReviewSubmissionItemAttributes
{
    /// <summary>
    /// Where the item sits in App Review.
    /// </summary>
    [JsonPropertyName("state")]
    public ReviewSubmissionItemState? State { get; set; }
}
