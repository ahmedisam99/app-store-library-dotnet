using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request that updates an app tag. Visibility is the only one you can
/// change; the name of a tag belongs to the App Store.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/apptagupdaterequest/data-data.dictionary/attributes-data.dictionary"/>
public sealed class AppTagUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// A Boolean value that indicates whether the App Store shows the tag on the app's product
    /// page and features the app under the tag. Assign <c>false</c> to opt the app out of the tag.
    /// </summary>
    /// <remarks>
    /// Assigning <c>null</c> sends an explicit <c>null</c>, which asks Apple to clear the stored
    /// value. Leave the property unassigned to keep it.
    /// </remarks>
    [JsonPropertyName("visibleInAppStore")]
    public bool? VisibleInAppStore { get => Get<bool?>(); set => Set(value); }
}
