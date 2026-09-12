using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Enjna.AppStoreServerLibrary.Models;
using Enjna.AppStoreServerLibrary.Models.Enums;
using Xunit;

namespace Enjna.AppStoreServerLibrary.Tests;

public class SignedDataVerifierTests
{
    #region Certificate constants (from Node test suite)

    private const string ROOT_CA_BASE64 =
        "MIIBgjCCASmgAwIBAgIJALUc5ALiH5pbMAoGCCqGSM49BAMDMDYxCzAJBgNVBAYTAlVTMRMwEQYDVQQIDApDYWxpZm9ybmlhMRIwEAYDVQQHDAlDdXBlcnRpbm8wHhcNMjMwMTA1MjEzMDIyWhcNMzMwMTAyMjEzMDIyWjA2MQswCQYDVQQGEwJVUzETMBEGA1UECAwKQ2FsaWZvcm5pYTESMBAGA1UEBwwJQ3VwZXJ0aW5vMFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEc+/Bl+gospo6tf9Z7io5tdKdrlN1YdVnqEhEDXDShzdAJPQijamXIMHf8xWWTa1zgoYTxOKpbuJtDplz1XriTaMgMB4wDAYDVR0TBAUwAwEB/zAOBgNVHQ8BAf8EBAMCAQYwCgYIKoZIzj0EAwMDRwAwRAIgemWQXnMAdTad2JDJWng9U4uBBL5mA7WI05H7oH7c6iQCIHiRqMjNfzUAyiu9h6rOU/K+iTR0I/3Y/NSWsXHX+acc";

    private const string INTERMEDIATE_CA_BASE64 =
        "MIIBnzCCAUWgAwIBAgIBCzAKBggqhkjOPQQDAzA2MQswCQYDVQQGEwJVUzETMBEGA1UECAwKQ2FsaWZvcm5pYTESMBAGA1UEBwwJQ3VwZXJ0aW5vMB4XDTIzMDEwNTIxMzEwNVoXDTMzMDEwMTIxMzEwNVowRTELMAkGA1UEBhMCVVMxCzAJBgNVBAgMAkNBMRIwEAYDVQQHDAlDdXBlcnRpbm8xFTATBgNVBAoMDEludGVybWVkaWF0ZTBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABBUN5V9rKjfRiMAIojEA0Av5Mp0oF+O0cL4gzrTF178inUHugj7Et46NrkQ7hKgMVnjogq45Q1rMs+cMHVNILWqjNTAzMA8GA1UdEwQIMAYBAf8CAQAwDgYDVR0PAQH/BAQDAgEGMBAGCiqGSIb3Y2QGAgEEAgUAMAoGCCqGSM49BAMDA0gAMEUCIQCmsIKYs41ullssHX4rVveUT0Z7Is5/hLK1lFPTtun3hAIgc2+2RG5+gNcFVcs+XJeEl4GZ+ojl3ROOmll+ye7dynQ=";

    private const string LEAF_CERT_BASE64 =
        "MIIBoDCCAUagAwIBAgIBDDAKBggqhkjOPQQDAzBFMQswCQYDVQQGEwJVUzELMAkGA1UECAwCQ0ExEjAQBgNVBAcMCUN1cGVydGlubzEVMBMGA1UECgwMSW50ZXJtZWRpYXRlMB4XDTIzMDEwNTIxMzEzNFoXDTMzMDEwMTIxMzEzNFowPTELMAkGA1UEBhMCVVMxCzAJBgNVBAgMAkNBMRIwEAYDVQQHDAlDdXBlcnRpbm8xDTALBgNVBAoMBExlYWYwWTATBgcqhkjOPQIBBggqhkjOPQMBBwNCAATitYHEaYVuc8g9AjTOwErMvGyPykPa+puvTI8hJTHZZDLGas2qX1+ErxgQTJgVXv76nmLhhRJH+j25AiAI8iGsoy8wLTAJBgNVHRMEAjAAMA4GA1UdDwEB/wQEAwIHgDAQBgoqhkiG92NkBgsBBAIFADAKBggqhkjOPQQDAwNIADBFAiBX4c+T0Fp5nJ5QRClRfu5PSByRvNPtuaTsk0vPB3WAIAIhANgaauAj/YP9s0AkEhyJhxQO/6Q2zouZ+H1CIOehnMzQ";

    private const string INTERMEDIATE_CA_INVALID_OID_BASE64 =
        "MIIBnjCCAUWgAwIBAgIBDTAKBggqhkjOPQQDAzA2MQswCQYDVQQGEwJVUzETMBEGA1UECAwKQ2FsaWZvcm5pYTESMBAGA1UEBwwJQ3VwZXJ0aW5vMB4XDTIzMDEwNTIxMzYxNFoXDTMzMDEwMTIxMzYxNFowRTELMAkGA1UEBhMCVVMxCzAJBgNVBAgMAkNBMRIwEAYDVQQHDAlDdXBlcnRpbm8xFTATBgNVBAoMDEludGVybWVkaWF0ZTBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABBUN5V9rKjfRiMAIojEA0Av5Mp0oF+O0cL4gzrTF178inUHugj7Et46NrkQ7hKgMVnjogq45Q1rMs+cMHVNILWqjNTAzMA8GA1UdEwQIMAYBAf8CAQAwDgYDVR0PAQH/BAQDAgEGMBAGCiqGSIb3Y2QGAgIEAgUAMAoGCCqGSM49BAMDA0cAMEQCIFROtTE+RQpKxNXETFsf7Mc0h+5IAsxxo/X6oCC/c33qAiAmC5rn5yCOOEjTY4R1H1QcQVh+eUwCl13NbQxWCuwxxA==";

    private const string LEAF_CERT_FOR_INTERMEDIATE_CA_INVALID_OID_BASE64 =
        "MIIBnzCCAUagAwIBAgIBDjAKBggqhkjOPQQDAzBFMQswCQYDVQQGEwJVUzELMAkGA1UECAwCQ0ExEjAQBgNVBAcMCUN1cGVydGlubzEVMBMGA1UECgwMSW50ZXJtZWRpYXRlMB4XDTIzMDEwNTIxMzY1OFoXDTMzMDEwMTIxMzY1OFowPTELMAkGA1UEBhMCVVMxCzAJBgNVBAgMAkNBMRIwEAYDVQQHDAlDdXBlcnRpbm8xDTALBgNVBAoMBExlYWYwWTATBgcqhkjOPQIBBggqhkjOPQMBBwNCAATitYHEaYVuc8g9AjTOwErMvGyPykPa+puvTI8hJTHZZDLGas2qX1+ErxgQTJgVXv76nmLhhRJH+j25AiAI8iGsoy8wLTAJBgNVHRMEAjAAMA4GA1UdDwEB/wQEAwIHgDAQBgoqhkiG92NkBgsBBAIFADAKBggqhkjOPQQDAwNHADBEAiAUAs+gzYOsEXDwQquvHYbcVymyNqDtGw9BnUFp2YLuuAIgXxQ3Ie9YU0cMqkeaFd+lyo0asv9eyzk6stwjeIeOtTU=";

    private const string LEAF_CERT_INVALID_OID_BASE64 =
        "MIIBoDCCAUagAwIBAgIBDzAKBggqhkjOPQQDAzBFMQswCQYDVQQGEwJVUzELMAkGA1UECAwCQ0ExEjAQBgNVBAcMCUN1cGVydGlubzEVMBMGA1UECgwMSW50ZXJtZWRpYXRlMB4XDTIzMDEwNTIxMzczMVoXDTMzMDEwMTIxMzczMVowPTELMAkGA1UEBhMCVVMxCzAJBgNVBAgMAkNBMRIwEAYDVQQHDAlDdXBlcnRpbm8xDTALBgNVBAoMBExlYWYwWTATBgcqhkjOPQIBBggqhkjOPQMBBwNCAATitYHEaYVuc8g9AjTOwErMvGyPykPa+puvTI8hJTHZZDLGas2qX1+ErxgQTJgVXv76nmLhhRJH+j25AiAI8iGsoy8wLTAJBgNVHRMEAjAAMA4GA1UdDwEB/wQEAwIHgDAQBgoqhkiG92NkBgsCBAIFADAKBggqhkjOPQQDAwNIADBFAiAb+7S3i//bSGy7skJY9+D4VgcQLKFeYfIMSrUCmdrFqwIhAIMVwzD1RrxPRtJyiOCXLyibIvwcY+VS73HYfk0O9lgz";

    private const string LEAF_CERT_PUBLIC_KEY_BASE64 =
        "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE4rWBxGmFbnPIPQI0zsBKzLxsj8pD2vqbr0yPISUx2WQyxmrNql9fhK8YEEyYFV7++p5i4YUSR/o9uQIgCPIhrA==";

    private const string REAL_APPLE_ROOT_BASE64 =
        "MIICQzCCAcmgAwIBAgIILcX8iNLFS5UwCgYIKoZIzj0EAwMwZzEbMBkGA1UEAwwSQXBwbGUgUm9vdCBDQSAtIEczMSYwJAYDVQQLDB1BcHBsZSBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTETMBEGA1UECgwKQXBwbGUgSW5jLjELMAkGA1UEBhMCVVMwHhcNMTQwNDMwMTgxOTA2WhcNMzkwNDMwMTgxOTA2WjBnMRswGQYDVQQDDBJBcHBsZSBSb290IENBIC0gRzMxJjAkBgNVBAsMHUFwcGxlIENlcnRpZmljYXRpb24gQXV0aG9yaXR5MRMwEQYDVQQKDApBcHBsZSBJbmMuMQswCQYDVQQGEwJVUzB2MBAGByqGSM49AgEGBSuBBAAiA2IABJjpLz1AcqTtkyJygRMc3RCV8cWjTnHcFBbZDuWmBSp3ZHtfTjjTuxxEtX/1H7YyYl3J6YRbTzBPEVoA/VhYDKX1DyxNB0cTddqXl5dvMVztK517IDvYuVTZXpmkOlEKMaNCMEAwHQYDVR0OBBYEFLuw3qFYM4iapIqZ3r6966/ayySrMA8GA1UdEwEB/wQFMAMBAf8wDgYDVR0PAQH/BAQDAgEGMAoGCCqGSM49BAMDA2gAMGUCMQCD6cHEFl4aXTQY2e3v9GwOAEZLuN+yRhHFD/3meoyhpmvOwgPUnPWTxnS4at+qIxUCMG1mihDK1A3UT82NQz60imOlM27jbdoXt2QfyFMm+YhidDkLF1vLUagM6BgD56KyKA==";

    private const string REAL_APPLE_INTERMEDIATE_BASE64 =
        "MIIDFjCCApygAwIBAgIUIsGhRwp0c2nvU4YSycafPTjzbNcwCgYIKoZIzj0EAwMwZzEbMBkGA1UEAwwSQXBwbGUgUm9vdCBDQSAtIEczMSYwJAYDVQQLDB1BcHBsZSBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTETMBEGA1UECgwKQXBwbGUgSW5jLjELMAkGA1UEBhMCVVMwHhcNMjEwMzE3MjAzNzEwWhcNMzYwMzE5MDAwMDAwWjB1MUQwQgYDVQQDDDtBcHBsZSBXb3JsZHdpZGUgRGV2ZWxvcGVyIFJlbGF0aW9ucyBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTELMAkGA1UECwwCRzYxEzARBgNVBAoMCkFwcGxlIEluYy4xCzAJBgNVBAYTAlVTMHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEbsQKC94PrlWmZXnXgtxzdVJL8T0SGYngDRGpngn3N6PT8JMEb7FDi4bBmPhCnZ3/sq6PF/cGcKXWsL5vOteRhyJ45x3ASP7cOB+aao90fcpxSv/EZFbniAbNgZGhIhpIo4H6MIH3MBIGA1UdEwEB/wQIMAYBAf8CAQAwHwYDVR0jBBgwFoAUu7DeoVgziJqkipnevr3rr9rLJKswRgYIKwYBBQUHAQEEOjA4MDYGCCsGAQUFBzABhipodHRwOi8vb2NzcC5hcHBsZS5jb20vb2NzcDAzLWFwcGxlcm9vdGNhZzMwNwYDVR0fBDAwLjAsoCqgKIYmaHR0cDovL2NybC5hcHBsZS5jb20vYXBwbGVyb290Y2FnMy5jcmwwHQYDVR0OBBYEFD8vlCNR01DJmig97bB85c+lkGKZMA4GA1UdDwEB/wQEAwIBBjAQBgoqhkiG92NkBgIBBAIFADAKBggqhkjOPQQDAwNoADBlAjBAXhSq5IyKogMCPtw490BaB677CaEGJXufQB/EqZGd6CSjiCtOnuMTbXVXmxxcxfkCMQDTSPxarZXvNrkxU3TkUMI33yzvFVVRT4wxWJC994OsdcZ4+RGNsYDyR5gmdr0nDGg=";

    private const string REAL_APPLE_SIGNING_CERT_BASE64 =
        "MIIEMTCCA7agAwIBAgIQR8KHzdn554Z/UoradNx9tzAKBggqhkjOPQQDAzB1MUQwQgYDVQQDDDtBcHBsZSBXb3JsZHdpZGUgRGV2ZWxvcGVyIFJlbGF0aW9ucyBDZXJ0aWZpY2F0aW9uIEF1dGhvcml0eTELMAkGA1UECwwCRzYxEzARBgNVBAoMCkFwcGxlIEluYy4xCzAJBgNVBAYTAlVTMB4XDTI1MDkxOTE5NDQ1MVoXDTI3MTAxMzE3NDcyM1owgZIxQDA+BgNVBAMMN1Byb2QgRUNDIE1hYyBBcHAgU3RvcmUgYW5kIGlUdW5lcyBTdG9yZSBSZWNlaXB0IFNpZ25pbmcxLDAqBgNVBAsMI0FwcGxlIFdvcmxkd2lkZSBEZXZlbG9wZXIgUmVsYXRpb25zMRMwEQYDVQQKDApBcHBsZSBJbmMuMQswCQYDVQQGEwJVUzBZMBMGByqGSM49AgEGCCqGSM49AwEHA0IABNnVvhcv7iT+7Ex5tBMBgrQspHzIsXRi0Yxfek7lv8wEmj/bHiWtNwJqc2BoHzsQiEjP7KFIIKg4Y8y0/nynuAmjggIIMIICBDAMBgNVHRMBAf8EAjAAMB8GA1UdIwQYMBaAFD8vlCNR01DJmig97bB85c+lkGKZMHAGCCsGAQUFBwEBBGQwYjAtBggrBgEFBQcwAoYhaHR0cDovL2NlcnRzLmFwcGxlLmNvbS93d2RyZzYuZGVyMDEGCCsGAQUFBzABhiVodHRwOi8vb2NzcC5hcHBsZS5jb20vb2NzcDAzLXd3ZHJnNjAyMIIBHgYDVR0gBIIBFTCCAREwggENBgoqhkiG92NkBQYBMIH+MIHDBggrBgEFBQcCAjCBtgyBs1JlbGlhbmNlIG9uIHRoaXMgY2VydGlmaWNhdGUgYnkgYW55IHBhcnR5IGFzc3VtZXMgYWNjZXB0YW5jZSBvZiB0aGUgdGhlbiBhcHBsaWNhYmxlIHN0YW5kYXJkIHRlcm1zIGFuZCBjb25kaXRpb25zIG9mIHVzZSwgY2VydGlmaWNhdGUgcG9saWN5IGFuZCBjZXJ0aWZpY2F0aW9uIHByYWN0aWNlIHN0YXRlbWVudHMuMDYGCCsGAQUFBwIBFipodHRwOi8vd3d3LmFwcGxlLmNvbS9jZXJ0aWZpY2F0ZWF1dGhvcml0eS8wHQYDVR0OBBYEFIFioG4wMMVA1ku9zJmGNPAVn3eqMA4GA1UdDwEB/wQEAwIHgDAQBgoqhkiG92NkBgsBBAIFADAKBggqhkjOPQQDAwNpADBmAjEA+qXnREC7hXIWVLsLxznjRpIzPf7VHz9V/CTm8+LJlrQepnmcPvGLNcX6XPnlcgLAAjEA5IjNZKgg5pQ79knF4IbTXdKv8vutIDMXDmjPVT3dGvFtsGRwXOywR2kZCdSrfeot";

    // October 2025 — within validity of test certs
    private static readonly DateTimeOffset EffectiveDate = DateTimeOffset.FromUnixTimeMilliseconds(1761962975000);

    #endregion

    #region Helpers

    private static X509Certificate2 CertFromBase64(string base64) => new(Convert.FromBase64String(base64));

    private static SignedDataVerifier CreateVerifier(
        string rootCertBase64,
        bool enableOnlineChecks)
    {
        return new SignedDataVerifier(
            [Convert.FromBase64String(rootCertBase64)],
            enableOnlineChecks,
            AppStoreEnvironment.Production);
    }

    #endregion

    #region Chain Verification Checks

    [Fact]
    public void ChainVerification_ValidChainWithoutOCSP()
    {
        var verifier = CreateVerifier(ROOT_CA_BASE64, enableOnlineChecks: false);
        var leaf = CertFromBase64(LEAF_CERT_BASE64);
        var intermediate = CertFromBase64(INTERMEDIATE_CA_BASE64);

        var publicKey = verifier.VerifyCertificateChain(leaf, intermediate, EffectiveDate);

        var expectedKeyBytes = Convert.FromBase64String(LEAF_CERT_PUBLIC_KEY_BASE64);
        var ecdsaKey = publicKey.ECDsa;
        var actualKeyBytes = ecdsaKey.ExportSubjectPublicKeyInfo();
        Assert.Equal(expectedKeyBytes, actualKeyBytes);
    }

    [Fact]
    public void ChainVerification_InvalidIntermediateOID_ThrowsVerificationFailure()
    {
        var verifier = CreateVerifier(ROOT_CA_BASE64, enableOnlineChecks: false);
        var leaf = CertFromBase64(LEAF_CERT_FOR_INTERMEDIATE_CA_INVALID_OID_BASE64);
        var intermediate = CertFromBase64(INTERMEDIATE_CA_INVALID_OID_BASE64);

        var ex = Assert.Throws<VerificationException>(() =>
            verifier.VerifyCertificateChain(leaf, intermediate, EffectiveDate));
        Assert.Equal(VerificationStatus.VerificationFailure, ex.Status);
    }

    [Fact]
    public void ChainVerification_InvalidLeafOID_ThrowsVerificationFailure()
    {
        var verifier = CreateVerifier(ROOT_CA_BASE64, enableOnlineChecks: false);
        var leaf = CertFromBase64(LEAF_CERT_INVALID_OID_BASE64);
        var intermediate = CertFromBase64(INTERMEDIATE_CA_BASE64);

        var ex = Assert.Throws<VerificationException>(() =>
            verifier.VerifyCertificateChain(leaf, intermediate, EffectiveDate));
        Assert.Equal(VerificationStatus.VerificationFailure, ex.Status);
    }

    [Fact]
    public void ChainVerification_EmptyRootCertArray_ThrowsVerificationFailure()
    {
        var verifier = new SignedDataVerifier([], false, AppStoreEnvironment.Production);
        var leaf = CertFromBase64(LEAF_CERT_BASE64);
        var intermediate = CertFromBase64(INTERMEDIATE_CA_BASE64);

        var ex = Assert.Throws<VerificationException>(() =>
            verifier.VerifyCertificateChain(leaf, intermediate, EffectiveDate));
        Assert.Equal(VerificationStatus.VerificationFailure, ex.Status);
    }

    [Fact]
    public void ChainVerification_ExpiredChain_Throws()
    {
        var verifier = CreateVerifier(ROOT_CA_BASE64, enableOnlineChecks: false);
        var leaf = CertFromBase64(LEAF_CERT_BASE64);
        var intermediate = CertFromBase64(INTERMEDIATE_CA_BASE64);
        var farFuture = DateTimeOffset.FromUnixTimeMilliseconds(2280946846000);

        var ex = Assert.Throws<VerificationException>(() =>
            verifier.VerifyCertificateChain(leaf, intermediate, farFuture));
        // .NET's X509Chain.Build fails before CheckDates, so we get VerificationFailure
        // instead of InvalidCertificate (which is what the Node library returns)
        Assert.True(
            ex.Status == VerificationStatus.VerificationFailure ||
            ex.Status == VerificationStatus.InvalidCertificate);
    }

    [Fact]
    [Trait("Category", "Network")]
    public void ChainVerification_RealChainWithOCSP()
    {
        var verifier = CreateVerifier(REAL_APPLE_ROOT_BASE64, enableOnlineChecks: true);
        var leaf = CertFromBase64(REAL_APPLE_SIGNING_CERT_BASE64);
        var intermediate = CertFromBase64(REAL_APPLE_INTERMEDIATE_BASE64);

        var publicKey = verifier.VerifyCertificateChain(leaf, intermediate, DateTimeOffset.UtcNow);
        Assert.NotNull(publicKey);
    }

    [Fact]
    public void ChainVerification_MismatchedRootCerts_ThrowsVerificationFailure()
    {
        var verifier = CreateVerifier(REAL_APPLE_ROOT_BASE64, enableOnlineChecks: false);
        var leaf = CertFromBase64(LEAF_CERT_BASE64);
        var intermediate = CertFromBase64(INTERMEDIATE_CA_BASE64);

        var ex = Assert.Throws<VerificationException>(() =>
            verifier.VerifyCertificateChain(leaf, intermediate, EffectiveDate));
        Assert.Equal(VerificationStatus.VerificationFailure, ex.Status);
    }

    [Fact]
    public void ChainVerification_InvalidRootCerts_Throws()
    {
        Assert.ThrowsAny<Exception>(() =>
        {
            var unused = new SignedDataVerifier(
                [new byte[] { 0x61, 0x62, 0x63 }],
                enableOnlineChecks: false,
                AppStoreEnvironment.Production);
        });
    }

    [Fact]
    [Trait("Category", "Network")]
    public void ChainVerification_CacheHit_ReturnsSameEntry()
    {
        var verifier = CreateVerifier(REAL_APPLE_ROOT_BASE64, enableOnlineChecks: true);
        var leaf = CertFromBase64(REAL_APPLE_SIGNING_CERT_BASE64);
        var intermediate = CertFromBase64(REAL_APPLE_INTERMEDIATE_BASE64);

        verifier.VerifyCertificateChain(leaf, intermediate, DateTimeOffset.UtcNow);
        Assert.Single(verifier.Cache);
        var entryAfterFirst = verifier.Cache.Values.Single();

        verifier.VerifyCertificateChain(leaf, intermediate, DateTimeOffset.UtcNow);
        Assert.Single(verifier.Cache);
        var entryAfterSecond = verifier.Cache.Values.Single();

        Assert.Same(entryAfterFirst, entryAfterSecond);
    }

    [Fact]
    [Trait("Category", "Network")]
    public void ChainVerification_CacheExpires_ReplacesEntry()
    {
        var verifier = CreateVerifier(REAL_APPLE_ROOT_BASE64, enableOnlineChecks: true);
        var leaf = CertFromBase64(REAL_APPLE_SIGNING_CERT_BASE64);
        var intermediate = CertFromBase64(REAL_APPLE_INTERMEDIATE_BASE64);

        verifier.VerifyCertificateChain(leaf, intermediate, DateTimeOffset.UtcNow);
        var entryBeforeExpiry = verifier.Cache.Values.Single();

        // Expire the cache entry
        var key = verifier.Cache.Keys.Single();
        lock (verifier.CacheLock)
        {
            verifier.Cache[key] = new SignedDataVerifier.CacheEntry(entryBeforeExpiry.PublicKey, 0);
        }

        verifier.VerifyCertificateChain(leaf, intermediate, DateTimeOffset.UtcNow);
        var entryAfterExpiry = verifier.Cache.Values.Single();

        Assert.NotSame(entryBeforeExpiry, entryAfterExpiry);
    }

    [Fact]
    public void ChainVerification_CacheKey_IncludesBothThumbprints()
    {
        var leaf = CertFromBase64(LEAF_CERT_BASE64);
        var intermediate = CertFromBase64(INTERMEDIATE_CA_BASE64);
        var otherIntermediate = CertFromBase64(INTERMEDIATE_CA_INVALID_OID_BASE64);

        var key1 = leaf.Thumbprint + ":" + intermediate.Thumbprint;
        var key2 = leaf.Thumbprint + ":" + otherIntermediate.Thumbprint;

        Assert.NotEqual(key1, key2);
    }

    #endregion

    #region Decoding Checks

    [Fact]
    public async Task VerifyAndDecodeNotificationAsync_MissingX5CHeader_ThrowsInvalidChainLength()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Production);
        var signedPayload = TestUtilities.ReadResourceAsString("mock_signed_data.missingX5CHeaderClaim");

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeNotificationAsync(signedPayload, "com.example", 1234,
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.InvalidChainLength, ex.Status);
    }

    [Fact]
    public async Task VerifyAndDecodeNotificationAsync_WrongBundleId_ThrowsInvalidBundleId()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);
        var signedPayload = TestUtilities.ReadResourceAsString("mock_signed_data.wrongBundleId");

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeNotificationAsync(signedPayload, "com.example", 1234,
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.InvalidBundleId, ex.Status);
    }

    [Fact]
    public async Task VerifyAndDecodeTransactionAsync_WrongBundleId_ThrowsInvalidBundleId()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);
        var signedTransaction = TestUtilities.ReadResourceAsString("mock_signed_data.transactionInfo");

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeTransactionAsync(signedTransaction, "com.example.x",
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.InvalidBundleId, ex.Status);
    }

    [Fact]
    public async Task VerifyAndDecodeTransactionAsync_OmittedBundleId_SkipsAppIdentifierValidation()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);
        var signedTransaction = TestUtilities.ReadResourceAsString("mock_signed_data.transactionInfo");

        var decoded = await verifier.VerifyAndDecodeTransactionAsync(signedTransaction,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("com.example", decoded.BundleId);
    }

    [Fact]
    public async Task VerifyAndDecodeNotificationAsync_WrongAppAppleId_ThrowsInvalidAppAppleId()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Production);
        var signedPayload = TestUtilities.ReadResourceAsString("mock_signed_data.testNotification");

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeNotificationAsync(signedPayload, "com.example", 9999,
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.InvalidAppAppleId, ex.Status);
    }

    [Fact]
    public async Task VerifyAndDecodeNotificationAsync_OmittedIdentifiers_SkipsAppIdentifierValidation()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);
        var signedPayload = TestUtilities.ReadResourceAsString("mock_signed_data.testNotification");

        var decoded = await verifier.VerifyAndDecodeNotificationAsync(signedPayload,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(NotificationTypeV2.Test, decoded.NotificationType);
    }

    [Fact]
    public async Task VerifyAndDecodeNotificationAsync_OnlyBundleId_ValidatesBundleIdIndependently()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);
        var signedPayload = TestUtilities.ReadResourceAsString("mock_signed_data.wrongBundleId");

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeNotificationAsync(signedPayload, "com.example",
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.InvalidBundleId, ex.Status);
    }

    [Fact]
    public async Task VerifyAndDecodeNotificationAsync_WrongEnvironment_ThrowsInvalidEnvironment()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Production);
        var signedPayload = TestUtilities.ReadResourceAsString("mock_signed_data.testNotification");

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeNotificationAsync(signedPayload, "com.example", 1234,
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.InvalidEnvironment, ex.Status);
    }

    [Fact]
    public async Task VerifyAndDecodeNotificationAsync_MalformedJwtFourParts_ThrowsVerificationFailure()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeNotificationAsync("a.b.c.d",
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.VerificationFailure, ex.Status);
    }

    [Fact]
    public async Task VerifyAndDecodeNotificationAsync_MalformedJwtBadPayload_ThrowsVerificationFailure()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);

        var ex = await Assert.ThrowsAsync<VerificationException>(() =>
            verifier.VerifyAndDecodeNotificationAsync("a.b.c",
                cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(VerificationStatus.VerificationFailure, ex.Status);
    }

    [Fact]
    public async Task VerifyAndDecodeNotificationAsync_ValidTestNotification_ReturnsDecodedPayload()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);
        var signedPayload = TestUtilities.ReadResourceAsString("mock_signed_data.testNotification");

        var decoded = await verifier.VerifyAndDecodeNotificationAsync(signedPayload, "com.example", 1234,
            cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal(NotificationTypeV2.Test, decoded.NotificationType);
    }

    [Fact]
    public async Task VerifyAndDecodeRenewalInfoAsync_ValidRenewalInfo_ReturnsDecodedPayload()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);
        var signedRenewalInfo = TestUtilities.ReadResourceAsString("mock_signed_data.renewalInfo");

        var decoded =
            await verifier.VerifyAndDecodeRenewalInfoAsync(signedRenewalInfo, TestContext.Current.CancellationToken);
        Assert.Equal(AppStoreEnvironment.Sandbox, decoded.Environment);
    }

    [Fact]
    public async Task VerifyAndDecodeRenewalInfoAsync_TransactionInfoAsRenewal_ReturnsDecodedPayload()
    {
        var verifier = TestUtilities.GetSignedPayloadVerifier(AppStoreEnvironment.Sandbox);
        var signedTransaction = TestUtilities.ReadResourceAsString("mock_signed_data.transactionInfo");

        var decoded =
            await verifier.VerifyAndDecodeRenewalInfoAsync(signedTransaction, TestContext.Current.CancellationToken);
        Assert.Equal(AppStoreEnvironment.Sandbox, decoded.Environment);
    }

    [Fact]
    public async Task VerifyAndDecodeRealtimeRequestAsync_ValidRequest_ReturnsAllFields()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signedPayload = TestUtilities.CreateSignedDataFromJson("models.decodedRealtimeRequest.json");

        var decoded = await verifier.VerifyAndDecodeRealtimeRequestAsync(signedPayload,
            cancellationToken: TestContext.Current.CancellationToken);
        Assert.Equal("99371282", decoded.OriginalTransactionId);
        Assert.Equal(531412L, decoded.AppAppleId);
        Assert.Equal("com.example.product", decoded.ProductId);
        Assert.Equal("en-US", decoded.UserLocale);
        Assert.Equal("3db5c98d-8acf-4e29-831e-8e1f82f9f6e9", decoded.RequestIdentifier);
        Assert.Equal(AppStoreEnvironment.LocalTesting, decoded.Environment);
        Assert.Equal(1698148900000L, decoded.SignedDate);
    }

    [Fact]
    public async Task VerifyAndDecodeRealtimeRequestAsync_OmittedAppAppleId_SkipsAppIdentifierValidation()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signedPayload = TestUtilities.CreateSignedDataFromJson("models.decodedRealtimeRequest.json");

        var decoded = await verifier.VerifyAndDecodeRealtimeRequestAsync(signedPayload,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal(531412L, decoded.AppAppleId);
    }

    [Fact]
    public async Task VerifyAndDecodeAppTransactionAsync_OmittedIdentifiers_SkipsAppIdentifierValidation()
    {
        var verifier = TestUtilities.GetDefaultSignedPayloadVerifier();
        var signedAppTransaction = TestUtilities.CreateSignedDataFromJson("models.appTransaction.json");

        var decoded = await verifier.VerifyAndDecodeAppTransactionAsync(signedAppTransaction,
            cancellationToken: TestContext.Current.CancellationToken);

        Assert.Equal("com.example", decoded.BundleId);
        Assert.Equal(531412L, decoded.AppAppleId);
    }

    #endregion
}
