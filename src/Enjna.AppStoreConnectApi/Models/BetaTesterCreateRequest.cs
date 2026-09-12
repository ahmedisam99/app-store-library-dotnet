using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The request body you use to create a beta tester.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatestercreaterequest"/>
public sealed class BetaTesterCreateRequest
{
    /// <summary>
    /// The resource data. This property is required.
    /// </summary>
    [JsonPropertyName("data")]
    public required BetaTesterCreateRequestData Data { get; set; }
}
