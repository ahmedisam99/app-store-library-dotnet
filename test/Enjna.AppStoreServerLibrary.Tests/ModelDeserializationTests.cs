using System;
using System.Text.Json;
using System.Threading.Tasks;
using Enjna.AppStoreServerLibrary.Models;
using Enjna.AppStoreServerLibrary.Models.Enums;
using Xunit;
using Type = Enjna.AppStoreServerLibrary.Models.Enums.Type;

namespace Enjna.AppStoreServerLibrary.Tests;

public class ModelDeserializationTests
{
    [Fact]
    public async Task DecodeAppTransaction()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signed = TestUtilities.CreateSignedDataFromJson("models.appTransaction.json");

        var decoded =
            await verifier.VerifyAndDecodeAppTransactionAsync(signed,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(AppStoreEnvironment.LocalTesting, decoded.ReceiptType);
        Assert.Equal(531412L, decoded.AppAppleId);
        Assert.Equal("com.example", decoded.BundleId);
        Assert.Equal("1.2.3", decoded.ApplicationVersion);
        Assert.Equal(512L, decoded.VersionExternalIdentifier);
        Assert.Equal(1698148900000L, decoded.ReceiptCreationDate);
        Assert.Equal(1698148800000L, decoded.OriginalPurchaseDate);
        Assert.Equal("1.1.2", decoded.OriginalApplicationVersion);
        Assert.Equal("device_verification_value", decoded.DeviceVerification);
        Assert.Equal("48ccfa42-7431-4f22-9908-7e88983e105a", decoded.DeviceVerificationNonce);
        Assert.Equal(1698148700000L, decoded.PreorderDate);
        Assert.Equal("71134", decoded.AppTransactionId);
        Assert.Equal(PurchasePlatform.Ios, decoded.OriginalPlatform);
    }

    [Fact]
    public async Task DecodeRenewalInfo()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signed = TestUtilities.CreateSignedDataFromJson("models.signedRenewalInfo.json");

        var decoded = await verifier.VerifyAndDecodeRenewalInfoAsync(signed, TestContext.Current.CancellationToken);

        Assert.Equal(ExpirationIntent.CustomerCancelled, decoded.ExpirationIntent);
        Assert.Equal("12345", decoded.OriginalTransactionId);
        Assert.Equal("com.example.product.2", decoded.AutoRenewProductId);
        Assert.Equal("com.example.product", decoded.ProductId);
        Assert.Equal(AutoRenewStatus.On, decoded.AutoRenewStatus);
        Assert.True(decoded.IsInBillingRetryPeriod);
        Assert.Equal(PriceIncreaseStatus.CustomerHasNotResponded, decoded.PriceIncreaseStatus);
        Assert.Equal(1698148900000L, decoded.GracePeriodExpiresDate);
        Assert.Equal(OfferType.PromotionalOffer, decoded.OfferType);
        Assert.Equal("abc.123", decoded.OfferIdentifier);
        Assert.Equal(1698148800000L, decoded.SignedDate);
        Assert.Equal(AppStoreEnvironment.LocalTesting, decoded.Environment);
        Assert.Equal(1698148800000L, decoded.RecentSubscriptionStartDate);
        Assert.Equal(1698148850000L, decoded.RenewalDate);
        Assert.Equal(9990L, decoded.RenewalPrice);
        Assert.Equal("USD", decoded.Currency);
        Assert.Equal(OfferDiscountType.PayAsYouGo, decoded.OfferDiscountType);
        Assert.Equal(new[] { "eligible1", "eligible2" }, decoded.EligibleWinBackOfferIds);
        Assert.Equal("71134", decoded.AppTransactionId);
        Assert.Equal("P1Y", decoded.OfferPeriod);
        Assert.Equal("7e3fb20b-4cdb-47cc-936d-99d65f608138", decoded.AppAccountToken);
        Assert.Equal(RenewalBillingPlanType.Monthly, decoded.RenewalBillingPlanType);

        Assert.NotNull(decoded.CommitmentInfo);
        Assert.Equal("com.example.product.commitment", decoded.CommitmentInfo.CommitmentAutoRenewProductId);
        Assert.Equal(AutoRenewStatus.On, decoded.CommitmentInfo.CommitmentAutoRenewStatus);
        Assert.Equal(RenewalBillingPlanType.Monthly, decoded.CommitmentInfo.CommitmentRenewalBillingPlanType);
        Assert.Equal(1698149500000L, decoded.CommitmentInfo.CommitmentRenewalDate);
        Assert.Equal(9990L, decoded.CommitmentInfo.CommitmentRenewalPrice);

        Assert.NotNull(decoded.AdvancedCommerceInfo);
        Assert.Equal("token-abc-123", decoded.AdvancedCommerceInfo.ConsistencyToken);
        Assert.Equal("ref-12345", decoded.AdvancedCommerceInfo.RequestReferenceId);
        Assert.Equal("TAX_CODE_1", decoded.AdvancedCommerceInfo.TaxCode);
        Assert.Equal(AdvancedCommercePeriod.P1M, decoded.AdvancedCommerceInfo.Period);
        Assert.NotNull(decoded.AdvancedCommerceInfo.Descriptors);
        Assert.Equal("Premium Plan", decoded.AdvancedCommerceInfo.Descriptors.Description);
        Assert.Equal("Premium", decoded.AdvancedCommerceInfo.Descriptors.DisplayName);
        Assert.NotNull(decoded.AdvancedCommerceInfo.Items);
        Assert.Single(decoded.AdvancedCommerceInfo.Items);
        var renewalItem = decoded.AdvancedCommerceInfo.Items[0];
        Assert.Equal("com.example.sku.premium", renewalItem.Sku);
        Assert.Equal("Premium feature", renewalItem.Description);
        Assert.Equal("Premium Feature", renewalItem.DisplayName);
        Assert.Equal(9990L, renewalItem.Price);
        Assert.NotNull(renewalItem.PriceIncreaseInfo);
        Assert.Equal(new[] { "com.example.sku.1", "com.example.sku.2" }, renewalItem.PriceIncreaseInfo.DependentSkus);
        Assert.Equal(12990L, renewalItem.PriceIncreaseInfo.Price);
        Assert.Equal(AdvancedCommercePriceIncreaseInfoStatus.Pending, renewalItem.PriceIncreaseInfo.Status);
    }

    [Fact]
    public async Task DecodeTransaction()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signed = TestUtilities.CreateSignedDataFromJson("models.signedTransaction.json");

        var decoded =
            await verifier.VerifyAndDecodeTransactionAsync(signed,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("23456", decoded.TransactionId);
        Assert.Equal("12345", decoded.OriginalTransactionId);
        Assert.Equal("11111", decoded.PreviousOriginalTransactionId);
        Assert.Equal("34343", decoded.WebOrderLineItemId);
        Assert.Equal("com.example", decoded.BundleId);
        Assert.Equal("com.example.product", decoded.ProductId);
        Assert.Equal("55555", decoded.SubscriptionGroupIdentifier);
        Assert.Equal(1698148900000L, decoded.PurchaseDate);
        Assert.Equal(1698148800000L, decoded.OriginalPurchaseDate);
        Assert.Equal(1698149000000L, decoded.ExpiresDate);
        Assert.Equal(1, decoded.Quantity);
        Assert.Equal(Type.AutoRenewableSubscription, decoded.Type);
        Assert.Equal("7e3fb20b-4cdb-47cc-936d-99d65f608138", decoded.AppAccountToken);
        Assert.Equal(InAppOwnershipType.Purchased, decoded.InAppOwnershipType);
        Assert.Equal(1698148900000L, decoded.SignedDate);
        Assert.Equal(RevocationReason.RefundedDueToIssue, decoded.RevocationReason);
        Assert.Equal(1698148950000L, decoded.RevocationDate);
        Assert.True(decoded.IsUpgraded);
        Assert.Equal(OfferType.IntroductoryOffer, decoded.OfferType);
        Assert.Equal("abc.123", decoded.OfferIdentifier);
        Assert.Equal(AppStoreEnvironment.LocalTesting, decoded.Environment);
        Assert.Equal(TransactionReason.Purchase, decoded.TransactionReason);
        Assert.Equal("USA", decoded.Storefront);
        Assert.Equal("143441", decoded.StorefrontId);
        Assert.Equal(10990L, decoded.Price);
        Assert.Equal("USD", decoded.Currency);
        Assert.Equal(OfferDiscountType.PayAsYouGo, decoded.OfferDiscountType);
        Assert.Equal("71134", decoded.AppTransactionId);
        Assert.Equal("P1Y", decoded.OfferPeriod);

        Assert.Equal(BillingPlanType.Monthly, decoded.BillingPlanType);
        Assert.NotNull(decoded.CommitmentInfo);
        Assert.Equal(3, decoded.CommitmentInfo.BillingPeriodNumber);
        Assert.Equal(1698150000000L, decoded.CommitmentInfo.CommitmentExpiresDate);
        Assert.Equal(119880L, decoded.CommitmentInfo.CommitmentPrice);
        Assert.Equal(12, decoded.CommitmentInfo.TotalBillingPeriods);

        Assert.NotNull(decoded.AdvancedCommerceInfo);
        Assert.Equal("ref-12345", decoded.AdvancedCommerceInfo.RequestReferenceId);
        Assert.Equal("TAX_CODE_1", decoded.AdvancedCommerceInfo.TaxCode);
        Assert.Equal(AdvancedCommercePeriod.P1M, decoded.AdvancedCommerceInfo.Period);
        Assert.Equal(1500L, decoded.AdvancedCommerceInfo.EstimatedTax);
        Assert.Equal(8490L, decoded.AdvancedCommerceInfo.TaxExclusivePrice);
        Assert.Equal("0.15", decoded.AdvancedCommerceInfo.TaxRate);
        Assert.NotNull(decoded.AdvancedCommerceInfo.Descriptors);
        Assert.Equal("Premium Plan", decoded.AdvancedCommerceInfo.Descriptors.Description);
        Assert.Equal("Premium", decoded.AdvancedCommerceInfo.Descriptors.DisplayName);
        Assert.NotNull(decoded.AdvancedCommerceInfo.Items);
        Assert.Single(decoded.AdvancedCommerceInfo.Items);
        var txItem = decoded.AdvancedCommerceInfo.Items[0];
        Assert.Equal("com.example.sku.premium", txItem.Sku);
        Assert.Equal("Premium feature", txItem.Description);
        Assert.Equal("Premium Feature", txItem.DisplayName);
        Assert.Equal(9990L, txItem.Price);
        Assert.Equal(1698149200000L, txItem.RevocationDate);
        Assert.NotNull(txItem.Refunds);
        Assert.Single(txItem.Refunds);
        Assert.Equal(5000L, txItem.Refunds[0].RefundAmount);
        Assert.Equal(1698149100000L, txItem.Refunds[0].RefundDate);
        Assert.Equal(AdvancedCommerceRefundReason.FulfillmentIssue, txItem.Refunds[0].RefundReason);
        Assert.Equal(AdvancedCommerceRefundType.Prorated, txItem.Refunds[0].RefundType);
    }

    [Fact]
    public async Task DecodeTransactionWithRevocation()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signed = TestUtilities.CreateSignedDataFromJson("models.signedTransactionWithRevocation.json");

        var decoded =
            await verifier.VerifyAndDecodeTransactionAsync(signed,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("12345", decoded.OriginalTransactionId);
        Assert.Equal("23456", decoded.TransactionId);
        Assert.Equal("34343", decoded.WebOrderLineItemId);
        Assert.Equal("com.example", decoded.BundleId);
        Assert.Equal("com.example.product", decoded.ProductId);
        Assert.Equal("55555", decoded.SubscriptionGroupIdentifier);
        Assert.Equal(1698148800000L, decoded.OriginalPurchaseDate);
        Assert.Equal(1698148900000L, decoded.PurchaseDate);
        Assert.Equal(1698148950000L, decoded.RevocationDate);
        Assert.Equal(1698149000000L, decoded.ExpiresDate);
        Assert.Equal(1, decoded.Quantity);
        Assert.Equal(Type.AutoRenewableSubscription, decoded.Type);
        Assert.Equal("7e3fb20b-4cdb-47cc-936d-99d65f608138", decoded.AppAccountToken);
        Assert.Equal(InAppOwnershipType.Purchased, decoded.InAppOwnershipType);
        Assert.Equal(1698148900000L, decoded.SignedDate);
        Assert.Equal(RevocationReason.RefundedDueToIssue, decoded.RevocationReason);
        Assert.Equal("abc.123", decoded.OfferIdentifier);
        Assert.True(decoded.IsUpgraded);
        Assert.Equal(OfferType.IntroductoryOffer, decoded.OfferType);
        Assert.Equal("USA", decoded.Storefront);
        Assert.Equal("143441", decoded.StorefrontId);
        Assert.Equal(TransactionReason.Purchase, decoded.TransactionReason);
        Assert.Equal(AppStoreEnvironment.LocalTesting, decoded.Environment);
        Assert.Equal(10990L, decoded.Price);
        Assert.Equal("USD", decoded.Currency);
        Assert.Equal(OfferDiscountType.PayAsYouGo, decoded.OfferDiscountType);
        Assert.Equal("71134", decoded.AppTransactionId);
        Assert.Equal("P1Y", decoded.OfferPeriod);
        Assert.Equal(RevocationType.RefundProrated, decoded.RevocationType);
        Assert.Equal(50000L, decoded.RevocationPercentage);
    }

    [Fact]
    public async Task DecodeNotification_Subscribed()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signed = TestUtilities.CreateSignedDataFromJson("models.signedNotification.json");

        var decoded =
            await verifier.VerifyAndDecodeNotificationAsync(signed,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(NotificationTypeV2.Subscribed, decoded.NotificationType);
        Assert.Equal(Subtype.InitialBuy, decoded.Subtype);
        Assert.Equal("002e14d5-51f5-4503-b5a8-c3a1af68eb20", decoded.NotificationUUID);
        Assert.Equal("2.0", decoded.Version);
        Assert.Equal(1698148900000L, decoded.SignedDate);
        Assert.NotNull(decoded.Data);
        Assert.Null(decoded.Summary);
        Assert.Null(decoded.ExternalPurchaseToken);
        Assert.Equal(AppStoreEnvironment.LocalTesting, decoded.Data.Environment);
        Assert.Equal(41234L, decoded.Data.AppAppleId);
        Assert.Equal("com.example", decoded.Data.BundleId);
        Assert.Equal("1.2.3", decoded.Data.BundleVersion);
        Assert.Equal("signed_transaction_info_value", decoded.Data.SignedTransactionInfo);
        Assert.Equal("signed_renewal_info_value", decoded.Data.SignedRenewalInfo);
        Assert.Equal(Status.Active, decoded.Data.Status);
        Assert.Null(decoded.Data.ConsumptionRequestReason);
    }

    [Fact]
    public async Task DecodeNotification_ConsumptionRequest()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signed = TestUtilities.CreateSignedDataFromJson("models.signedConsumptionRequestNotification.json");

        var decoded =
            await verifier.VerifyAndDecodeNotificationAsync(signed,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(NotificationTypeV2.ConsumptionRequest, decoded.NotificationType);
        Assert.Null(decoded.Subtype);
        Assert.Equal("002e14d5-51f5-4503-b5a8-c3a1af68eb20", decoded.NotificationUUID);
        Assert.Equal("2.0", decoded.Version);
        Assert.Equal(1698148900000L, decoded.SignedDate);
        Assert.NotNull(decoded.Data);
        Assert.Null(decoded.Summary);
        Assert.Null(decoded.ExternalPurchaseToken);
        Assert.Equal(AppStoreEnvironment.LocalTesting, decoded.Data.Environment);
        Assert.Equal(41234L, decoded.Data.AppAppleId);
        Assert.Equal("com.example", decoded.Data.BundleId);
        Assert.Equal("1.2.3", decoded.Data.BundleVersion);
        Assert.Equal("signed_transaction_info_value", decoded.Data.SignedTransactionInfo);
        Assert.Equal("signed_renewal_info_value", decoded.Data.SignedRenewalInfo);
        Assert.Equal(Status.Active, decoded.Data.Status);
        Assert.Equal(ConsumptionRequestReason.UnintendedPurchase, decoded.Data.ConsumptionRequestReason);
    }

    [Fact]
    public async Task DecodeNotification_Summary()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signed = TestUtilities.CreateSignedDataFromJson("models.signedSummaryNotification.json");

        var decoded =
            await verifier.VerifyAndDecodeNotificationAsync(signed,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(NotificationTypeV2.RenewalExtension, decoded.NotificationType);
        Assert.Equal(Subtype.Summary, decoded.Subtype);
        Assert.Null(decoded.Data);
        Assert.NotNull(decoded.Summary);
        Assert.Equal(AppStoreEnvironment.LocalTesting, decoded.Summary.Environment);
        Assert.Equal(41234L, decoded.Summary.AppAppleId);
        Assert.Equal("com.example", decoded.Summary.BundleId);
        Assert.Equal("com.example.product", decoded.Summary.ProductId);
        Assert.Equal("efb27071-45a4-4aca-9854-2a1e9146f265", decoded.Summary.RequestIdentifier);
        Assert.Equal(new[] { "CAN", "USA", "MEX" }, decoded.Summary.StorefrontCountryCodes);
        Assert.Equal(5L, decoded.Summary.SucceededCount);
        Assert.Equal(2L, decoded.Summary.FailedCount);
    }

    [Fact]
    public async Task DecodeNotification_ExternalPurchaseToken_Production()
    {
        // Verifier with bundleId/appAppleId matching the token, but environment=LocalTesting.
        // The token's externalPurchaseId does NOT start with "SANDBOX", so derived env = Production.
        // bundleId/appAppleId match → passes that check, then environment mismatch → InvalidEnvironment.
        // This proves the correct values are extracted from externalPurchaseToken (not from data).
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.LocalTesting);
        var signed = TestUtilities.CreateSignedDataFromJson("models.signedExternalPurchaseTokenNotification.json");

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeNotificationAsync(signed, "com.example", 55555,
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.InvalidEnvironment, ex.Status);

        // Verify deserialization of all fields via direct JSON deserialization
        var json = TestUtilities.ReadResourceAsString("models.signedExternalPurchaseTokenNotification.json");
        var decoded = JsonSerializer.Deserialize<ResponseBodyV2DecodedPayload>(json);

        Assert.NotNull(decoded);
        Assert.Equal(NotificationTypeV2.ExternalPurchaseToken, decoded.NotificationType);
        Assert.Equal(Subtype.Unreported, decoded.Subtype);
        Assert.Equal("002e14d5-51f5-4503-b5a8-c3a1af68eb20", decoded.NotificationUUID);
        Assert.Equal("2.0", decoded.Version);
        Assert.Equal(1698148900000L, decoded.SignedDate);
        Assert.Null(decoded.Data);
        Assert.Null(decoded.Summary);
        Assert.NotNull(decoded.ExternalPurchaseToken);
        Assert.Equal("b2158121-7af9-49d4-9561-1f588205523e", decoded.ExternalPurchaseToken.ExternalPurchaseId);
        Assert.Equal(1698148950000L, decoded.ExternalPurchaseToken.TokenCreationDate);
        Assert.Equal(55555L, decoded.ExternalPurchaseToken.AppAppleId);
        Assert.Equal("com.example", decoded.ExternalPurchaseToken.BundleId);
        Assert.Equal(1698149000000L, decoded.ExternalPurchaseToken.TokenExpirationDate);
        Assert.Equal(
            DateTimeOffset.FromUnixTimeMilliseconds(1698149000000L).UtcDateTime,
            decoded.ExternalPurchaseToken.TokenExpirationDateUtc);
        Assert.Equal(TokenType.Services, decoded.ExternalPurchaseToken.TokenType);
    }

    [Fact]
    public async Task DecodeNotification_ExternalPurchaseToken_Sandbox()
    {
        // Same approach: externalPurchaseId starts with "SANDBOX_", so derived env = Sandbox.
        // Verifier env = LocalTesting → environment mismatch → InvalidEnvironment.
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.LocalTesting);
        var signed =
            TestUtilities.CreateSignedDataFromJson("models.signedExternalPurchaseTokenSandboxNotification.json");

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeNotificationAsync(signed, "com.example", 55555,
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.InvalidEnvironment, ex.Status);

        // Verify fields via deserialization
        var json = TestUtilities.ReadResourceAsString("models.signedExternalPurchaseTokenSandboxNotification.json");
        var decoded = JsonSerializer.Deserialize<ResponseBodyV2DecodedPayload>(json);

        Assert.NotNull(decoded);
        Assert.Equal(NotificationTypeV2.ExternalPurchaseToken, decoded.NotificationType);
        Assert.Equal(Subtype.Unreported, decoded.Subtype);
        Assert.Equal("002e14d5-51f5-4503-b5a8-c3a1af68eb20", decoded.NotificationUUID);
        Assert.Equal("2.0", decoded.Version);
        Assert.Equal(1698148900000L, decoded.SignedDate);
        Assert.Null(decoded.Data);
        Assert.Null(decoded.Summary);
        Assert.NotNull(decoded.ExternalPurchaseToken);
        Assert.Equal("SANDBOX_b2158121-7af9-49d4-9561-1f588205523e", decoded.ExternalPurchaseToken.ExternalPurchaseId);
        Assert.Equal(1698148950000L, decoded.ExternalPurchaseToken.TokenCreationDate);
        Assert.Equal(55555L, decoded.ExternalPurchaseToken.AppAppleId);
        Assert.Equal("com.example", decoded.ExternalPurchaseToken.BundleId);
    }

    [Fact]
    public async Task DecodeNotification_RescindConsent()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signed = TestUtilities.CreateSignedDataFromJson("models.signedRescindConsentNotification.json");

        var decoded =
            await verifier.VerifyAndDecodeNotificationAsync(signed,
                cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(NotificationTypeV2.RescindConsent, decoded.NotificationType);
        Assert.Null(decoded.Subtype);
        Assert.Equal("002e14d5-51f5-4503-b5a8-c3a1af68eb20", decoded.NotificationUUID);
        Assert.Equal("2.0", decoded.Version);
        Assert.Equal(1698148900000L, decoded.SignedDate);
        Assert.Null(decoded.Data);
        Assert.Null(decoded.Summary);
        Assert.Null(decoded.ExternalPurchaseToken);
        Assert.NotNull(decoded.AppData);
        Assert.Equal(AppStoreEnvironment.LocalTesting, decoded.AppData.Environment);
        Assert.Equal(41234L, decoded.AppData.AppAppleId);
        Assert.Equal("com.example", decoded.AppData.BundleId);
        Assert.Equal("signed_app_transaction_info_value", decoded.AppData.SignedAppTransactionInfo);
    }

    [Fact]
    public void RevocationType_DeserializesFromString()
    {
        var json = """{"revocationType":"REFUND_FULL"}""";
        var decoded = JsonSerializer.Deserialize<JWSTransactionDecodedPayload>(json);

        Assert.NotNull(decoded);
        Assert.Equal(RevocationType.RefundFull, decoded.RevocationType);
    }

    [Fact]
    public void DeserializeAppData()
    {
        var json = TestUtilities.ReadResourceAsString("models.appData.json");
        var appData = JsonSerializer.Deserialize<AppData>(json);

        Assert.NotNull(appData);
        Assert.Equal(987654321L, appData.AppAppleId);
        Assert.Equal("com.example", appData.BundleId);
        Assert.Equal(AppStoreEnvironment.Sandbox, appData.Environment);
        Assert.Equal("signed-app-transaction-info", appData.SignedAppTransactionInfo);
    }

    [Fact]
    public void AppStoreEnvironment_RoundTripsJsonEnumMemberValues()
    {
        var json = JsonSerializer.Serialize(AppStoreEnvironment.Sandbox);
        var decoded = JsonSerializer.Deserialize<AppStoreEnvironment>(json);

        Assert.Equal("\"Sandbox\"", json);
        Assert.Equal(AppStoreEnvironment.Sandbox, decoded);
    }

    [Fact]
    public void RealtimeResponseBody_Message_SerializesCorrectly()
    {
        var body = new RealtimeResponseBody
        {
            Message = new Message { MessageIdentifier = "msg-123" }
        };

        var json = JsonSerializer.Serialize(body);
        var deserialized = JsonSerializer.Deserialize<RealtimeResponseBody>(json);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.Message);
        Assert.Equal("msg-123", deserialized.Message.MessageIdentifier);
        Assert.Null(deserialized.AlternateProduct);
        Assert.Null(deserialized.PromotionalOffer);
    }

    [Fact]
    public void RealtimeResponseBody_AlternateProduct_SerializesCorrectly()
    {
        var body = new RealtimeResponseBody
        {
            AlternateProduct = new AlternateProduct
            {
                MessageIdentifier = "msg-456",
                ProductId = "com.example.alt"
            }
        };

        var json = JsonSerializer.Serialize(body);
        var deserialized = JsonSerializer.Deserialize<RealtimeResponseBody>(json);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AlternateProduct);
        Assert.Equal("msg-456", deserialized.AlternateProduct.MessageIdentifier);
        Assert.Equal("com.example.alt", deserialized.AlternateProduct.ProductId);
    }

    [Fact]
    public void RealtimeResponseBody_PromotionalOfferV2_SerializesCorrectly()
    {
        var body = new RealtimeResponseBody
        {
            PromotionalOffer = new PromotionalOffer
            {
                MessageIdentifier = "msg-789",
                PromotionalOfferSignatureV2 = "signed-offer-v2"
            }
        };

        var json = JsonSerializer.Serialize(body);
        var deserialized = JsonSerializer.Deserialize<RealtimeResponseBody>(json);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.PromotionalOffer);
        Assert.Equal("msg-789", deserialized.PromotionalOffer.MessageIdentifier);
        Assert.Equal("signed-offer-v2", deserialized.PromotionalOffer.PromotionalOfferSignatureV2);
    }

    [Fact]
    public void RealtimeResponseBody_PromotionalOfferV1_SerializesCorrectly()
    {
        var body = new RealtimeResponseBody
        {
            PromotionalOffer = new PromotionalOffer
            {
                MessageIdentifier = "msg-v1",
                PromotionalOfferSignatureV1 = new PromotionalOfferSignatureV1
                {
                    EncodedSignature = "sig",
                    ProductId = "com.example.product",
                    Nonce = "nonce-value",
                    Timestamp = 1698148900000,
                    KeyId = "key-123",
                    OfferIdentifier = "offer-1",
                    AppAccountToken = "token-abc"
                }
            }
        };

        var json = JsonSerializer.Serialize(body);
        var deserialized = JsonSerializer.Deserialize<RealtimeResponseBody>(json);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.PromotionalOffer);
        Assert.NotNull(deserialized.PromotionalOffer.PromotionalOfferSignatureV1);
        var v1 = deserialized.PromotionalOffer.PromotionalOfferSignatureV1;
        Assert.Equal("sig", v1.EncodedSignature);
        Assert.Equal("com.example.product", v1.ProductId);
        Assert.Equal("nonce-value", v1.Nonce);
        Assert.Equal(1698148900000L, v1.Timestamp);
        Assert.Equal("key-123", v1.KeyId);
        Assert.Equal("offer-1", v1.OfferIdentifier);
        Assert.Equal("token-abc", v1.AppAccountToken);
    }

    [Fact]
    public void RealtimeResponseBody_FieldNames_AreCorrect()
    {
        var body = new RealtimeResponseBody
        {
            Message = new Message { MessageIdentifier = "test" }
        };

        var json = JsonSerializer.Serialize(body);
        using var doc = JsonDocument.Parse(json);

        Assert.True(doc.RootElement.TryGetProperty("message", out _));
        Assert.True(doc.RootElement.TryGetProperty("alternateProduct", out _));
        Assert.True(doc.RootElement.TryGetProperty("promotionalOffer", out _));
        Assert.True(doc.RootElement.TryGetProperty("advancedCommerceInfo", out _));
    }

    [Fact]
    public void RealtimeResponseBody_AdvancedCommerceInfo_SerializesCorrectly()
    {
        var body = new RealtimeResponseBody
        {
            AdvancedCommerceInfo = new AdvancedCommerceInfo
            {
                MessageIdentifier = "msg-id",
                AdvancedCommerceData = "base64-encoded-jws"
            }
        };

        var json = JsonSerializer.Serialize(body);
        var deserialized = JsonSerializer.Deserialize<RealtimeResponseBody>(json);

        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.AdvancedCommerceInfo);
        Assert.Equal("msg-id", deserialized.AdvancedCommerceInfo.MessageIdentifier);
        Assert.Equal("base64-encoded-jws", deserialized.AdvancedCommerceInfo.AdvancedCommerceData);
    }
}
