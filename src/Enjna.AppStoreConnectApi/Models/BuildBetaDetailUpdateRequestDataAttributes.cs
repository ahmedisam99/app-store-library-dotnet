using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of a request that updates a Build Beta Details resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/buildbetadetailupdaterequest/data/attributes"/>
public sealed class BuildBetaDetailUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// A Boolean value that turns automatic tester notifications for the build on or off.
    /// </summary>
    [JsonPropertyName("autoNotifyEnabled")]
    public bool? AutoNotifyEnabled { get => Get<bool?>(); set => Set(value); }
}
