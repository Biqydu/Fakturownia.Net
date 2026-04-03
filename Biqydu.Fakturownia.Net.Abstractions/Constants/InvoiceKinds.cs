using Biqydu.Fakturownia.Net.Abstractions.Models;

namespace Biqydu.Fakturownia.Net.Abstractions.Constants;

/// <summary>
/// Document types supported by Fakturownia.pl.
/// These values can be used in <see cref="InvoiceRequest.Kind"/>.
/// </summary>
public static class InvoiceKinds
{
    /// <summary>Standard VAT invoice.</summary>
    public const string Vat = "vat";

    /// <summary>Proforma invoice.</summary>
    public const string Proforma = "proforma";

    /// <summary>Non-VAT bill or simplified invoice.</summary>
    public const string Bill = "bill";

    /// <summary>Fiscal receipt.</summary>
    public const string Receipt = "receipt";

    /// <summary>Advance payment invoice.</summary>
    public const string Advance = "advance";

    /// <summary>Correction invoice.</summary>
    public const string Correction = "correction";

    /// <summary>VAT MP invoice (Method of payment - Faktura VAT MP).</summary>
    public const string VatMp = "vat_mp";

    /// <summary>Other type of invoice.</summary>
    public const string InvoiceOther = "invoice_other";

    /// <summary>VAT margin invoice (e.g., for used goods or travel agencies).</summary>
    public const string VatMargin = "vat_margin";

    /// <summary>Cash Received document.</summary>
    public const string Kp = "kp";

    /// <summary>Cash Paid document.</summary>
    public const string Kw = "kw";

    /// <summary>Final invoice issued after an advance payment.</summary>
    public const string Final = "final";

    /// <summary>Estimated invoice or quote.</summary>
    public const string Estimate = "estimate";
}