# App Store Connect API for .NET

A .NET library for the [App Store Connect API](https://developer.apple.com/documentation/appstoreconnectapi) — the API that manages apps, in-app purchases, subscriptions, TestFlight, customer reviews, and analytics reports.

> This is a community-maintained library and is not affiliated with Apple.

## Table of Contents

1. [Installation](#installation)
2. [Documentation](#documentation)
3. [Obtaining an App Store Connect API key](#obtaining-an-app-store-connect-api-key)
4. [Usage](#usage)
   1. [API Client](#api-client)
   2. [Querying](#querying)
   3. [Paging](#paging)
   4. [Updating resources](#updating-resources)
   5. [Error Handling](#error-handling)
5. [Using with Dependency Injection](#using-with-dependency-injection)
6. [Coverage](#coverage)

## Installation

### Requirements

- .NET 8.0+

### NuGet

```bash
dotnet add package Enjna.AppStoreConnectApi
```

## Documentation

[Documentation](https://ahmedisam99.github.io/app-store-library-dotnet/index.html)

## Obtaining an App Store Connect API key

Go to Users and Access > Integrations > App Store Connect API to create a key and find your issuer ID. When using a key, you'll need the key ID and issuer ID as well.

This is **not** the same key as the In-App Purchase key the App Store Server API uses. An In-App Purchase key doesn't authenticate against the App Store Connect API, and an App Store Connect API key doesn't authenticate against the App Store Server API. If you use both APIs, create both keys.

Individual keys, created from a user's own profile rather than by an Admin, have no issuer ID and only reach the endpoints that user's role allows. Construct a client for one with `AppStoreConnectAPIClient.ForIndividualKey`.

## Usage

### API Client

`AppStoreConnectAPIClient` implements `IDisposable`. It signs a fresh bearer token for each request and owns the `HttpClient` it creates, so a single long-lived instance is the intended usage.

```csharp
var issuerId = "99b16628-15e4-4668-972b-eeff55eeff55";
var keyId = "ABCDEFGHIJ";
var privateKey = File.ReadAllText("/path/to/AuthKey_ABCDEFGHIJ.p8");

using var client = new AppStoreConnectAPIClient(privateKey, keyId, issuerId);

// Or, with an individual key, which has no issuer ID:
using var individualClient = AppStoreConnectAPIClient.ForIndividualKey(privateKey, keyId);

var query = new AppStoreConnectQuery().Filter("bundleId", "com.example");
var response = await client.ListAppsAsync(query);

foreach (var app in response.Data)
{
    Console.WriteLine($"{app.Id}: {app.Attributes?.Name} ({app.Attributes?.Sku})");
}
```

### Querying

`AppStoreConnectQuery` builds the query parameters the API accepts. Every call returns the same query, so they chain, and every read endpoint that accepts query parameters takes one as an optional argument.

```csharp
var query = new AppStoreConnectQuery()
    .Filter("bundleId", "com.example", "com.example.other") // Several values match as an OR
    .Fields("apps", "name", "bundleId", "sku")
    .Include("appStoreVersions")
    .Sort("-bundleId")
    .Limit(200);

var response = await client.ListAppsAsync(query);
```

Use `Exists`, `Limit(relationship, limit)`, and `Parameter` for relationship existence filters, per-relationship limits, and anything else the API accepts.

### Paging

List responses are paged with an opaque cursor, so follow the next-page link rather than rebuilding the URL. `EnumerateResourcesAsync` walks a first page and everything after it, one page at a time.

```csharp
var firstPage = await client.ListAppsAsync(new AppStoreConnectQuery().Limit(200));

await foreach (var app in client.EnumerateResourcesAsync(firstPage))
{
    Console.WriteLine(app.Attributes?.BundleId);
}
```

`EnumeratePagesAsync` yields whole pages instead, and `GetNextPageAsync` reads exactly one more page, returning `null` at the end.

### Updating resources

An update carries only the attributes you assign. A property you never assign stays out of the JSON, so Apple keeps the value it already has, and the rest of the resource is untouched. That is what makes a partial update partial.

Assigning `null` is a different thing from not assigning at all. It writes an explicit `null`, which asks Apple to clear the stored value:

```csharp
// Clears the end date, leaving the offer open-ended.
await client.UpdateSubscriptionIntroductoryOfferAsync(
    offerId,
    new SubscriptionIntroductoryOfferUpdateRequest
    {
        Data = new SubscriptionIntroductoryOfferUpdateRequestData
        {
            Id = offerId,
            Attributes = new SubscriptionIntroductoryOfferUpdateRequestDataAttributes
            {
                EndDate = null
            }
        }
    });
```

C# can't tell an unassigned property from one assigned `null`, so the attributes of an update request record which properties were assigned and only those reach the wire. `IsAssigned` and `AssignedAttributes` report what the request will carry.

So assignment is what matters, not the value. Copying fields off another object assigns every one of them, and a source field that happens to be `null` then clears the stored value instead of leaving it alone:

```csharp
// Wipes the review note whenever the source happens to have none.
Attributes = new InAppPurchaseV2UpdateRequestDataAttributes
{
    Name = source.Name,
    ReviewNote = source.ReviewNote
}
```

Assign only the properties you mean to change.

Apple documents no behaviour for an explicit `null` on any attribute, and not every attribute has an empty state to return to — a name or a bundle ID doesn't. An endpoint that refuses a null answers with an error you can catch. One that ignores it answers 200 and keeps the stored value, so read the attributes on the response when you expect a value to disappear.

Relationships reach the same result through a different mechanism. `RelationshipDeclaration` and `RelationshipDeclarationList` always write their `data` member, so a declaration with `Data = null` clears the relationship, and leaving the whole declaration off the request leaves it alone.

### Error Handling

Any non-success response throws an `APIException`, which carries the status code and Apple's parsed `Errors` array. Compare `ErrorDetail.Code` by prefix rather than by exact match, since Apple appends more specific suffixes.

```csharp
try
{
    var response = await client.ListAppsAsync();
}
catch (APIException ex)
{
    Console.WriteLine($"HTTP {ex.HttpStatusCode}");

    foreach (var error in ex.Errors ?? [])
    {
        Console.WriteLine($"{error.Code}: {error.Detail}");
    }

    // Rate-limit information from the failed response itself
    Console.WriteLine(ex.RateLimit?.UserHourRemaining);
}
```

`client.LastRateLimit` carries the same information from the most recent response, successful or not. It is client-wide state, so read it right after the call you care about, or prefer `APIException.RateLimit` on the failure path.

## Using with Dependency Injection

`AppStoreConnectAPIClient` is thread-safe and meant to be registered as a singleton. Construct it directly. The `HttpClient` it creates for itself sets `PooledConnectionLifetime` to five minutes, so pooled connections are dropped and DNS is resolved again on that cycle. That is the stale-DNS problem `IHttpClientFactory` is usually brought in to solve. The DI container disposes the client at shutdown, and the client disposes the `HttpClient` it owns.

```csharp
builder.Services.AddSingleton(_ =>
{
    var privateKey = File.ReadAllText("/path/to/AuthKey_ABCDEFGHIJ.p8");

    return new AppStoreConnectAPIClient(
        privateKey,
        keyId: "ABCDEFGHIJ",
        issuerId: "99b16628-15e4-4668-972b-eeff55eeff55"
    );
});
```

Reach for `IHttpClientFactory` when you want your own handlers in the chain, such as logging, retries, or a proxy. Register a typed client so the factory owns the `HttpClient` and rotates its handlers for you. Typed clients are transient by default, which costs nothing here: the client holds no state between requests. If the custom handlers aren't worth the extra wiring, stay with the singleton above.

```csharp
builder.Services.AddHttpClient<AppStoreConnectAPIClient>()
    .AddTypedClient(httpClient =>
    {
        var privateKey = File.ReadAllText("/path/to/AuthKey_ABCDEFGHIJ.p8");

        return new AppStoreConnectAPIClient(
            privateKey,
            keyId: "ABCDEFGHIJ",
            issuerId: "99b16628-15e4-4668-972b-eeff55eeff55",
            httpClient: httpClient
        );
    });
```

A client handed an `HttpClient` never disposes it, so the factory keeps that lifetime. What doesn't work is calling `IHttpClientFactory.CreateClient` inside an `AddSingleton` factory and holding on to the result. The captured client pins one handler chain for the life of the process, and the connection recycling you registered the factory for never happens.

## Coverage

Built against the App Store Connect API specification 4.4.1.

Covered today: apps, in-app purchases, subscriptions, subscription groups, subscription pricing, offer codes, customer reviews, analytics reports, and TestFlight builds and beta groups. Not covered: Game Center, Xcode Cloud, provisioning, and App Store versions and release management.
