# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What This Is

Two community-maintained .NET 8.0 packages that version and ship independently: `Enjna.AppStoreServerLibrary` for the App Store Server API, and `Enjna.AppStoreConnectApi` for the App Store Connect API. Neither depends on the other.

A snapshot of Apple's own documentation is vendored under `spec/`: the App Store Server API, App Store Server Notifications, Retention Messaging and Advanced Commerce documentation sets, plus Apple's App Store Connect OpenAPI document and its documentation pages. It is the reference for what the packages should cover. Refresh it with `dotnet run --project tools/AppleSpec -- sync`, after which `git diff spec/` is what Apple changed.

The snapshot covers API shape, not client behaviour. Apple documents nothing about certificate chain validation, OCSP, or receipt parsing; for those, read Apple's own [Swift](https://github.com/apple/app-store-server-library-swift) or [Node.js](https://github.com/apple/app-store-server-library-node) library.

## Build & Test Commands

```bash
dotnet build                    # Build the solution
dotnet test                     # Run all tests
dotnet run --project test/Enjna.AppStoreServerLibrary.Tests  # Run one suite directly (verbose output)
dotnet run --project test/Enjna.AppStoreConnectApi.Tests
```

To run a filtered subset of tests (xunit.v3 syntax):
```bash
dotnet run --project test/Enjna.AppStoreServerLibrary.Tests -- --filter-method "*MethodName*"
```

## Architecture

### Core Services (`src/Enjna.AppStoreServerLibrary/`)

- **`AppStoreServerAPIClient`**: HTTP client for Apple's App Store Server API. Creates bearer tokens with ECDsa-signed JWTs. All public methods accept an optional `bundleId` override parameter, threaded through `MakeRequestAsync` → `CreateBearerToken`.
- **`SignedDataVerifier`**: Verifies and decodes Apple-signed JWTs (transactions, renewal info, notifications, app transactions, realtime requests). Performs X.509 certificate chain validation with optional OCSP checking and caching.
- **`JWSSignatureCreator`**: Abstract base for creating signed JWS tokens. Subclasses:
  - `PromotionalOfferV2SignatureCreator`
  - `IntroductoryOfferEligibilitySignatureCreator`
  - `AdvancedCommerceInAppSignatureCreator`.
- **`PromotionalOfferSignatureCreator`**: Standalone (not a JWSSignatureCreator subclass). Creates legacy V1 promotional offer signatures using raw ECDSA over a separator-joined payload.
- **`ReceiptUtility`**: Extracts transaction IDs from legacy app receipts and transaction receipts.

### App Store Connect (`src/Enjna.AppStoreConnectApi/`)

- **`AppStoreConnectAPIClient`**: HTTP client for the App Store Connect API, split by domain across `AppStoreConnectAPIClient.*.cs` partials. Signs an ES256 bearer token per request. Built from Apple's OpenAPI specification (`spec/appstoreconnectapi/openapi.json`).
- Models follow the JSON:API envelope: `Resource<TAttributes>`, `ResourceResponse<T>`, `ResourceListResponse<T>`.
- Update-request attributes derive from `AttributeChangeSet`, which tracks assignment: a property left unassigned stays out of the request, while one assigned `null` is sent as an explicit `null` to clear the stored value.

### Models (`src/Enjna.AppStoreServerLibrary/Models/`)

- Data models are simple classes.
- Enums live in `Models/Enums/`.
- String-backed enums use `JsonEnumMemberConverter<T>`, `[EnumMember]`, and include an `_Unmapped` sentinel value for forward-compatibility with unknown API values.
- Integer-backed enums (e.g., `Status`, `OfferType`) do not use the custom converter.

### Tests (`test/Enjna.AppStoreServerLibrary.Tests/`)

- **xunit.v3** with .NET Testing Platform (`TestingPlatformDotnetTestSupport`).
- Test resources are **embedded resources** loaded via `TestUtilities.ReadResourceAsString()` / `ReadResourceAsBytes()` using dot-separated paths (e.g., `"models.signedTransaction.json"`).
- `TestUtilities.CreateSignedDataFromJson()` wraps JSON fixtures in ephemeral ES256 JWTs for decoding tests.
- `TestUtilities.GetDefaultSignedPayloadVerifier()` creates a verifier with `Environment.LocalTesting` which **skips certificate chain validation entirely**, allowing tests to decode payloads without real Apple credentials.
- `test/Enjna.AppStoreConnectApi.Tests/` follows the same conventions. `TestUtilities.GetClientWithJson()` / `GetClientWithBody()` return a client wired to a `TestHttpMessageHandler` that replays queued responses and records the requests it received.

## Code Conventions

- **File-scoped namespaces**, explicit imports (no implicit usings).
- **`ConfigureAwait(false)`** on all async calls.
- **`base.`** prefix when calling inherited methods from subclasses.
- XML doc comments on all public members (build generates documentation file).

## Coverage Against Apple's Documentation

`dotnet run --project tools/AppleSpec -- check` compares both packages against `spec/` and exits non-zero on a real gap. Deliberate omissions live in `spec/coverage.server.json`, each with its reason. A Connect operation, model type or model property that Apple deprecated must carry `[Obsolete("... Use X instead.")]`; `check` fails otherwise, reading the flag from the vendored REST and data pages as well as `openapi.json`, which omits some, every 4.4.1 deprecation among them. `[Obsolete]` on something Apple still documents as current fails too. Use the `apple-spec-coverage` agent (`.claude/agents/apple-spec-coverage.md`) to judge whether something the check reports is a gap worth closing or a scope decision worth recording.
