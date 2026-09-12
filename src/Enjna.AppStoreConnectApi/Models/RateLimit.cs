using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The rate-limit information that the App Store Connect API returns in the <c>X-Rate-Limit</c>
/// header of every response.
/// </summary>
/// <remarks>
/// A response reports the budget that governed it and no other, so the pair it didn't report reads
/// as <c>null</c>. Reads were measured reporting the hourly pair alone and a write the per-minute
/// pair alone. A <c>null</c> budget means this response reported no figure for it, never that the
/// budget is unlimited.
/// </remarks>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/identifying-rate-limits"/>
public sealed class RateLimit
{
    private const string UserHourLimitKey = "user-hour-lim";
    private const string UserHourRemainingKey = "user-hour-rem";
    private const string UserMinuteLimitKey = "user-minute-lim";
    private const string UserMinuteRemainingKey = "user-minute-rem";

    private readonly Dictionary<string, int> _fields = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// The number of requests the API key may send within a rolling hour.
    /// </summary>
    [JsonPropertyName("userHourLimit")]
    public int? UserHourLimit => FindField(UserHourLimitKey);

    /// <summary>
    /// The number of requests the API key may still send within the current rolling hour.
    /// </summary>
    [JsonPropertyName("userHourRemaining")]
    public int? UserHourRemaining => FindField(UserHourRemainingKey);

    /// <summary>
    /// The number of requests the API key may send within a minute.
    /// </summary>
    /// <remarks>
    /// Apple's rate limit page documents the hourly keys alone. This pair was measured on a write,
    /// which answered <c>user-minute-lim:250;user-minute-rem:244;</c>.
    /// </remarks>
    [JsonPropertyName("userMinuteLimit")]
    public int? UserMinuteLimit => FindField(UserMinuteLimitKey);

    /// <summary>
    /// The number of requests the API key may still send within the current minute.
    /// </summary>
    [JsonPropertyName("userMinuteRemaining")]
    public int? UserMinuteRemaining => FindField(UserMinuteRemainingKey);

    /// <summary>
    /// Every field the header carried, under Apple's own name for it and looked up without regard
    /// to case. The typed properties are lookups into this, so a budget Apple starts reporting
    /// after this version reaches a caller here without a change to this library. A field whose
    /// value isn't an integer is absent from here and survives in <see cref="Raw"/> alone.
    /// </summary>
    [JsonPropertyName("fields")]
    public IReadOnlyDictionary<string, int> Fields => _fields;

    /// <summary>
    /// The header value exactly as Apple sent it, which preserves any field <see cref="Fields"/>
    /// couldn't read.
    /// </summary>
    [JsonPropertyName("raw")]
    public string? Raw { get; set; }

    /// <summary>
    /// Parses an <c>X-Rate-Limit</c> header value, such as <c>user-hour-lim:3500;user-hour-rem:500;</c>.
    /// </summary>
    /// <param name="header">The raw header value.</param>
    /// <returns>The parsed rate-limit information, or <c>null</c> when the header is absent or empty.</returns>
    public static RateLimit? Parse(string? header)
    {
        if (string.IsNullOrWhiteSpace(header))
        {
            return null;
        }

        var rateLimit = new RateLimit
        {
            Raw = header
        };

        var segments = header.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var segment in segments)
        {
            var separatorIndex = segment.IndexOf(':');

            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = segment[..separatorIndex].Trim();
            var value = segment[(separatorIndex + 1)..].Trim();

            if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            {
                continue;
            }

            rateLimit._fields[key] = parsed;
        }

        return rateLimit;
    }

    private int? FindField(string key)
    {
        if (_fields.TryGetValue(key, out var value))
        {
            return value;
        }

        return null;
    }
}
