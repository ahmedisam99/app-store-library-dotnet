using System.Text.Json;
using Enjna.AppStoreConnectApi.Models;
using Xunit;

namespace Enjna.AppStoreConnectApi.Tests;

public class RelationshipTests
{
    private const string NotIncluded = "{\"links\":{\"self\":\"https://api.appstoreconnect.apple.com/v2/inAppPurchases/6446819279/relationships/inAppPurchaseAvailability\"}}";

    private const string NullLinkage = "{\"data\":null}";

    private const string ToOneLinkage = "{\"data\":{\"type\":\"inAppPurchasePriceSchedules\",\"id\":\"6446819279\"}}";

    private const string ToManyLinkage = "{\"data\":[{\"type\":\"betaGroups\",\"id\":\"7f4b2a\"},{\"type\":\"betaGroups\",\"id\":\"9c1d8e\"}]}";

    private const string EmptyToManyLinkage = "{\"data\":[]}";

    [Fact]
    public void ReportsNoLinkageWhenTheResponseDoesntIncludeTheRelationship()
    {
        var relationship = Deserialize(NotIncluded);

        Assert.False(relationship.IncludesLinkage);
        Assert.False(relationship.HasNullLinkage);
        Assert.Null(relationship.Data);
        Assert.Null(relationship.ToOne());
        Assert.Empty(relationship.ToMany());
    }

    [Fact]
    public void ReportsNullLinkageWhenTheResponseIncludesAnExplicitNull()
    {
        var relationship = Deserialize(NullLinkage);

        Assert.True(relationship.IncludesLinkage);
        Assert.True(relationship.HasNullLinkage);
        Assert.Equal(JsonValueKind.Null, relationship.Data!.Value.ValueKind);
        Assert.Null(relationship.ToOne());
        Assert.Empty(relationship.ToMany());
    }

    [Fact]
    public void ReportsLinkageWhenTheResponseIncludesAToOneIdentifier()
    {
        var relationship = Deserialize(ToOneLinkage);

        Assert.True(relationship.IncludesLinkage);
        Assert.False(relationship.HasNullLinkage);

        var identifier = relationship.ToOne()!;

        Assert.Equal("inAppPurchasePriceSchedules", identifier.Type);
        Assert.Equal("6446819279", identifier.Id);
    }

    [Fact]
    public void ReportsLinkageWhenTheResponseIncludesToManyIdentifiers()
    {
        var relationship = Deserialize(ToManyLinkage);

        Assert.True(relationship.IncludesLinkage);
        Assert.False(relationship.HasNullLinkage);

        var identifiers = relationship.ToMany();

        Assert.Equal(2, identifiers.Length);
        Assert.Equal("betaGroups", identifiers[0].Type);
        Assert.Equal("7f4b2a", identifiers[0].Id);
        Assert.Equal("9c1d8e", identifiers[1].Id);
    }

    [Fact]
    public void ReportsLinkageWhenTheResponseIncludesAnEmptyToManyArray()
    {
        var relationship = Deserialize(EmptyToManyLinkage);

        Assert.True(relationship.IncludesLinkage);
        Assert.False(relationship.HasNullLinkage);
        Assert.Empty(relationship.ToMany());
    }

    [Fact]
    public void ReadsAToManyRelationshipAsToOneAsNull()
    {
        var relationship = Deserialize(ToManyLinkage);

        Assert.Null(relationship.ToOne());
        Assert.True(relationship.IncludesLinkage);
    }

    [Fact]
    public void ReadsAToOneRelationshipAsToManyAsEmpty()
    {
        var relationship = Deserialize(ToOneLinkage);

        Assert.Empty(relationship.ToMany());
        Assert.True(relationship.IncludesLinkage);
    }

    [Fact]
    public void SeparatesAnUnrequestedRelationshipFromOneThatLinksToNothing()
    {
        var notIncluded = Deserialize(NotIncluded);
        var linksToNothing = Deserialize(NullLinkage);

        Assert.Null(notIncluded.ToOne());
        Assert.Null(linksToNothing.ToOne());

        Assert.False(notIncluded.IncludesLinkage);
        Assert.False(notIncluded.HasNullLinkage);

        Assert.True(linksToNothing.IncludesLinkage);
        Assert.True(linksToNothing.HasNullLinkage);
    }

    [Fact]
    public void KeepsTheThreeLinkageStatesApartInsideAResource()
    {
        var app = JsonSerializer.Deserialize<App>(
            "{\"type\":\"apps\",\"id\":\"6446939457\",\"relationships\":{"
            + "\"appStoreVersions\":" + ToManyLinkage + ","
            + "\"betaLicenseAgreement\":" + NullLinkage + ","
            + "\"builds\":" + NotIncluded + "}}")!;

        var relationships = app.Relationships!;

        Assert.True(relationships["appStoreVersions"].IncludesLinkage);
        Assert.False(relationships["appStoreVersions"].HasNullLinkage);

        Assert.True(relationships["betaLicenseAgreement"].IncludesLinkage);
        Assert.True(relationships["betaLicenseAgreement"].HasNullLinkage);

        Assert.False(relationships["builds"].IncludesLinkage);
        Assert.False(relationships["builds"].HasNullLinkage);
    }

    private static Relationship Deserialize(string json)
    {
        return JsonSerializer.Deserialize<Relationship>(json)!;
    }
}
