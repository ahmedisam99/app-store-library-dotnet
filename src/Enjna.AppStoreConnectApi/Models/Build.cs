namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Builds resource. A build is one binary you uploaded to
/// App Store Connect, which you can hand to TestFlight testers and submit to the App Store.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/build"/>
public sealed class Build : Resource<BuildAttributes>
{
}
