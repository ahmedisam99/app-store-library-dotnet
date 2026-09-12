# App Store Server API changelog

<!-- Generated from Apple's DocC render JSON by tools/AppleSpec. Do not edit. -->
<!-- page: https://developer.apple.com/documentation/appstoreserverapi/app-store-server-api-changelog -->
<!-- data: https://developer.apple.com/tutorials/data/documentation/appstoreserverapi/app-store-server-api-changelog.json -->

Learn about new features and updates in the App Store Server API.

## Overview

Use this changelog to learn about feature updates, deprecations, and removals for the App Store Server API.

### Server update  — 2026/05/05

Updated recommended domain from `api.storekit.itunes.apple.com` to `api.storekit.apple.com`, and `api.storekit-sandbox.itunes.apple.com` to `api.storekit-sandbox.apple.com`. The previous domains will continue to be supported.

### 1.21 - 2026/04/27

**New features**

- Added the following fields in `JWSTransactionDecodedPayload` to support monthly subscriptions with a 12-month commitment: `TransactionCommitmentInfo` and `billingPlanType`; and the following fields in `JWSRenewalInfoDecodedPayload`:   `RenewalCommitmentInfo` and `renewalBillingPlanType`.

### 1.20 - 2026/04/13

**New features**

- Added the `Finish-Transaction` endpoint.
- Defined a new type, `anyTransactionId`. The endpoints that previously used the `transactionId` type now use `anyTransactionId`, but the functionality is unchanged.

### 1.19 - 2025/12/10

**New features**

- Added the `Send-Consumption-Information` endpoint.
- Added the `revocationType` and `revocationPercentage` fields to the `JWSTransactionDecodedPayload`.
- Added the `advancedCommercePriceIncreaseInfo` object, and `advancedCommercePriceIncreaseInfoDependentSKU`, `advancedCommercePriceIncreaseInfoStatus`, `advancedCommercePriceIncreaseInfoPrice`, fields to the `JWSRenewalInfoDecodedPayload`.
- Use the new `Send-Consumption-Information` endpoint for App Store In-App Purchases that don’t use the Advanced Commerce API.

### 1.18 - 2025/10/29

**New features**

- Added the `ONE_TIME` value to `offerDiscountType` to indicate In-App Purchase offer codes.

### 1.17 - 2025/10/16

**New features**

- Added the `Get-App-Transaction-Info` endpoint and `AppTransactionInfoResponse` response object.

### 1.16 - 2025/06/09

**New features**

- Added the `Set-App-Account-Token` endpoint and `UpdateAppAccountTokenRequest` request object, and related error codes:  `TransactionIdIsNotOriginalTransactionIdError`, `FamilyTransactionNotSupportedError`, and `InvalidAppAccountTokenUUIDError`.

### 1.15 - 2025/02/21

**New features**

- Updated the `JWSRenewalInfoDecodedPayload` and `JWSTransactionDecodedPayload` to include the new `appTransactionId` and `offerPeriod` fields.
- Updated the `JWSRenewalInfoDecodedPayload` to include the `appAccountToken` field.
- Added the `AppTransactionIdNotSupportedError` error object.

### 1.14 - 2025/01/17

**New features**

- Added support for `AdvancedCommerceAPI`.

### 1.13 — 2024/07/08

**New features**

- Updated the `JWSRenewalInfoDecodedPayload` to include the new `eligibleWinBackOfferIds` field.
- Added the win-back offer type in `offerType`.

### 1.12 — 2024/06/10

**New features**

- Added the endpoint `Get-Transaction-History`, which provides transaction history for all In-App Purchases, including consumable In-App Purchases in a finished state.
- Added the fields `renewalPrice`, `currency` and `offerDiscountType` to the `JWSRenewalInfoDecodedPayload`.

**Deprecations**

- The `Get-Transaction-History-V1` endpoint is deprecated. Use the new `Get-Transaction-History` endpoint instead.

### 1.11  — 2024/04/11

New features

- Added the `refundPreferenceV1` field to the  `ConsumptionRequestV1` request body.
- `Send-Consumption-Information-V1` added  support for receiving information for auto-renewable subscriptions.
- Added the `InvalidTransactionTypeNotSupportedError` error object.

**Deprecations**

- The system no longer sends the `InvalidTransactionNotConsumableError` error object. It uses `InvalidTransactionTypeNotSupportedError` instead.

### 1.10.1  — 2024/03/12

- The type of the `price` field changed from `int32` to `int64`.

### Server update  — 2024/02/29

**New features**

- The `Get-Notification-History` endpoint adds support for the new notification type for unreported external purchase tokens.

### 1.10 — 2023/10/26

New features

- Added the following new properties in the decoded transaction payload `JWSTransactionDecodedPayload`: `price`, `currency`, and `offerDiscountType`.

### 1.9 — 2023/09/27

New features

- Updated the error format of the `Send-Consumption-Information-V1` endpoint to match that of other endpoints. The endpoint now returns a JSON body that can contain an error code.
- New error codes for the `Send-Consumption-Information-V1` endpoint include: `InvalidAccountTenureError`, `InvalidAppAccountTokenError`, `InvalidConsumptionStatusError`, `InvalidCustomerConsentedError`, `InvalidDeliveryStatusError`, `InvalidLifetimeDollarsPurchasedError`, `InvalidLifetimeDollarsRefundedError`, `InvalidPlatformError`, `InvalidPlayTimeError`, `InvalidSampleContentProvidedError`, `InvalidTransactionNotConsumableError`, `InvalidUserStatusError`.

### 1.8 — 2023/06/05

New features

- Added a new endpoint `Get-Transaction-Info` with its response  `TransactionInfoResponse`, which provides information about a single transaction.
- The `Get-Notification-History` endpoint adds a new filter parameter, `onlyFailures`. When you set it to `true`, the endpoint returns only the notifications that failed to reach the developer’s server.
- The following endpoints changed their path parameters from `originalTransactionId` to `transactionId`: `Get-All-Subscription-Statuses`, `Get-Transaction-History-V1`, `Get-Refund-History`, and `Send-Consumption-Information-V1`. These endpoints now accept any transaction identifier, including original transaction identifiers.
- The `Get-Notification-History` endpoint now accepts a `transactionId` instead of requiring an original transaction identifier (`originalTransactionId`) in the `NotificationHistoryRequest` body.
- The `Get-Transaction-History-V1` endpoint adds a new filter parameter, `revoked`, that filters the response to return only revoked transactions or only nonrevoked transactions.
- The `Get-All-Subscription-Statuses` endpoint adds a new filter parameter, `status`, that enables you to request subscriptions with the status values you specify.
- Added the `storefront`, `storefrontId`, and `transactionReason` fields to the `JWSTransactionDecodedPayload` object.
- Added the `renewalDate` field to the  `JWSRenewalInfoDecodedPayload` object.
- Added the `sendAttempts` field to the  `CheckTestNotificationResponse` and the `notificationHistoryResponseItem` of the `NotificationHistoryResponse` to provide information about all the send attempts for App Store Server Notifications.
- Added the error codes `FamilySharedSubscriptionExtensionIneligibleError`, `StatusRequestNotFoundError`, `InvalidStatusError`, `InvalidRevokedError`, `InvalidTransactionIdError`, `TransactionIdNotFoundError`,  and `RateLimitExceededError`.
- All endpoints are subject to a rate limit and can return a `RateLimitExceededError` with an HTTP 429 error code. For more information, see `identifying-rate-limits`.

**Deprecations**

- The `excludeRevoked` filter in `Get-Transaction-History-V1` is deprecated. Use the new `revoked` filter instead.
- The `firstSendAttemptResult` field is deprecated in the `CheckTestNotificationResponse` and `notificationHistoryResponseItem` objects. Use the first `sendAttemptItem` in the `sendAttempts` array instead.

### 1.7 — 2023/01/30

New features

- The new endpoint `Extend-Subscription-Renewal-Dates-for-All-Active-Subscribers` takes a subscription product identifier and extends the renewal date for all eligible subscribers. It responds with `MassExtendRenewalDateResponse`. For more information, see `extending-the-renewal-date-for-auto-renewable-subscriptions`. For information about new App Store server notifications related to this endpoint, see the `app-store-server-notifications-changelog`.
- The new endpoint `Get-Status-of-Subscription-Renewal-Date-Extensions` checks the status of a subscription-renewal-date extension, and responds with the `MassExtendRenewalDateStatusResponse`.

### 1.6 — 2022/08/08

New features

- The new version 2 endpoint `Get-Refund-History` returns a paginated list of refunded transactions in the `RefundHistoryResponse`.

Deprecations

- The endpoint `Get-Refund-History-V1` and its response `RefundLookupResponse` are deprecated.
- In `firstSendAttemptResult`, the `SSL_ISSUE` value is deprecated and replaced with `TLS_ISSUE`.

### 1.5 — 2022/06/06

New features

- The API has two new endpoints to support testing how your server receives App Store Server Notifications. The endpoints are: `Request-a-Test-Notification` and `Get-Test-Notification-Status`.
- The API adds the new `Get-Notification-History` endpoint.
- The `Get-Transaction-History-V1` endpoint is enhanced with new parameters to support filtering and sorting functionality.
- The `JWSRenewalInfoDecodedPayload` now includes the `recentSubscriptionStartDate` field.

### 1.4

This version doesn’t contain any public changes.

### 1.3

This version doesn’t contain any public changes.

### Server update — 2022/03/17

Removals

- The `JWSDecodedHeader` object no longer includes the `kid` field.

### 1.2 — 2022/02/24

New features

- The `JWSTransactionDecodedPayload` and `JWSRenewalInfoDecodedPayload` objects now include the `environment` field.

### Server update — 2022/02/23

- The `Get-Refund-History-V1` endpoint now returns a maximum of 50 refunded transactions.

### 1.1 — 2022/10/21

New features

- The API adds three endpoints: `Look-Up-Order-ID`, `Get-Refund-History-V1`, and `Extend-a-Subscription-Renewal-Date`.

### Server update — 2021/09/20

The API is now available in the production environment, using the following base URL:

```other
https://api.storekit.apple.com/inApps/
```

### 1.0b1 — 2021/06/07

Initial version of the App Store Server API.

New features

- This API has three endpoints, available in the sandbox environment: `Get-Transaction-History-V1`, `Send-Consumption-Information-V1`, and `Get-All-Subscription-Statuses`.
