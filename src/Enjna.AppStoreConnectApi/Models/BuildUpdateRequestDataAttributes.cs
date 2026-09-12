using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of a request that updates a Builds resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/buildupdaterequest/data/attributes"/>
public sealed class BuildUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// Set this to <c>true</c> to expire the build early, which stops testers from installing it.
    /// </summary>
    [JsonPropertyName("expired")]
    public bool? Expired { get => Get<bool?>(); set => Set(value); }

    /// <summary>
    /// A Boolean value that declares whether the build uses encryption that export compliance
    /// doesn't exempt.
    /// </summary>
    [JsonPropertyName("usesNonExemptEncryption")]
    public bool? UsesNonExemptEncryption { get => Get<bool?>(); set => Set(value); }
}
