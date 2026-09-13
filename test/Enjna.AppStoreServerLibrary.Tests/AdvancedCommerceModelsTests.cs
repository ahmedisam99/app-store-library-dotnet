using System.Text.Json;
using Enjna.AppStoreServerLibrary.Models;
using Enjna.AppStoreServerLibrary.Models.Enums;
using Xunit;

namespace Enjna.AppStoreServerLibrary.Tests;

public class AdvancedCommerceModelsTests
{
    private static T Deserialize<T>(string resourcePath)
    {
        var json = TestUtilities.ReadResourceAsString(resourcePath);
        var result = JsonSerializer.Deserialize<T>(json);
        Assert.NotNull(result);
        return result;
    }

    [Fact]
    public void DeserializesAdvancedCommerceDescriptors()
    {
        var model = Deserialize<AdvancedCommerceDescriptors>("models.advancedCommerceDescriptors.json");
        Assert.False(string.IsNullOrEmpty(model.Description));
        Assert.False(string.IsNullOrEmpty(model.DisplayName));
    }

    [Fact]
    public void DeserializesAdvancedCommerceOffer()
    {
        var model = Deserialize<AdvancedCommerceOffer>("models.advancedCommerceOffer.json");
        Assert.NotNull(model.Period);
        Assert.NotNull(model.PeriodCount);
        Assert.NotNull(model.Price);
        Assert.NotNull(model.Reason);
    }

    [Fact]
    public void DeserializesOneTimeChargeCreateRequest()
    {
        var model = Deserialize<AdvancedCommerceOneTimeChargeCreateRequest>(
            "models.advancedCommerceOneTimeChargeCreateRequest.json");
        Assert.Equal("USD", model.Currency);
        Assert.Equal("taxCode", model.TaxCode);
        Assert.Equal("USA", model.Storefront);
        Assert.Equal("CREATE", model.Operation);
        Assert.Equal("1.0", model.Version);
        Assert.Equal("550e8400-e29b-41d4-a716-446655440000", model.RequestInfo.RequestReferenceId);
        Assert.Equal("sku", model.Item.Sku);
        Assert.Equal("description", model.Item.Description);
        Assert.Equal("display name", model.Item.DisplayName);
        Assert.Equal(10000L, model.Item.Price);
    }

    [Fact]
    public void DeserializesOneTimeChargeItem()
    {
        var model = Deserialize<AdvancedCommerceOneTimeChargeItem>("models.advancedCommerceOneTimeChargeItem.json");
        Assert.Equal("sku", model.Sku);
        Assert.True(model.Price > 0);
    }

    [Fact]
    public void DeserializesRequestInfo()
    {
        var model = Deserialize<AdvancedCommerceRequestInfo>("models.advancedCommerceRequestInfo.json");
        Assert.False(string.IsNullOrEmpty(model.RequestReferenceId));
    }

    [Fact]
    public void DeserializesRequestRefundItem()
    {
        var model = Deserialize<AdvancedCommerceRequestRefundItem>("models.advancedCommerceRequestRefundItem.json");
        Assert.Equal("sku", model.Sku);
        Assert.NotNull(model.RefundReason);
        Assert.NotNull(model.RefundType);
    }

    [Fact]
    public void DeserializesRequestRefundRequest()
    {
        var model =
            Deserialize<AdvancedCommerceRequestRefundRequest>("models.advancedCommerceRequestRefundRequest.json");
        Assert.Equal("USD", model.Currency);
        Assert.True(model.RefundRiskingPreference);
        Assert.Equal("USA", model.Storefront);
        Assert.Equal(2, model.Items.Length);
        Assert.Equal(AdvancedCommerceRefundReason.Legal, model.Items[0].RefundReason);
        Assert.Equal(AdvancedCommerceRefundType.Full, model.Items[0].RefundType);
        Assert.True(model.Items[0].Revoke);
        Assert.Equal(AdvancedCommerceRefundReason.Other, model.Items[1].RefundReason);
        Assert.Equal(AdvancedCommerceRefundType.Prorated, model.Items[1].RefundType);
        Assert.False(model.Items[1].Revoke);
    }

    [Fact]
    public void DeserializesRequestRefundResponse()
    {
        var model = Deserialize<AdvancedCommerceRequestRefundResponse>(
            "models.advancedCommerceRequestRefundResponse.json");
        Assert.Equal("signed_transaction_info_value", model.SignedTransactionInfo);
    }

    [Fact]
    public void DeserializesSubscriptionCancelRequest()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionCancelRequest>(
            "models.advancedCommerceSubscriptionCancelRequest.json");
        Assert.NotNull(model.RequestInfo.RequestReferenceId);
    }

    [Fact]
    public void DeserializesSubscriptionCancelResponse()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionCancelResponse>(
            "models.advancedCommerceSubscriptionCancelResponse.json");
        Assert.Equal("signed_renewal_info", model.SignedRenewalInfo);
        Assert.Equal("signed_transaction_info", model.SignedTransactionInfo);
    }

    [Fact]
    public void DeserializesSubscriptionChangeMetadataRequest()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionChangeMetadataRequest>(
            "models.advancedCommerceSubscriptionChangeMetadataRequest.json");
        Assert.NotNull(model.RequestInfo.RequestReferenceId);
    }

    [Fact]
    public void DeserializesSubscriptionChangeMetadataResponse()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionChangeMetadataResponse>(
            "models.advancedCommerceSubscriptionChangeMetadataResponse.json");
        Assert.NotNull(model.SignedTransactionInfo);
    }

    [Fact]
    public void DeserializesSubscriptionCreateItem()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionCreateItem>(
            "models.advancedCommerceSubscriptionCreateItem.json");
        Assert.Equal("sku", model.Sku);
        Assert.True(model.Price > 0);
    }

    [Fact]
    public void DeserializesSubscriptionCreateRequest()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionCreateRequest>(
            "models.advancedCommerceSubscriptionCreateRequest.json");
        Assert.Equal("USD", model.Currency);
        Assert.Equal("taxCode", model.TaxCode);
        Assert.Equal("USA", model.Storefront);
        Assert.Equal("transactionId", model.PreviousTransactionId);
        Assert.Equal(AdvancedCommercePeriod.P1M, model.Period);
        Assert.Equal(2, model.Items.Length);
        Assert.Equal("description", model.Descriptors.Description);
        Assert.Equal("display name", model.Descriptors.DisplayName);
    }

    [Fact]
    public void DeserializesSubscriptionMigrateRequest()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionMigrateRequest>(
            "models.advancedCommerceSubscriptionMigrateRequest.json");
        Assert.Equal("targetProductId", model.TargetProductId);
        Assert.Equal("taxCode", model.TaxCode);
        Assert.Single(model.Items);
        Assert.Equal("sku", model.Items[0].Sku);
    }

    [Fact]
    public void DeserializesSubscriptionMigrateResponse()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionMigrateResponse>(
            "models.advancedCommerceSubscriptionMigrateResponse.json");
        Assert.NotNull(model.SignedTransactionInfo);
    }

    [Fact]
    public void DeserializesSubscriptionModifyInAppRequest()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionModifyInAppRequest>(
            "models.advancedCommerceSubscriptionModifyInAppRequest.json");
        Assert.Equal("transactionId", model.TransactionId);
        Assert.True(model.RetainBillingCycle);
        Assert.Equal("USD", model.Currency);
        Assert.Equal("taxCode", model.TaxCode);
        Assert.NotNull(model.Descriptors);
        Assert.Equal(AdvancedCommerceEffective.Immediately, model.Descriptors.Effective);
    }

    [Fact]
    public void DeserializesSubscriptionModifyChangeItem()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionModifyChangeItem>(
            "models.advancedCommerceSubscriptionModifyChangeItem.json");
        Assert.Equal("sku", model.Sku);
        Assert.True(model.Price > 0);
    }

    [Fact]
    public void DeserializesSubscriptionPriceChangeRequest()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionPriceChangeRequest>(
            "models.advancedCommerceSubscriptionPriceChangeRequest.json");
        Assert.Equal("USD", model.Currency);
        Assert.Single(model.Items);
        Assert.Equal("sku123", model.Items[0].Sku);
        Assert.Equal(15000L, model.Items[0].Price);
    }

    [Fact]
    public void DeserializesSubscriptionPriceChangeResponse()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionPriceChangeResponse>(
            "models.advancedCommerceSubscriptionPriceChangeResponse.json");
        Assert.NotNull(model.SignedTransactionInfo);
    }

    [Fact]
    public void DeserializesSubscriptionReactivateInAppRequest()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionReactivateInAppRequest>(
            "models.advancedCommerceSubscriptionReactivateInAppRequest.json");
        Assert.Equal("transactionId", model.TransactionId);
        Assert.NotNull(model.Items);
        Assert.Single(model.Items);
        Assert.Equal("sku", model.Items[0].Sku);
    }

    [Fact]
    public void DeserializesSubscriptionRevokeRequest()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionRevokeRequest>(
            "models.advancedCommerceSubscriptionRevokeRequest.json");
        Assert.True(model.RefundRiskingPreference);
        Assert.Equal(AdvancedCommerceRefundReason.Legal, model.RefundReason);
        Assert.Equal(AdvancedCommerceRefundType.Full, model.RefundType);
        Assert.Equal("USA", model.Storefront);
    }

    [Fact]
    public void DeserializesSubscriptionRevokeResponse()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionRevokeResponse>(
            "models.advancedCommerceSubscriptionRevokeResponse.json");
        Assert.NotNull(model.SignedTransactionInfo);
    }

    [Fact]
    public void DeserializesPerformanceTestResponse()
    {
        var model = Deserialize<PerformanceTestResponse>("models.performanceTestResponse.json");
        Assert.Equal("c4b87a1d-2e3f-4a5b-9c6d-7e8f9a0b1c2d", model.RequestId);
        Assert.NotNull(model.Config);
        Assert.Equal(10L, model.Config.MaxConcurrentRequests);
        Assert.Equal(100L, model.Config.TotalRequests);
        Assert.Equal(60000L, model.Config.TotalDuration);
        Assert.Equal(500L, model.Config.ResponseTimeThreshold);
        Assert.Equal(95L, model.Config.SuccessRateThreshold);
    }

    [Fact]
    public void DeserializesPerformanceTestResultResponse()
    {
        var model = Deserialize<PerformanceTestResultResponse>("models.performanceTestResultResponse.json");
        Assert.Equal("https://example.com/retention", model.Target);
        Assert.Equal(PerformanceTestStatus.Pass, model.Result);
        Assert.Equal(98L, model.SuccessRate);
        Assert.Equal(0L, model.NumPending);
        Assert.NotNull(model.ResponseTimes);
        Assert.Equal(120L, model.ResponseTimes.Average);
        Assert.Equal(100L, model.ResponseTimes.P50);
        Assert.Equal(200L, model.ResponseTimes.P90);
        Assert.Equal(250L, model.ResponseTimes.P95);
        Assert.Equal(400L, model.ResponseTimes.P99);
        Assert.NotNull(model.Failures);
        Assert.Equal(1L, model.Failures["TIMED_OUT"]);
        Assert.Equal(1L, model.Failures["NO_RESPONSE"]);
    }

    [Fact]
    public void DeserializesGetDefaultMessageResponse()
    {
        var model = Deserialize<DefaultConfigurationResponse>("models.getDefaultMessageResponse.json");
        Assert.Equal("a1b2c3d4-e5f6-7890-a1b2-c3d4e5f67890", model.MessageIdentifier);
    }

    [Fact]
    public void DeserializesGetRealtimeUrlResponse()
    {
        var model = Deserialize<RealtimeUrlResponse>("models.getRealtimeUrlResponse.json");
        Assert.Equal("https://example.com/realtime", model.RealtimeUrl);
    }

    [Fact]
    public void DeserializesGetImageListResponseWithImageSize()
    {
        var model = Deserialize<GetImageListResponse>("models.getImageListResponse.json");
        Assert.NotNull(model.ImageIdentifiers);
        Assert.Equal(2, model.ImageIdentifiers.Length);
        Assert.Equal(ImageSize.FullSize, model.ImageIdentifiers[0].ImageSize);
    }

    [Fact]
    public void DeserializesSubscriptionMigrateDescriptors()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionMigrateDescriptors>(
            "models.advancedCommerceSubscriptionMigrateDescriptors.json");
        Assert.Equal("description", model.Description);
        Assert.Equal("displayName", model.DisplayName);
    }

    [Fact]
    public void DeserializesSubscriptionMigrateItem()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionMigrateItem>(
            "models.advancedCommerceSubscriptionMigrateItem.json");
        Assert.Equal("sku", model.Sku);
        Assert.Equal("description", model.Description);
        Assert.Equal("displayName", model.DisplayName);
    }

    [Fact]
    public void DeserializesSubscriptionMigrateRenewalItem()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionMigrateRenewalItem>(
            "models.advancedCommerceSubscriptionMigrateRenewalItem.json");
        Assert.Equal("sku", model.Sku);
        Assert.Equal("description", model.Description);
        Assert.Equal("displayName", model.DisplayName);
    }

    [Fact]
    public void DeserializesSubscriptionModifyAddItem()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionModifyAddItem>(
            "models.advancedCommerceSubscriptionModifyAddItem.json");
        Assert.Equal("sku", model.Sku);
        Assert.Equal("description", model.Description);
        Assert.Equal("displayName", model.DisplayName);
        Assert.Equal(12000L, model.Price);
    }

    [Fact]
    public void DeserializesSubscriptionModifyDescriptors()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionModifyDescriptors>(
            "models.advancedCommerceSubscriptionModifyDescriptors.json");
        Assert.Equal("description", model.Description);
        Assert.Equal("displayName", model.DisplayName);
        Assert.Equal(AdvancedCommerceEffective.Immediately, model.Effective);
    }

    [Fact]
    public void DeserializesSubscriptionModifyPeriodChange()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionModifyPeriodChange>(
            "models.advancedCommerceSubscriptionModifyPeriodChange.json");
        Assert.Equal(AdvancedCommercePeriod.P3M, model.Period);
        Assert.Equal(AdvancedCommerceEffective.Immediately, model.Effective);
    }

    [Fact]
    public void DeserializesSubscriptionModifyRemoveItem()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionModifyRemoveItem>(
            "models.advancedCommerceSubscriptionModifyRemoveItem.json");
        Assert.Equal("sku", model.Sku);
    }

    [Fact]
    public void DeserializesSubscriptionPriceChangeItem()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionPriceChangeItem>(
            "models.advancedCommerceSubscriptionPriceChangeItem.json");
        Assert.Equal("sku", model.Sku);
        Assert.Equal(16000L, model.Price);
        Assert.NotNull(model.DependentSkus);
        Assert.Equal("dependentSKU", model.DependentSkus[0]);
    }

    [Fact]
    public void DeserializesSubscriptionReactivateItem()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionReactivateItem>(
            "models.advancedCommerceSubscriptionReactivateItem.json");
        Assert.Equal("sku", model.Sku);
    }

    [Fact]
    public void DeserializesSubscriptionChangeMetadataDescriptors()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionChangeMetadataDescriptors>(
            "models.advancedCommerceSubscriptionChangeMetadataDescriptors.json");
        Assert.Equal("description", model.Description);
        Assert.Equal("displayName", model.DisplayName);
        Assert.Equal(AdvancedCommerceEffective.Immediately, model.Effective);
    }

    [Fact]
    public void DeserializesSubscriptionChangeMetadataItem()
    {
        var model = Deserialize<AdvancedCommerceSubscriptionChangeMetadataItem>(
            "models.advancedCommerceSubscriptionChangeMetadataItem.json");
        Assert.Equal("sku", model.Sku);
        Assert.Equal("currentSku", model.CurrentSku);
        Assert.Equal("description", model.Description);
        Assert.Equal("displayName", model.DisplayName);
        Assert.Equal(AdvancedCommerceEffective.NextBillCycle, model.Effective);
    }
}
