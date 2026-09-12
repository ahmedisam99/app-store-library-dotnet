using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Enjna.AppStoreConnectApi.Models;

/// <summary>
/// The base class for the attributes of an update request, which carry the changes you are making
/// rather than the whole resource.
/// </summary>
/// <remarks>
/// <para>
/// A property you never assign stays out of the request, so Apple keeps the value it already has.
/// A property you assign <c>null</c> goes out as an explicit <c>null</c>, which asks Apple to clear
/// the stored value. C# can't tell those two apart on its own, so the setters record which
/// properties were assigned and only those reach the wire.
/// </para>
/// <para>
/// That makes assignment the thing that matters, not the value. Copying fields off another object
/// assigns every one of them, so a source field that happens to be <c>null</c> clears the stored
/// value instead of leaving it alone. Assign only the properties you mean to change.
/// </para>
/// <para>
/// Apple documents no behaviour for an explicit <c>null</c> on any attribute, and not every
/// attribute can be cleared. An endpoint that refuses one answers with an error you can catch. An
/// endpoint that ignores it answers 200 and keeps the stored value, so read the attributes on the
/// response when you expect a value to disappear.
/// </para>
/// </remarks>
public abstract class AttributeChangeSet
{
    private readonly Dictionary<string, object?> _values = new(StringComparer.Ordinal);

    private protected AttributeChangeSet()
    {
    }

    /// <summary>
    /// The names of the properties that were assigned, and that the request therefore carries.
    /// </summary>
    [JsonIgnore]
    public IReadOnlyCollection<string> AssignedAttributes => _values.Keys;

    /// <summary>
    /// Reports whether a property was assigned, whatever it was assigned to.
    /// </summary>
    /// <param name="attributeName">The name of the C# property, such as <c>EndDate</c>.</param>
    /// <returns><c>true</c> when the property was assigned and the request carries it.</returns>
    public bool IsAssigned(string attributeName)
    {
        return _values.ContainsKey(attributeName);
    }

    private protected T? Get<T>([CallerMemberName] string attributeName = "")
    {
        return _values.TryGetValue(attributeName, out var value) ? (T?)value : default;
    }

    private protected void Set(object? value, [CallerMemberName] string attributeName = "")
    {
        _values[attributeName] = value;
    }

    /// <summary>
    /// Teaches the serializer to write every assigned property, including one assigned <c>null</c>,
    /// and to leave out every property that was never assigned.
    /// </summary>
    /// <param name="typeInfo">The contract the serializer built for a type.</param>
    internal static void ConfigureChangeTracking(JsonTypeInfo typeInfo)
    {
        if (!typeof(AttributeChangeSet).IsAssignableFrom(typeInfo.Type))
        {
            return;
        }

        foreach (var property in typeInfo.Properties)
        {
            if (property.AttributeProvider is not PropertyInfo member)
            {
                continue;
            }

            var attributeName = member.Name;
            property.ShouldSerialize = (attributes, _) => ((AttributeChangeSet)attributes).IsAssigned(attributeName);
        }
    }
}
