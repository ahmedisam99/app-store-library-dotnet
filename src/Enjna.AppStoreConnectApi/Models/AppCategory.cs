namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents an App Categories resource. A category is either a top-level
/// App Store category or a subcategory of one, and the same resource models both.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/appcategory"/>
public sealed class AppCategory : Resource<AppCategoryAttributes>
{
}
