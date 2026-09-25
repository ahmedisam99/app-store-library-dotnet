namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Review Submissions resource: a submission to App Review
/// that groups one or more items, such as app versions, in-app purchase versions, or in-app
/// events, so App Review looks at them together.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmission"/>
public sealed class ReviewSubmission : Resource<ReviewSubmissionAttributes>
{
}
