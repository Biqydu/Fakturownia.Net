namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Allowed roles for invoice recipients, often used in KSeF or JST contexts.
/// </summary>
public static class RecipientRoles
{
    /// <summary>Standard recipient.</summary>
    public const string Recipient = "Odbiorca";

    public const string AdditionalBuyer = "Dodatkowy nabywca";

    /// <summary>Entity making the payment.</summary>
    public const string Payer = "Dokonujący płatności";

    /// <summary>Local Government Unit.</summary>
    public const string JstRecipient = "JST – odbiorca";

    /// <summary>VAT Group member.</summary>
    public const string GvMemberRecipient = "Członek GV – odbiorca";

    public const string Employee = "Pracownik";

    public const string Other = "Rola inna";
}