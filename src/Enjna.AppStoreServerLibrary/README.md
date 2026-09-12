# App Store Server Library for .NET

A .NET library for the [App Store Server API](https://developer.apple.com/documentation/appstoreserverapi), [App Store Server Notifications](https://developer.apple.com/documentation/appstoreservernotifications), and [Retention Messaging API](https://developer.apple.com/documentation/retentionmessaging).

> This is a community-maintained library and is not affiliated with Apple. For official libraries, see [Swift](https://github.com/apple/app-store-server-library-swift), [Node.js](https://github.com/apple/app-store-server-library-node), [Python](https://github.com/apple/app-store-server-library-python), and [Java](https://github.com/apple/app-store-server-library-java).

`Enjna.AppStoreServerLibrary` ships independently of `Enjna.AppStoreConnectApi`, the sibling package for the [App Store Connect API](https://developer.apple.com/documentation/appstoreconnectapi). Each has its own version, and neither depends on the other. Install whichever you need.

## Table of Contents

1. [Installation](#installation)
2. [Documentation](#documentation)
   1. [Obtaining an In-App Purchase key from App Store Connect](#obtaining-an-in-app-purchase-key-from-app-store-connect)
   2. [Obtaining Apple Root Certificates](#obtaining-apple-root-certificates)
3. [Usage](#usage)
   1. [API Client](#api-client)
   2. [Signed Data Verification](#signed-data-verification)
   3. [Receipt Usage](#receipt-usage)
   4. [Promotional Offer Signature Creation](#promotional-offer-signature-creation)
4. [Using with Dependency Injection](#using-with-dependency-injection)
   1. [Registering the API Client](#registering-the-api-client)
   2. [Registering Other Services](#registering-other-services)

## Installation

### Requirements

- .NET 8.0+

### NuGet

```bash
dotnet add package Enjna.AppStoreServerLibrary
```

## Documentation

[Documentation](https://ahmedisam99.github.io/app-store-library-dotnet/index.html)

[WWDC Video](https://developer.apple.com/videos/play/wwdc2023/10143/)

### Obtaining an In-App Purchase key from App Store Connect

To use the App Store Server API or create promotional offer signatures, a signing key downloaded from App Store Connect is required. To obtain this key, you must have the Admin role. Go to Users and Access > Integrations > In-App Purchase. Here you can create and manage keys, as well as find your issuer ID. When using a key, you'll need the key ID and issuer ID as well.

### Obtaining Apple Root Certificates

Download and store the root certificates found in the Apple Root Certificates section of the [Apple PKI](https://www.apple.com/certificateauthority/) site. Provide these certificates as an array to a `SignedDataVerifier` to allow verifying the signed data comes from Apple.

## Usage

### API Client

```csharp
var issuerId = "99b16628-15e4-4668-972b-eeff55eeff55";
var keyId = "ABCDEFGHIJ";
var bundleId = "com.example";
var privateKey = File.ReadAllText("/path/to/key.p8");
var environment = AppStoreEnvironment.Sandbox;

var client = new AppStoreServerAPIClient(privateKey, keyId, issuerId, environment);

var response = await client.RequestTestNotificationAsync(bundleId);
Console.WriteLine(response.TestNotificationToken);
```

### Signed Data Verification

```csharp
var bundleId = "com.example";
var appleRootCAs = new[] { File.ReadAllBytes("/path/to/AppleRootCA-G3.cer") };
var enableOnlineChecks = true;
var environment = AppStoreEnvironment.Sandbox;
long? appAppleId = null; // Optional. In Production, pass this if you want to validate it.

var verifier = new SignedDataVerifier(appleRootCAs, enableOnlineChecks, environment);

var notificationPayload = "ey...";
var verifiedNotification = await verifier.VerifyAndDecodeNotificationAsync(notificationPayload, bundleId, appAppleId);
Console.WriteLine(verifiedNotification.NotificationType);
```

### Receipt Usage

```csharp
var issuerId = "99b16628-15e4-4668-972b-eeff55eeff55";
var keyId = "ABCDEFGHIJ";
var bundleId = "com.example";
var privateKey = File.ReadAllText("/path/to/key.p8");
var environment = AppStoreEnvironment.Sandbox;

var client = new AppStoreServerAPIClient(privateKey, keyId, issuerId, environment);

var appReceipt = "MI...";
var receiptUtility = new ReceiptUtility();
var transactionId = receiptUtility.ExtractTransactionIdFromAppReceipt(appReceipt);
if (transactionId is not null)
{
    var request = new TransactionHistoryRequest
    {
        Sort = SortOrder.Ascending,
        Revoked = false,
        ProductTypes = [ProductType.AutoRenewable]
    };

    HistoryResponse? response = null;
    var transactions = new List<string>();
    do
    {
        request.Revision = response?.Revision;
        response = await client.GetTransactionHistoryAsync(transactionId, bundleId, request);
        if (response.SignedTransactions is not null)
        {
            transactions.AddRange(response.SignedTransactions);
        }
    } while (response.HasMore);

    Console.WriteLine($"Found {transactions.Count} transactions");
}
```

### Promotional Offer Signature Creation

```csharp
var keyId = "ABCDEFGHIJ";
var bundleId = "com.example";
var privateKey = File.ReadAllText("/path/to/key.p8");

var productId = "<product_id>";
var subscriptionOfferId = "<subscription_offer_id>";
var appAccountToken = "<app_account_token>";
var nonce = Guid.NewGuid();
var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

using var signatureCreator = new PromotionalOfferSignatureCreator(privateKey, keyId);
var signature = signatureCreator.CreateSignature(productId, subscriptionOfferId, appAccountToken, nonce, timestamp, bundleId);
Console.WriteLine(signature);
```

## Using with Dependency Injection

All classes in this library are thread-safe and can be registered as singletons. The DI container will handle disposal at application shutdown for classes that implement `IDisposable`.

The one class that requires special attention is `AppStoreServerAPIClient`, since it uses an `HttpClient` internally. In .NET, managing `HttpClient` lifetimes manually can lead to socket exhaustion or stale DNS issues. The recommended approach is to use `IHttpClientFactory`.

### Registering the API Client

#### Option 1: Named client with a manual factory registration

```csharp
builder.Services.AddHttpClient("AppStoreServer");

builder.Services.AddSingleton(sp =>
{
    var privateKey = File.ReadAllText("/path/to/key.p8");
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("AppStoreServer");

    return new AppStoreServerAPIClient(
        privateKey,
        keyId: "ABCDEFGHIJ",
        issuerId: "99b16628-15e4-4668-972b-eeff55eeff55",
        environment: AppStoreEnvironment.Production,
        httpClient: httpClient
    );
});
```

#### Option 2: Typed client with `AddTypedClient`

```csharp
// Registered as transient by default
builder.Services.AddHttpClient<AppStoreServerAPIClient>()
    .AddTypedClient((httpClient) =>
    {
        var privateKey = File.ReadAllText("/path/to/key.p8");

        return new AppStoreServerAPIClient(
            privateKey,
            keyId: "ABCDEFGHIJ",
            issuerId: "99b16628-15e4-4668-972b-eeff55eeff55",
            environment: AppStoreEnvironment.Production,
            httpClient: httpClient
        );
    });
```

In both cases, by passing an externally managed `HttpClient`, the `AppStoreServerAPIClient` will not dispose it, leaving lifetime management to the factory.

### Registering Other Services

The remaining classes don't use `HttpClient` and can be registered directly as singletons:

```csharp
builder.Services.AddSingleton(new SignedDataVerifier(
    appleRootCertificates: new[] { File.ReadAllBytes("/path/to/AppleRootCA-G3.cer") },
    enableOnlineChecks: true,
    environment: AppStoreEnvironment.Production
));

builder.Services.AddSingleton<ReceiptUtility>();

builder.Services.AddSingleton(new PromotionalOfferSignatureCreator(
    signingKey: File.ReadAllText("/path/to/key.p8"),
    keyId: "ABCDEFGHIJ"
));
```
