using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that adds an item to a review submission.
/// </summary>
/// <remarks>
/// An item stands for one thing to review. Set <see cref="ReviewSubmission"/>, and set one of the
/// other relationships to the version, experiment, or event you're submitting. To submit several
/// things together, add one item for each of them to the same review submission.
/// </remarks>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/reviewsubmissionitemcreaterequest/data/relationships"/>
public sealed class ReviewSubmissionItemCreateRequestDataRelationships
{
    /// <summary>
    /// The related Review Submissions resource to add the item to, of type <c>reviewSubmissions</c>.
    /// This relationship is required.
    /// </summary>
    [JsonPropertyName("reviewSubmission")]
    public required RelationshipDeclaration ReviewSubmission { get; set; }

    /// <summary>
    /// The App Store version to review, of type <c>appStoreVersions</c>.
    /// </summary>
    [JsonPropertyName("appStoreVersion")]
    public RelationshipDeclaration? AppStoreVersion { get; set; }

    /// <summary>
    /// The custom product page version to review, of type <c>appCustomProductPageVersions</c>.
    /// </summary>
    [JsonPropertyName("appCustomProductPageVersion")]
    public RelationshipDeclaration? AppCustomProductPageVersion { get; set; }

    /// <summary>
    /// The App Store version experiment to review, as the original experiment resource, which Apple
    /// deprecated and replaced with the v2 one. Its type is <c>appStoreVersionExperiments</c>.
    /// </summary>
    [JsonPropertyName("appStoreVersionExperiment")]
    public RelationshipDeclaration? AppStoreVersionExperiment { get; set; }

    /// <summary>
    /// The App Store version experiment to review, as the v2 experiment resource: an A/B test that
    /// compares product page variants. Its type is also <c>appStoreVersionExperiments</c>.
    /// </summary>
    [JsonPropertyName("appStoreVersionExperimentV2")]
    public RelationshipDeclaration? AppStoreVersionExperimentV2 { get; set; }

    /// <summary>
    /// The in-app event to review, of type <c>appEvents</c>.
    /// </summary>
    [JsonPropertyName("appEvent")]
    public RelationshipDeclaration? AppEvent { get; set; }

    /// <summary>
    /// The background asset version to review, of type <c>backgroundAssetVersions</c>.
    /// </summary>
    [JsonPropertyName("backgroundAssetVersion")]
    public RelationshipDeclaration? BackgroundAssetVersion { get; set; }

    /// <summary>
    /// The Game Center achievement version to review, of type <c>gameCenterAchievementVersions</c>.
    /// </summary>
    [JsonPropertyName("gameCenterAchievementVersion")]
    public RelationshipDeclaration? GameCenterAchievementVersion { get; set; }

    /// <summary>
    /// The Game Center activity version to review, of type <c>gameCenterActivityVersions</c>.
    /// </summary>
    [JsonPropertyName("gameCenterActivityVersion")]
    public RelationshipDeclaration? GameCenterActivityVersion { get; set; }

    /// <summary>
    /// The Game Center challenge version to review, of type <c>gameCenterChallengeVersions</c>.
    /// </summary>
    [JsonPropertyName("gameCenterChallengeVersion")]
    public RelationshipDeclaration? GameCenterChallengeVersion { get; set; }

    /// <summary>
    /// The Game Center leaderboard set version to review, of type
    /// <c>gameCenterLeaderboardSetVersions</c>.
    /// </summary>
    [JsonPropertyName("gameCenterLeaderboardSetVersion")]
    public RelationshipDeclaration? GameCenterLeaderboardSetVersion { get; set; }

    /// <summary>
    /// The Game Center leaderboard version to review, of type <c>gameCenterLeaderboardVersions</c>.
    /// </summary>
    [JsonPropertyName("gameCenterLeaderboardVersion")]
    public RelationshipDeclaration? GameCenterLeaderboardVersion { get; set; }

    /// <summary>
    /// The in-app purchase version to review, of type <c>inAppPurchaseVersions</c>.
    /// </summary>
    [JsonPropertyName("inAppPurchaseVersion")]
    public RelationshipDeclaration? InAppPurchaseVersion { get; set; }

    /// <summary>
    /// The subscription version to review, of type <c>subscriptionVersions</c>.
    /// </summary>
    [JsonPropertyName("subscriptionVersion")]
    public RelationshipDeclaration? SubscriptionVersion { get; set; }

    /// <summary>
    /// The subscription group version to review, of type <c>subscriptionGroupVersions</c>. Submit
    /// one on its own when you change the group's localizations without changing any subscription
    /// in the group.
    /// </summary>
    [JsonPropertyName("subscriptionGroupVersion")]
    public RelationshipDeclaration? SubscriptionGroupVersion { get; set; }
}
