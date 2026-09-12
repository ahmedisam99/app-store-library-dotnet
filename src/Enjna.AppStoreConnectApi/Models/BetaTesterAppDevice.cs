using System.Text.Json.Serialization;
using Enjna.AppStoreConnectApi.Models.Enums;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// A device that a beta tester uses to test an app.
/// </summary>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/betatester/attributes"/>
public sealed class BetaTesterAppDevice
{
    /// <summary>
    /// The model of the device, such as <c>iPhone 15 Pro</c>.
    /// </summary>
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    /// <summary>
    /// The platform the device runs.
    /// </summary>
    [JsonPropertyName("platform")]
    public BetaTesterAppDevicePlatform? Platform { get; set; }

    /// <summary>
    /// The version of the operating system the device runs.
    /// </summary>
    [JsonPropertyName("osVersion")]
    public string? OsVersion { get; set; }

    /// <summary>
    /// The version of the build the beta tester installed on the device.
    /// </summary>
    [JsonPropertyName("appBuildVersion")]
    public string? AppBuildVersion { get; set; }
}
