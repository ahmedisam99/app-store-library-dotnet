using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Enjna.AppStoreConnectApi;

/// <summary>
/// A JSON converter for enums that uses <see cref="EnumMemberAttribute"/> values for serialization.
/// This is needed because <see cref="JsonStringEnumConverter{TEnum}"/> does not support
/// <see cref="EnumMemberAttribute"/> in .NET 8.
/// </summary>
internal sealed class JsonEnumMemberConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    private static readonly Dictionary<string, TEnum> NameToValue = new(StringComparer.OrdinalIgnoreCase);
    private static readonly Dictionary<TEnum, string> ValueToName = new();
    private static readonly TEnum? UnmappedValue;

    static JsonEnumMemberConverter()
    {
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var value = (TEnum)field.GetValue(null)!;
            var attr = field.GetCustomAttribute<EnumMemberAttribute>();
            var name = attr?.Value ?? field.Name;
            NameToValue[name] = value;
            ValueToName[value] = name;

            if (field.Name == "_Unmapped")
            {
                UnmappedValue = value;
            }
        }
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException($"Unable to convert a {reader.TokenType} token to {typeof(TEnum)}");
        }

        var str = reader.GetString()!;
        if (NameToValue.TryGetValue(str, out var value))
        {
            return value;
        }

        if (UnmappedValue.HasValue)
        {
            return UnmappedValue.Value;
        }

        throw new JsonException($"Unable to convert \"{str}\" to {typeof(TEnum)}");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (UnmappedValue.HasValue && EqualityComparer<TEnum>.Default.Equals(value, UnmappedValue.Value))
        {
            throw new JsonException(
                $"Can't serialize {typeof(TEnum)}._Unmapped. It stands for a value this version of the library "
                    + "doesn't recognize, so it has no value to send. Set the property to null, or to a value this "
                    + "version recognizes, before sending it.");
        }

        if (ValueToName.TryGetValue(value, out var name))
        {
            writer.WriteStringValue(name);
        }
        else
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
