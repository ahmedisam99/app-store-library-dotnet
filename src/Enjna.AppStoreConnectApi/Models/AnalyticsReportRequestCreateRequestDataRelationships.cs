using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of the request body that creates an Analytics Report Requests resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/analyticsreportrequestcreaterequest/data/relationships"/>
public sealed class AnalyticsReportRequestCreateRequestDataRelationships
{
    /// <summary>
    /// The Apps resource to generate the analytics reports for. This relationship is required.
    /// </summary>
    [JsonPropertyName("app")]
    public required RelationshipDeclaration App { get; set; }
}
