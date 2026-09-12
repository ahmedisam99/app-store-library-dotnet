# App Store Server Notifications changelog

<!-- Generated from Apple's DocC render JSON by tools/AppleSpec. Do not edit. -->
<!-- page: https://developer.apple.com/documentation/appstoreservernotifications/app-store-server-notifications-changelog -->
<!-- data: https://developer.apple.com/tutorials/data/documentation/appstoreservernotifications/app-store-server-notifications-changelog.json -->

Learn about changes to the App Store Server Notifications service.

## Overview

App Store Server Notifications has two versions of notifications. Version 1 notifications and the `App-Store-Server-Notifications-V1` endpoint are deprecated. Instead, implement the `App-Store-Server-Notifications-V2` endpoint on your server to receive version 2 notifications.

To set up your server to receive notifications, see `enabling-app-store-server-notifications`. Use this changelog to learn about feature updates, version information, deprecations, and removals for App Store Server Notifications.

### April 27, 2026

**New features**

- Added the following fields to support monthly subscriptions with a 12-month commitment: `TransactionCommitmentInfo` and `billingPlanType` in `JWSTransactionDecodedPayload`; `RenewalCommitmentInfo` and `renewalBillingPlanType` in `JWSRenewalInfoDecodedPayload`.

### December 10, 2025

**New features**

- Added the `revocationType` and `revocationPercentage` fields to the `JWSTransactionDecodedPayload`.
- Added the `advancedCommercePriceIncreaseInfo` object, and `advancedCommercePriceIncreaseInfoDependentSKU`, `advancedCommercePriceIncreaseInfoStatus`, `advancedCommercePriceIncreaseInfoPrice`, fields to the `JWSRenewalInfoDecodedPayload`.

### November 4, 2025

**New features**

- Updated the `responseBodyV2DecodedPayload` to include the new payload object, `appData`.
- Added the notification type `RESCIND_CONSENT` to `notificationType`.

### October 29, 2025

**New features**

- Added the `ONE_TIME` value to `offerDiscountType` to indicate In-App Purchase offer codes.

### June 26, 2025

**New features**

- Added the `ACTIVE_TOKEN_REMINDER` and `CREATED` values to `subtype`, which can appear in notifications with an `EXTERNAL_PURCHASE_TOKEN` `notificationType`.
- Updated `externalPurchaseToken` to include the new fields `tokenType` and `tokenExpirationDate`.

### May 27, 2025

**New features**

- The `ONE_TIME_CHARGE` `notificationType` is now available in the production environment.

### March 24, 2025

**New features**

- Added the notification types `METADATA_UPDATE` and `MIGRATE` to `notificationType`.
- Added the `previousOriginalTransactionId` field to the `JWSTransactionDecodedPayload`.

### February 21, 2025

**New features**

- Updated the `JWSRenewalInfoDecodedPayload` and `JWSTransactionDecodedPayload` to include the new `appTransactionId` and `offerPeriod` fields.
- Updated the `JWSRenewalInfoDecodedPayload` to include the `appAccountToken` field.

### January 17, 2025

**New features**

- Added support for the `AdvancedCommerceAPI`.

### July 8, 2024

**New features**

- Updated the `JWSRenewalInfoDecodedPayload` to include the new field `eligibleWinBackOfferIds`.
- Added the win-back offer type to `offerType`.

### June 10, 2024

**New features**

- Added the notification type `ONE_TIME_CHARGE` to `notificationType`. This notification type is currently available only in the sandbox environment.
- Added the fields `renewalPrice`, `currency`, and `offerDiscountType` to the `JWSRenewalInfoDecodedPayload`.

### April 11, 2024

**New features**

- Added the `consumptionRequestReason` to the `data` object.
- The `CONSUMPTION_REQUEST` `notificationType` added notifications for refund requests for auto-renewable subscriptions.

### March 12, 2024

**New features**

- The type of the `price` field changed from `int32` to `int64`.

### February 29, 2024

**New features**

- Added a new `notificationType`:  `EXTERNAL_PURCHASE_TOKEN` and a `subtype`: `UNREPORTED`.
- Updated the `responseBodyV2DecodedPayload` to include the new payload object, `externalPurchaseToken`.
- Added the types `externalPurchaseId` and `tokenCreationDate`.

### January 23, 2024

**New features**

- Changed the notification type the App Store server sends when a customer redeems a subscription offer for an inactive subscription to the `SUBSCRIBED` `notificationType`. The App Store server only sends the `OFFER_REDEEMED` notification type when customers redeem an offer on an active subscription.

### October 26, 2023

**New features**

- Added new properties in the `JWSTransactionDecodedPayload` object: `price`, `currency`, and `offerDiscountType`.

### June 5, 2023

**New features**

- Added a new version 2 `notificationType`, `REFUND_REVERSED`.
- Added the following new fields in the transaction decoded payload, `JWSTransactionDecodedPayload`: `storefront`, `storefrontId`, and `transactionReason`.
- Added the `renewalDate` field in the renewal info decoded payload, `JWSRenewalInfoDecodedPayload`.
- Added a subscription `status` field in the `data` object of the `responseBodyV2DecodedPayload`.
- The `responseBodyV1` now includes a `deprecation` field.

**Deprecations**

- The `App-Store-Server-Notifications-V1` endpoint and version 1 notifications are deprecated. Implement the `App-Store-Server-Notifications-V2` endpoint on your server to receive version 2 notifications instead.

### January 30, 2023

**New features**

- Added a new notification type for App Store Server Notifications 2 that consists of the `notificationType` value `RENEWAL_EXTENSION` and `subtype` values of `SUMMARY` and `FAILURE`. This notification provides information when you extend the subscription renewal date for all active subscribers, based on a product identifier. For more information, see `Extend-Subscription-Renewal-Dates-for-All-Active-Subscribers` in the `AppStoreServerAPI`.
- Updated the `responseBodyV2DecodedPayload` to include the new `summary` object, which appears in the payload for a `RENEWAL_EXTENSION` notification with a `SUMMARY` `subtype`.

### November 7, 2022

**New features**

- Added the `PRODUCT_NOT_FOR_SALE` `subtype` for the `EXPIRED` `notificationType`.

### June 6, 2022

**New features**

- App Store Server Notifications 2 supports sending a `TEST` notification. For more information, see `notificationType`, and the endpoints `Request-a-Test-Notification` and `Get-Test-Notification-Status` in the `AppStoreServerAPI`.

### May 12, 2022

**New features**

- In App Store Server Notifications 2, the notification subtype `ACCEPTED` is now sent when the App Store notifies the customer of an auto-renewable subscription price increase that doesn’t require customer consent. This notification subtype is available only in version 2 notifications. For more information, see `subtype`.

### October 21, 2021 - version 2

**New features**

- `App-Store-Server-Notifications-V2` is available, and version 1 is still supported. For information about the notifications sent in version 2, see `notificationType`, `substate`, and `responseBodyV2`.
- For information about the notifications sent in version 1, see `notification_type` and `responseBodyV1` (previously named `responseBody`).

### March 10, 2021

**Deprecations**

- In `app-store-server-notifications-version-1`, the following notification type and top-level objects are deprecated and removed: `RENEWAL,latest_receipt`, `latest_receipt_info`, `latest_expired_receipt`, and `latest_expired_receipt_info`. For more information, see `responseBodyV1` and `notification_type`.

### November 21, 2019 - version 1

**New features**

- App Store Server Notifications is available.
