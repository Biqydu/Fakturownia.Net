namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Allowed roles for invoice issuers, often used in KSeF or factoring contexts.
/// </summary>
public static class IssuerRoles
{
    /// <summary>Standard invoice issuer.</summary>
    public const string Issuer = "Wystawca faktury";

    /// <summary>Factor - entity providing factoring services.</summary>
    public const string Factor = "Faktor";

    /// <summary>Original issuer.</summary>
    public const string OriginalEntity = "Podmiot pierwotny";

    /// <summary>Local Government Unit.</summary>
    public const string JstIssuer = "JST – wystawca";

    /// <summary>VAT Group member.</summary>
    public const string GvMemberIssuer = "Członek GV – wystawca";

    public const string Other = "Rola inna";
}