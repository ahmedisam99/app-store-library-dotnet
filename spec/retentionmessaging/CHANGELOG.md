# Retention Messaging API changelog

<!-- Generated from Apple's DocC render JSON by tools/AppleSpec. Do not edit. -->
<!-- page: https://developer.apple.com/documentation/retentionmessaging/retention-messaging-changelog -->
<!-- data: https://developer.apple.com/tutorials/data/documentation/retentionmessaging/retention-messaging-changelog.json -->

Learn about new features and updates in the Retention Messaging API.

## Overview

Use this changelog to learn about feature updates, deprecations, and removals for the Retention Messaging API.

### Server update  — 2026/05/05

Updated recommended domain from `api.storekit.itunes.apple.com` to `api.storekit.apple.com`, and `api.storekit-sandbox.itunes.apple.com` to `api.storekit-sandbox.apple.com`. The previous domains will continue to be supported.

### 1.5 2026/04/27

**New features**

- Added the new property `billingPlanType` to `alternateProduct` to support monthly subscriptions with 12-month commitments.

### 1.4  2026/03/31

**New features**

- Added `Configure-Realtime-URL`, `Get-Realtime-URL`, `Delete-Realtime-URL`, and `Get-Default-Message` endpoints.
- Added objects, properties, and types related to the new endpoints, including: `BulletPoint`, `bulletPointText`, `headerPosition`, `imageSize`, `realtimeURL`, and `RealtimeUrlRequest`.
- Added `error-codes` to indicate bad requests and other errors related to the added endpoints.
- Updated `GetImageListResponseItem` and `Upload-Image` to add support for the `imageSize` parameter.
- Updated `UploadMessageRequestBody` to add support for the `bulletPoints` and `headerPosition` parameters.

### 1.3  2025/12/09

**New features**

- The framework now supports the ability to test server response times for real time retention messaging using the `Initiate-Performance-Test` and `Get-Performance-Test-Results` endpoints.

### 1.2  2025/11/05

**New features**

- Updated the `RealtimeResponseBody` to include the `advancedCommerceInfo` object.

### 1.1  2025/09/04

**New features**

- Updated the `DecodedRealtimeRequestBody` to include the `environment` and `signedDate` fields.

### 1.0  2025/07/16

Initial pre-release.
