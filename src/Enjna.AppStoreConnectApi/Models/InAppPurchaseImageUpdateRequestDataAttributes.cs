using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The attributes of the request body that commits an In-App Purchase Images resource.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/inapppurchaseimageupdaterequest/data/attributes"/>
public sealed class InAppPurchaseImageUpdateRequestDataAttributes : AttributeChangeSet
{
    /// <summary>
    /// The MD5 checksum of the file you uploaded, so App Store Connect can confirm it received the
    /// file intact.
    /// </summary>
    [JsonPropertyName("sourceFileChecksum")]
    public string? SourceFileChecksum { get => Get<string?>(); set => Set(value); }

    /// <summary>
    /// Set this to <c>true</c> once you send every upload operation, to tell App Store Connect the
    /// file is complete.
    /// </summary>
    [JsonPropertyName("uploaded")]
    public bool? Uploaded { get => Get<bool?>(); set => Set(value); }
}
