using System;
using System.Collections.Generic;
using System.Globalization;

namespace Enjna.AppStoreConnectApi;

/// <summary>
/// A fluent builder for the query parameters that App Store Connect API requests accept, such as
/// sparse fieldsets, filters, includes, sorting, and paging limits.
/// </summary>
/// <remarks>
/// Every method that takes values requires at least one value, and rejects a value that is
/// <c>null</c> or blank. A parameter with no values can't be sent — it would be left out of the
/// request — and leaving a filter out widens the request to the whole collection instead of
/// narrowing it, which is the dangerous direction to fail in. So a filter built from an empty
/// collection is a mistake the builder reports where the mistake was made, rather than a request
/// that quietly returns everything.
/// </remarks>
/// <seealso href="https://developer.apple.com/documentation/appstoreconnectapi/large-data-sets"/>
public sealed class AppStoreConnectQuery
{
    private readonly Dictionary<string, string[]> _parameters = new(StringComparer.Ordinal);

    /// <summary>
    /// Requests a sparse fieldset, limiting the response to the given fields of one resource type.
    /// Requesting fields for the same resource type twice replaces the earlier fields, so pass
    /// every field in one call.
    /// </summary>
    /// <param name="resourceType">The resource type the fields belong to, such as <c>apps</c>.</param>
    /// <param name="fields">The attribute and relationship names to return, at least one.</param>
    /// <returns>The same query, so you can chain calls.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the fields are <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when no fields are given, or a field is blank.</exception>
    public AppStoreConnectQuery Fields(string resourceType, params string[] fields)
    {
        var name = $"fields[{resourceType}]";
        RequireValues(name, fields, nameof(fields));

        return SetParameter(name, fields);
    }

    /// <summary>
    /// Filters the response by a field. Several values for one field are matched as a logical OR.
    /// Filtering the same field twice replaces the earlier values, so pass every value in one call.
    /// </summary>
    /// <param name="field">The attribute, relationship, or ID to filter by, such as <c>bundleId</c>.</param>
    /// <param name="values">The values to match, at least one.</param>
    /// <returns>The same query, so you can chain calls.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the values are <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when no values are given, or a value is blank.</exception>
    public AppStoreConnectQuery Filter(string field, params string[] values)
    {
        var name = $"filter[{field}]";
        RequireValues(name, values, nameof(values));

        return SetParameter(name, values);
    }

    /// <summary>
    /// Filters the response by the existence of a relationship.
    /// </summary>
    /// <param name="field">The relationship to test, such as <c>gameCenterEnabledVersions</c>.</param>
    /// <param name="value">The value to match, usually <c>true</c> or <c>false</c>.</param>
    /// <returns>The same query, so you can chain calls.</returns>
    /// <exception cref="ArgumentException">Thrown when the value is <c>null</c> or blank.</exception>
    public AppStoreConnectQuery Exists(string field, string value)
    {
        var name = $"exists[{field}]";
        var values = new[] { value };
        RequireValues(name, values, nameof(value));

        return SetParameter(name, values);
    }

    /// <summary>
    /// Includes related resources in the response's <c>included</c> array.
    /// </summary>
    /// <param name="relationships">The relationship names to include, such as <c>builds</c>, at least one.</param>
    /// <returns>The same query, so you can chain calls.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the relationships are <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when no relationships are given, or one is blank.</exception>
    public AppStoreConnectQuery Include(params string[] relationships)
    {
        RequireValues("include", relationships, nameof(relationships));

        return SetParameter("include", relationships);
    }

    /// <summary>
    /// Sorts the response. Prefix a sort expression with <c>-</c> to sort in descending order.
    /// </summary>
    /// <param name="sorts">The sort expressions, such as <c>-bundleId</c>, at least one.</param>
    /// <returns>The same query, so you can chain calls.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the sort expressions are <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when no sort expressions are given, or one is blank.</exception>
    public AppStoreConnectQuery Sort(params string[] sorts)
    {
        RequireValues("sort", sorts, nameof(sorts));

        return SetParameter("sort", sorts);
    }

    /// <summary>
    /// Limits the number of resources the response returns per page. Most endpoints allow up to 200.
    /// </summary>
    /// <param name="limit">The maximum number of resources to return.</param>
    /// <returns>The same query, so you can chain calls.</returns>
    public AppStoreConnectQuery Limit(int limit)
    {
        return SetParameter("limit", new[] { limit.ToString(CultureInfo.InvariantCulture) });
    }

    /// <summary>
    /// Limits the number of related resources the response returns for one included relationship.
    /// It only takes effect together with <see cref="Include"/>.
    /// </summary>
    /// <param name="relationship">The relationship to limit, such as <c>builds</c>.</param>
    /// <param name="limit">The maximum number of related resources to return.</param>
    /// <returns>The same query, so you can chain calls.</returns>
    public AppStoreConnectQuery Limit(string relationship, int limit)
    {
        return SetParameter(
            $"limit[{relationship}]",
            new[] { limit.ToString(CultureInfo.InvariantCulture) });
    }

    /// <summary>
    /// Sets an arbitrary query parameter, for anything the other methods don't cover. Setting the
    /// same parameter twice replaces the earlier value.
    /// </summary>
    /// <param name="name">The full parameter name, such as <c>filter[vendorNumber]</c>.</param>
    /// <param name="values">The parameter values, at least one, which are sent as one comma-joined parameter.</param>
    /// <returns>The same query, so you can chain calls.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the name or the values are <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when no values are given, or a value is blank.</exception>
    public AppStoreConnectQuery Parameter(string name, params string[] values)
    {
        ArgumentNullException.ThrowIfNull(name);
        RequireValues(name, values, nameof(values));

        return SetParameter(name, values);
    }

    /// <summary>
    /// Builds the query parameters this query represents.
    /// </summary>
    /// <returns>A new dictionary of parameter names and their values.</returns>
    public Dictionary<string, string[]> ToQueryParameters()
    {
        var parameters = new Dictionary<string, string[]>(_parameters.Count, StringComparer.Ordinal);

        foreach (var (name, values) in _parameters)
        {
            parameters[name] = (string[])values.Clone();
        }

        return parameters;
    }

    /// <summary>
    /// Checks that a parameter can actually be sent: it has at least one value, and every value
    /// carries something. A parameter that fails either check would be dropped on its way to the
    /// wire, and a dropped <c>filter</c> asks for the whole collection, so it's reported here —
    /// in the caller's own call — instead of at the request.
    /// </summary>
    /// <param name="name">The full parameter name, for the message.</param>
    /// <param name="values">The values the caller gave.</param>
    /// <param name="parameterName">The name of the caller's parameter the values came from.</param>
    /// <exception cref="ArgumentNullException">Thrown when the values are <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when no values are given, or a value is blank.</exception>
    private static void RequireValues(string name, string[] values, string parameterName)
    {
        ArgumentNullException.ThrowIfNull(values, parameterName);

        if (values.Length == 0)
        {
            throw new ArgumentException(
                $"The query parameter \"{name}\" was given no values. A parameter with no values " +
                "can't be sent, and leaving a filter out of a request returns the whole " +
                "collection, so pass at least one value or leave the parameter off the query.",
                parameterName);
        }

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    $"The query parameter \"{name}\" has a null or blank value.",
                    parameterName);
            }
        }
    }

    /// <summary>
    /// Stores already validated values under a parameter name, keeping a copy of the array so that
    /// the caller mutating theirs afterwards can't change what the query sends.
    /// </summary>
    /// <param name="name">The full parameter name.</param>
    /// <param name="values">The validated values.</param>
    /// <returns>The same query, so you can chain calls.</returns>
    private AppStoreConnectQuery SetParameter(string name, string[] values)
    {
        _parameters[name] = (string[])values.Clone();

        return this;
    }
}
