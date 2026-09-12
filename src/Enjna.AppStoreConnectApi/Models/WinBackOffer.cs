namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The data structure that represents a Win-Back Offers resource, which discounts a subscription for
/// a customer whose subscription lapsed. The App Store decides who qualifies from the eligibility
/// rules in the offer's attributes.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/winbackoffer"/>
public sealed class WinBackOffer : Resource<WinBackOfferAttributes>
{
}
