using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Types of tax identification numbers supported by Fakturownia.pl.
/// Used in <see cref="InvoiceRequest.BuyerTaxNoKind"/> and <see cref="InvoiceRequest.SellerTaxNoKind"/>.
/// </summary>
public static class TaxNoKinds
{
    /// <summary>Standard Polish NIP (Default).</summary>
    public const string Nip = "";

    /// <summary>European VAT ID (NIP UE).</summary>
    public const string NipUe = "nip_ue";

    /// <summary>Other type of identification number.</summary>
    public const string Other = "other";

    /// <summary>
    /// No tax ID number
    /// </summary>
    public const string Empty = "empty";
}