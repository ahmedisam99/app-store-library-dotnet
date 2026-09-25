using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppleSpec;

public static class ConnectInventory
{
    private const string ClientTypeName = "AppStoreConnectAPIClient";

    // MakeLinkRequestAsync is deliberately absent: it follows an opaque pagination cursor Apple
    // hands back in a response and has no path of its own.
    private static readonly string[] RequestHelpers =
    [
        "MakeRequestAsync",
        "MakeRawRequestAsync",
        "MakeRawTextRequestAsync"
    ];

    // Methods also carry links to Apple's articles; those encode no route and must not match here.
    private static readonly Regex EndpointSlug = new(
        @"^https://developer\.apple\.com/documentation/appstoreconnectapi/(get|post|patch|put|delete)-v[0-9]+(-|$)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static ConnectSurface Read(string projectDir)
    {
        var surface = new ConnectSurface();

        // Top directory only: bin/ and obj/ hold generated copies of these same files.
        var files = Directory.GetFiles(projectDir, $"{ClientTypeName}*.cs", SearchOption.TopDirectoryOnly);
        Array.Sort(files, StringComparer.Ordinal);

        foreach (var file in files)
        {
            ReadFile(file, surface);
        }

        return surface;
    }

    private static void ReadFile(string file, ConnectSurface surface)
    {
        var name = Path.GetFileName(file);
        var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file), new CSharpParseOptions(LanguageVersion.Latest));
        var root = tree.GetCompilationUnitRoot();

        foreach (var diagnostic in root.GetDiagnostics().Where(item => item.Severity == DiagnosticSeverity.Error))
        {
            surface.Unreduced.Add($"{name}: will not parse — {diagnostic.GetMessage()}");
        }

        foreach (var method in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
        {
            var calls = method.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(invocation => Array.IndexOf(RequestHelpers, CalleeName(invocation)) >= 0)
                .ToList();

            if (!IsOnClientType(method))
            {
                if (calls.Count > 0)
                {
                    surface.Unreduced.Add($"{name}:{method.Identifier.ValueText}: issues {calls.Count} request(s) from outside {ClientTypeName}");
                }

                continue;
            }

            if (!method.Modifiers.Any(SyntaxKind.PublicKeyword))
            {
                continue;
            }

            if (calls.Count == 0)
            {
                surface.NonEndpointMethods.Add(method.Identifier.ValueText);
                continue;
            }

            foreach (var call in calls)
            {
                Reduce(call, method, name, surface);
            }
        }
    }

    private static void Reduce(InvocationExpressionSyntax call, MethodDeclarationSyntax method, string file, ConnectSurface surface)
    {
        var methodName = method.Identifier.ValueText;
        var arguments = call.ArgumentList.Arguments;
        var pathArgument = arguments.FirstOrDefault(argument => argument.NameColon?.Name.Identifier.ValueText == "path");
        var verbArgument = arguments.FirstOrDefault(argument => argument.NameColon?.Name.Identifier.ValueText == "method");

        if (pathArgument is null || verbArgument is null)
        {
            surface.Unreduced.Add($"{file}:{methodName}: {CalleeName(call)} called without named `path:` and `method:` arguments");
            return;
        }

        if (!TryReadTemplate(pathArgument.Expression, out var template))
        {
            surface.Unreduced.Add($"{file}:{methodName}: path is not a literal or a simply interpolated string — {Compact(pathArgument.Expression)}");
            return;
        }

        if (verbArgument.Expression is not MemberAccessExpressionSyntax verb
            || verb.Expression is not IdentifierNameSyntax { Identifier.ValueText: "HttpMethod" })
        {
            surface.Unreduced.Add($"{file}:{methodName}: verb is not HttpMethod.X — {Compact(verbArgument.Expression)}");
            return;
        }

        surface.Calls.Add(new ConnectCall
        {
            Method = methodName,
            Verb = verb.Name.Identifier.ValueText.ToUpperInvariant(),
            Path = template,
            File = file,
            SeeAlso = SeeAlsoHref(method),
            Obsolete = IsObsolete(method.AttributeLists)
        });
    }

    private static bool TryReadTemplate(ExpressionSyntax expression, out string template)
    {
        template = "";

        if (expression is LiteralExpressionSyntax literal && literal.IsKind(SyntaxKind.StringLiteralExpression))
        {
            template = literal.Token.ValueText;
            return true;
        }

        if (expression is not InterpolatedStringExpressionSyntax interpolated
            || !interpolated.StringStartToken.IsKind(SyntaxKind.InterpolatedStringStartToken))
        {
            return false;
        }

        var text = new StringBuilder();

        foreach (var part in interpolated.Contents)
        {
            switch (part)
            {
                case InterpolatedStringTextSyntax literalPart:
                    text.Append(literalPart.TextToken.ValueText);
                    break;

                case InterpolationSyntax { AlignmentClause: null, FormatClause: null, Expression: IdentifierNameSyntax hole }:
                    text.Append('{').Append(hole.Identifier.ValueText).Append('}');
                    break;

                default:
                    return false;
            }
        }

        template = text.ToString();
        return true;
    }

    private static string? SeeAlsoHref(MethodDeclarationSyntax method)
    {
        string? found = null;

        foreach (var trivia in method.GetLeadingTrivia())
        {
            if (trivia.GetStructure() is not DocumentationCommentTriviaSyntax documentation)
            {
                continue;
            }

            foreach (var node in documentation.DescendantNodes())
            {
                var attributes = node switch
                {
                    XmlEmptyElementSyntax element when element.Name.LocalName.ValueText == "seealso" => element.Attributes,
                    XmlElementSyntax element when element.StartTag.Name.LocalName.ValueText == "seealso" => element.StartTag.Attributes,
                    _ => default
                };

                foreach (var attribute in attributes.OfType<XmlTextAttributeSyntax>())
                {
                    if (attribute.Name.LocalName.ValueText != "href")
                    {
                        continue;
                    }

                    var href = string.Concat(attribute.TextTokens.Select(token => token.ValueText)).Trim();

                    if (EndpointSlug.IsMatch(href))
                    {
                        found = href;
                    }
                }
            }
        }

        return found;
    }

    // Every model is a plain class or enum in Models/, so a type is found by its simple name. Nested
    // types would need qualifying, and the package declares none.
    public static Dictionary<string, ConnectModel> ReadModels(string projectDir)
    {
        var models = new Dictionary<string, ConnectModel>(StringComparer.Ordinal);
        var directory = Path.Combine(projectDir, "Models");

        if (!Directory.Exists(directory))
        {
            return models;
        }

        var files = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);
        Array.Sort(files, StringComparer.Ordinal);

        foreach (var file in files)
        {
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(file), new CSharpParseOptions(LanguageVersion.Latest)).GetCompilationUnitRoot();

            foreach (var type in root.DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
            {
                if (!type.Modifiers.Any(SyntaxKind.PublicKeyword))
                {
                    continue;
                }

                var model = new ConnectModel
                {
                    Name = type.Identifier.ValueText,
                    File = Path.GetFileName(file),
                    Obsolete = IsObsolete(type.AttributeLists)
                };

                foreach (var property in type.ChildNodes().OfType<PropertyDeclarationSyntax>())
                {
                    var wireName = JsonPropertyName(property.AttributeLists) ?? property.Identifier.ValueText;
                    model.Properties[wireName] = IsObsolete(property.AttributeLists);
                }

                models[model.Name] = model;
            }
        }

        return models;
    }

    private static bool IsObsolete(SyntaxList<AttributeListSyntax> lists) =>
        lists.SelectMany(list => list.Attributes).Any(attribute => AttributeName(attribute) is "Obsolete" or "ObsoleteAttribute");

    private static string? JsonPropertyName(SyntaxList<AttributeListSyntax> lists)
    {
        var attribute = lists.SelectMany(list => list.Attributes)
            .FirstOrDefault(attribute => AttributeName(attribute) is "JsonPropertyName" or "JsonPropertyNameAttribute");

        return attribute?.ArgumentList?.Arguments.FirstOrDefault()?.Expression is LiteralExpressionSyntax literal
            ? literal.Token.ValueText
            : null;
    }

    private static string AttributeName(AttributeSyntax attribute) => attribute.Name switch
    {
        QualifiedNameSyntax qualified => qualified.Right.Identifier.ValueText,
        SimpleNameSyntax simple => simple.Identifier.ValueText,
        _ => attribute.Name.ToString()
    };

    private static bool IsOnClientType(MethodDeclarationSyntax method) =>
        method.Parent is ClassDeclarationSyntax type
        && type.Identifier.ValueText == ClientTypeName
        && type.Modifiers.Any(SyntaxKind.PartialKeyword);

    private static string? CalleeName(InvocationExpressionSyntax invocation) => invocation.Expression switch
    {
        SimpleNameSyntax name => name.Identifier.ValueText,
        MemberAccessExpressionSyntax access => access.Name.Identifier.ValueText,
        _ => null
    };

    private static string Compact(ExpressionSyntax expression)
    {
        var text = string.Join(" ", expression.ToString().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
        return text.Length <= 120 ? text : text[..117] + "...";
    }
}

public sealed class ConnectSurface
{
    public List<ConnectCall> Calls { get; init; } = [];

    public List<string> NonEndpointMethods { get; init; } = [];

    // Any entry here fails the check: a skipped call site shrinks the denominator and makes every
    // other count a lie.
    public List<string> Unreduced { get; init; } = [];
}

public sealed class ConnectCall
{
    public required string Method { get; init; }
    public required string Verb { get; init; }

    public required string Path { get; init; }

    public required string File { get; init; }

    public string? SeeAlso { get; init; }

    public bool Obsolete { get; init; }
}

public sealed class ConnectModel
{
    public required string Name { get; init; }

    public required string File { get; init; }

    public bool Obsolete { get; init; }

    // Keyed by wire name, so a property is matched to Apple's member without guessing at casing.
    public Dictionary<string, bool> Properties { get; } = new(StringComparer.Ordinal);
}
