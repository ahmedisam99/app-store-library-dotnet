using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppleSpec;

public static class ServerInventory
{
    public static NetInventory Read(string projectDir)
    {
        var inventory = new NetInventory();

        foreach (var file in SourceFiles(projectDir))
        {
            var relative = Path.GetRelativePath(projectDir, file).Replace('\\', '/');
            var root = CSharpSyntaxTree.ParseText(File.ReadAllText(file)).GetRoot();

            foreach (var node in root.DescendantNodes())
            {
                switch (node)
                {
                    case TypeDeclarationSyntax type and (ClassDeclarationSyntax or RecordDeclarationSyntax or StructDeclarationSyntax):
                        inventory.Types.Add(ReadType(type, relative));
                        break;

                    case EnumDeclarationSyntax @enum:
                        inventory.Enums.Add(ReadEnum(@enum, relative));
                        break;

                    case MethodDeclarationSyntax method:
                        inventory.Endpoints.AddRange(ReadEndpoints(method, relative));
                        break;
                }
            }
        }

        Flatten(inventory);

        return inventory;
    }

    private static IEnumerable<string> SourceFiles(string projectDir) =>
        Directory.EnumerateFiles(projectDir, "*.cs", SearchOption.AllDirectories)
            .Where(file => !file.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                && !file.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal))
            .OrderBy(file => file, StringComparer.Ordinal);

    private static NetType ReadType(TypeDeclarationSyntax type, string file)
    {
        var properties = type.Members
            .OfType<PropertyDeclarationSyntax>()
            .Where(IsPublicInstance)
            .Select(ReadProperty)
            .ToList();

        return new NetType
        {
            Name = QualifiedName(type),
            File = file,
            Base = BaseName(type),
            Internal = IsInternal(type),
            Properties = properties
        };
    }

    private static NetProperty ReadProperty(PropertyDeclarationSyntax property)
    {
        var wire = HasAttribute(property.AttributeLists, "JsonIgnore")
            ? null
            : AttributeArgument(property.AttributeLists, "JsonPropertyName") ?? property.Identifier.Text;

        return new NetProperty
        {
            Clr = property.Identifier.Text,
            Wire = wire,
            Type = property.Type.ToString()
        };
    }

    private static NetEnum ReadEnum(EnumDeclarationSyntax @enum, string file)
    {
        var stringBacked = @enum.AttributeLists
            .SelectMany(list => list.Attributes)
            .Any(attribute => attribute.ToString().Contains("JsonEnumMemberConverter", StringComparison.Ordinal));

        var members = new List<NetEnumMember>();
        var ordinal = 0L;

        foreach (var member in @enum.Members)
        {
            if (member.EqualsValue?.Value is LiteralExpressionSyntax literal
                && long.TryParse(literal.Token.ValueText, out var assigned))
            {
                ordinal = assigned;
            }

            // _Unmapped is the forward-compatibility sentinel: it stands for a value Apple has added
            // and this library has not mapped, so it has no wire spelling of its own.
            var sentinel = member.Identifier.Text == "_Unmapped";

            members.Add(new NetEnumMember
            {
                Clr = member.Identifier.Text,
                Wire = sentinel
                    ? null
                    : stringBacked
                        ? AttributeArgument(member.AttributeLists, "EnumMember")
                        : ordinal.ToString(System.Globalization.CultureInfo.InvariantCulture),
                Sentinel = sentinel
            });

            ordinal++;
        }

        return new NetEnum
        {
            Name = QualifiedName(@enum),
            File = file,
            Backing = stringBacked ? "string" : "int",
            Members = members
        };
    }

    private static IEnumerable<NetEndpoint> ReadEndpoints(MethodDeclarationSyntax method, string file)
    {
        foreach (var invocation in method.DescendantNodes().OfType<InvocationExpressionSyntax>())
        {
            if (InvokedName(invocation.Expression) != "MakeRequestAsync")
            {
                continue;
            }

            var path = NamedArgument(invocation, "path");
            var verb = NamedArgument(invocation, "method");

            if (path is null || verb is null)
            {
                continue;
            }

            yield return new NetEndpoint
            {
                Method = method.Identifier.Text,
                Verb = PathText.Verb(verb),
                Path = PathText.Literal(path),
                File = file
            };
        }
    }

    // Deduplication is by wire name: DecodedRealtimeRequestBody redeclares signedDate as
    // non-nullable with new, so the flattened hierarchy would otherwise hold it twice.
    private static void Flatten(NetInventory inventory)
    {
        var byName = new Dictionary<string, NetType>(StringComparer.Ordinal);

        foreach (var type in inventory.Types)
        {
            byName[type.Name] = type;
        }

        foreach (var type in inventory.Types)
        {
            var seen = new HashSet<string>(type.Properties.Select(property => property.Wire ?? property.Clr), StringComparer.Ordinal);
            var ancestor = type.Base;
            var depth = 0;

            while (ancestor is not null && byName.TryGetValue(ancestor, out var parent) && depth++ < 16)
            {
                foreach (var property in parent.Properties.Where(property => seen.Add(property.Wire ?? property.Clr)))
                {
                    type.Properties.Add(property);
                }

                ancestor = parent.Base;
            }
        }
    }

    private static bool IsPublicInstance(PropertyDeclarationSyntax property) =>
        property.Modifiers.Any(SyntaxKind.PublicKeyword) && !property.Modifiers.Any(SyntaxKind.StaticKeyword);

    private static bool IsInternal(TypeDeclarationSyntax type) => !type.Modifiers.Any(SyntaxKind.PublicKeyword);

    private static string QualifiedName(BaseTypeDeclarationSyntax type)
    {
        var name = type.Identifier.Text;

        if (type is TypeDeclarationSyntax { TypeParameterList: { } parameters })
        {
            name += parameters.ToString();
        }

        for (var outer = type.Parent; outer is not null; outer = outer.Parent)
        {
            if (outer is BaseTypeDeclarationSyntax parent)
            {
                name = $"{parent.Identifier.Text}.{name}";
            }
        }

        return name;
    }

    private static string? BaseName(TypeDeclarationSyntax type) => type.BaseList?.Types.FirstOrDefault()?.Type.ToString();

    private static bool HasAttribute(SyntaxList<AttributeListSyntax> lists, string name) =>
        lists.SelectMany(list => list.Attributes).Any(attribute => AttributeName(attribute) == name);

    private static string? AttributeArgument(SyntaxList<AttributeListSyntax> lists, string name) =>
        lists.SelectMany(list => list.Attributes)
            .Where(attribute => AttributeName(attribute) == name)
            .SelectMany(attribute => attribute.ArgumentList?.Arguments ?? default)
            .Select(argument => argument.Expression)
            .OfType<LiteralExpressionSyntax>()
            .Where(literal => literal.IsKind(SyntaxKind.StringLiteralExpression))
            .Select(literal => literal.Token.ValueText)
            .FirstOrDefault();

    private static string AttributeName(AttributeSyntax attribute)
    {
        var name = attribute.Name.ToString();
        var separator = name.LastIndexOf('.');

        return separator >= 0 ? name[(separator + 1)..] : name;
    }

    private static string? InvokedName(ExpressionSyntax expression) => expression switch
    {
        IdentifierNameSyntax identifier => identifier.Identifier.Text,
        GenericNameSyntax generic => generic.Identifier.Text,
        MemberAccessExpressionSyntax member => InvokedName(member.Name),
        _ => null
    };

    private static ExpressionSyntax? NamedArgument(InvocationExpressionSyntax invocation, string name) =>
        invocation.ArgumentList.Arguments
            .FirstOrDefault(argument => argument.NameColon?.Name.Identifier.Text == name)?.Expression;
}

internal static class PathText
{
    public static string Literal(ExpressionSyntax expression) => expression switch
    {
        LiteralExpressionSyntax literal => literal.Token.ValueText,
        InterpolatedStringExpressionSyntax interpolated => string.Concat(interpolated.Contents.Select(content => content switch
        {
            InterpolatedStringTextSyntax text => text.TextToken.ValueText,
            InterpolationSyntax hole => $"{{{hole.Expression}}}",
            _ => ""
        })),
        _ => expression.ToString()
    };

    public static string Verb(ExpressionSyntax expression) => expression switch
    {
        MemberAccessExpressionSyntax member => member.Name.Identifier.Text.ToUpperInvariant(),
        _ => expression.ToString().ToUpperInvariant()
    };
}

public sealed class NetInventory
{
    public List<NetType> Types { get; init; } = [];
    public List<NetEnum> Enums { get; init; } = [];
    public List<NetEndpoint> Endpoints { get; init; } = [];
}

public sealed class NetType
{
    public required string Name { get; init; }
    public required string File { get; init; }
    public string? Base { get; init; }
    public bool Internal { get; init; }
    public List<NetProperty> Properties { get; init; } = [];
}

public sealed class NetProperty
{
    public required string Clr { get; init; }

    public string? Wire { get; init; }

    public required string Type { get; init; }
}

public sealed class NetEnum
{
    public required string Name { get; init; }
    public required string File { get; init; }
    public required string Backing { get; init; }
    public List<NetEnumMember> Members { get; init; } = [];
}

public sealed class NetEnumMember
{
    public required string Clr { get; init; }
    public string? Wire { get; init; }
    public bool Sentinel { get; init; }
}

public sealed class NetEndpoint
{
    public required string Method { get; init; }
    public required string Verb { get; init; }
    public required string Path { get; init; }
    public required string File { get; init; }
}
