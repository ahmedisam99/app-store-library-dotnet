using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// One entry of the <c>included</c> array of a <see cref="SubscriptionUpdateRequest"/>. Apple
/// allows three resource types there, and the three types listed below are the only ones that
/// implement this interface.
/// </summary>
/// <remarks>
/// Every allowed type is named in a <see cref="JsonDerivedTypeAttribute"/> so that the serializer
/// writes an entry by its own runtime type. Without those attributes it writes the members of the
/// declared type instead, which for an interface with no members is an empty object, and it does so
/// without raising anything. None of the attributes carries a type discriminator, so nothing is
/// added to the JSON that Apple does not expect. An implementation from outside this library is not
/// named here and fails the serializer with a <see cref="System.NotSupportedException"/>.
/// </remarks>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/subscriptionupdaterequest"/>
[JsonDerivedType(typeof(SubscriptionPromotionalOfferInlineCreate))]
[JsonDerivedType(typeof(SubscriptionPriceInlineCreate))]
[JsonDerivedType(typeof(SubscriptionIntroductoryOfferInlineCreate))]
public interface ISubscriptionUpdateRequestIncludedResource
{
}
