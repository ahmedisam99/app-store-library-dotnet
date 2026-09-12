using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models.Enums;

/// <summary>
/// The publication state of a developer response to a customer review.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/customerreviewresponsev1/attributes"/>
[JsonConverter(typeof(JsonEnumMemberConverter<CustomerReviewResponseState>))]
public enum CustomerReviewResponseState
{
    /// <summary>
    /// Represents a value not yet supported by this version of the library.
    /// </summary>
    _Unmapped,

    /// <summary>
    /// The response is live on the App Store, below the review it answers.
    /// </summary>
    [EnumMember(Value = "PUBLISHED")]
    Published,

    /// <summary>
    /// The response is waiting to be published, and customers can't see it yet.
    /// </summary>
    [EnumMember(Value = "PENDING_PUBLISH")]
    PendingPublish
}
