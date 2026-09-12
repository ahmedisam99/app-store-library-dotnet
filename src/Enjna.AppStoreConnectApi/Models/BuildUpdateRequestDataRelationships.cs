using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The relationships of a request that updates a Builds resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/buildupdaterequest/data/relationships"/>
public sealed class BuildUpdateRequestDataRelationships
{
    /// <summary>
    /// The related App Encryption Declarations resource to attach to the build.
    /// </summary>
    [JsonPropertyName("appEncryptionDeclaration")]
    public RelationshipDeclaration? AppEncryptionDeclaration { get; set; }
}
