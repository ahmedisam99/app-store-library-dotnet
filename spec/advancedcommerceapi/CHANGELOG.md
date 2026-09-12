# Advanced Commerce API changelog

<!-- Generated from Apple's DocC render JSON by tools/AppleSpec. Do not edit. -->
<!-- page: https://developer.apple.com/documentation/advancedcommerceapi/changelog -->
<!-- data: https://developer.apple.com/tutorials/data/documentation/advancedcommerceapi/changelog.json -->

Learn about new features and updates in the Advanced Commerce API.

## Overview

Use this changelog to learn about feature updates, deprecations, and removals for the Advanced Commerce API.

## Server update – April 22, 2026

- For subscribers in South Korea, if the price increase meets the criteria that requires communication with the customer, the App Store notifies subscribers via email, price increase sheet, and push notification. Previously, developers needed to implement the price-increase communications for South Korea. For more information, see `handling-subscription-price-changes`.

## Server update – March 26, 2026

- Added the following error code: `MigrationNotAllowedWhenPriceIncreaseCommunicatedError`.

## Server update — March 5, 2026

- Added additional tax codes for books in `taxcodes`.

## Server update – January 23, 2026

- Added the following error codes: `InvalidProratedPriceForChangeItemWithEffectiveLaterError` and `FreeTrialOfferMustUsePeriodCountOfOneError`.

## 1.2 – December 10, 2025

- Added the `dependentSKUs` field to the `Change-Subscription-Price` endpoint payload for managing subscription price changes. For more information, see `handling-subscription-price-changes`.
- Added the following error codes: `ACAPriceIncreaseIsNotCurrentlySupportedInIndiaError`, `DependentSKUsCannotBeChainedError`, `DependentSKUsCannotBeSharedError`, `InvalidPriceForChangeItemInPriceIncreaseError`, `InvalidSKUProvidedMustBeCurrentSKUSetToRenewError`, `ItemCannotBeSpecifiedMultipleTimesError`, and `PriceChangeCannotBeIssuedWhenAlreadyCommunicatedError`.

## Server update - November 13, 2025

- Added support for the `https://developer.apple.com/programs/mini-apps-partner/`.

## Server update — July 2, 2025

- Added tax codes for games in `taxcodes`.

## Server update – May 5, 2025

- Added the error code `TransactionCannotBeRefundedContactSupportError`.
- Removed the unused error code `TransactionNotFoundError`.

## 1.1 — March 24, 2025

- Added the endpoints `Change-Subscription-Metadata`, `Migrate-Subscription-to-Advanced-Commerce-API`, `Request-Transaction-Refund`, and `Revoke-Subscription`, and the related data types and error codes.

## 1.0 — January 23, 2025

- Initial release.
